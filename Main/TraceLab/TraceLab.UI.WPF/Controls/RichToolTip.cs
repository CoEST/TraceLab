// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;


namespace TraceLab.UI.WPF.Controls
{
        /// <summary>
        /// A kind-of Tooltip implementation that stays show once element is hovered and the content inside is responsive
        /// 
        /// It corresponds to most of the TooltipService attached properties so use them as you wish
        /// 
        /// Usage: (Like Tooltip)
        /// <![CDATA[
        /// <Control ToolTipService.Placement="Right">
        ///     <xmlns-pfx:RichToolTip.PopupContent>
        ///         <TextBlock Text="This will be displayed in the popup" />
        ///     </xmlns-pfx:RichToolTip.PopupContent>
        /// </Control>
        /// 
        /// <Control ToolTipService.Placement="Right">
        ///     <xmlns-pfx:RichToolTip.PopupContent>
        ///         <RichToolTip Placement="..." PlacementTarget="..." HorizontalOffset=".." and so on>
        ///             <TextBlock Text="This will be displayed in the popup" />
        ///         </RichToolTip>
        ///     </xmlns-pfx:RichToolTip.PopupContent>
        /// </Control>
        /// ]]>
        /// 
        /// Known Issues:
        /// 1 - I didn't have the time nor the strength to care about repositioning. I simply hide the popup whenever it would need repositioning. (Window movement, etc..) But it's ok since it's the default behavior of popup overall.
        /// 2 - XBap mode sets transparency through a hack! supported only in full trust.
        /// 3 - In XBap mode, moving the mouse slowly towards the popup will cause it to hide
        /// 4 - In XBap mode, moving the mouse over the element shows the tooltip even when the browser isn't the active window
        /// </summary>
        /// 
        public partial class RichToolTip : ContentControl
        {
            #region Fields
            const int AnimationDurationInMs = 200;
            const int ShowDeferredMilliseconds = 500;
            const bool AnimationEnabledDefault = true;

            delegate void Action();
            Popup _parentPopup;

            static RichToolTip lastShownPopup = null;
            #endregion

            #region Properties
            //UIElement _relatedObject;
            public UIElement RelatedObject
            {
                get
                {
                    return (UIElement)this.GetValue(RelatedObjectProperty);
                }
                set
                {
                    this.SetValue(RelatedObjectProperty, value);
                }
            }
            public static readonly DependencyProperty RelatedObjectProperty = DependencyProperty.Register("RelatedObject", typeof(UIElement), typeof(RichToolTip), new FrameworkPropertyMetadata());
                

            private bool enableAnimation = AnimationEnabledDefault;
            public bool EnableAnimation
            {
                get { return enableAnimation; }
                set { enableAnimation = value; }
            }
            #endregion

            #region Instancing
            static RichToolTip()
            {
                EventManager.RegisterClassHandler(typeof(UIElement), UIElement.MouseDownEvent, new MouseButtonEventHandler(OnElementMouseDown), true);
                EventManager.RegisterClassHandler(typeof(RichToolTip), ButtonBase.ClickEvent, new RoutedEventHandler(OnButtonBaseClick), false);
                
                //EventManager.RegisterClassHandler(typeof(UIElement), UIElement.MouseEnterEvent, new MouseEventHandler(element_MouseEnter), true);
                //EventManager.RegisterClassHandler(typeof(UIElement), UIElement.MouseLeaveEvent, new MouseEventHandler(element_MouseLeave), true);

                EventManager.RegisterClassHandler(typeof(Selector), Selector.SelectionChangedEvent, new SelectionChangedEventHandler(selector_SelectionChangedEvent), true);
                

                //only in XBap mode
                if (BrowserInteropHelper.IsBrowserHosted)
                {
                    EventManager.RegisterClassHandler(typeof(NavigationWindow), UIElement.MouseLeaveEvent, new RoutedEventHandler(OnNavigationWindowMouseLeaveEvent), true);
                }
                else
                {
                    EventManager.RegisterClassHandler(typeof(Window), Window.SizeChangedEvent, new RoutedEventHandler(OnWindowSizeChanged), true);
                }

                CommandManager.RegisterClassCommandBinding(typeof(RichToolTip), new CommandBinding(CloseCommand, ExecuteCloseCommand));

                InitStoryboards();
            }

            public RichToolTip()
            {
                Loaded += new RoutedEventHandler(ContentTooltip_Loaded);
                Unloaded += new RoutedEventHandler(ContentTooltip_Unloaded);
            }

            public RichToolTip(UIElement relatedObject)
                : this()
            {
                Load(relatedObject);
            }
            #endregion

            #region Loading
            void Load(UIElement relatedObject)
            {
                FrameworkElement fe = relatedObject as FrameworkElement;
                if (fe == null)
                {
                    throw new InvalidOperationException("The element is not supported");
                }

                relatedObject.MouseEnter += element_MouseEnter;
                relatedObject.MouseLeave += element_MouseLeave;

                this.MouseEnter += richToolTip_MouseEnter;
                this.MouseLeave += richToolTip_MouseLeave;

                fe.Unloaded += new RoutedEventHandler(RelatedObject_Unloaded);
            }

            void PrepareToShow(UIElement relatedObject)
            {
                RelatedObject = relatedObject;
                BindRootVisual();
            }

            void RelatedObject_Unloaded(object sender, RoutedEventArgs e)
            {
                if (RelatedObject != null)
                {
                    RelatedObject.MouseEnter -= element_MouseEnter;
                    RelatedObject.MouseLeave -= element_MouseLeave;
                }
            }

            void ContentTooltip_Unloaded(object sender, RoutedEventArgs e)
            {
                UnbindRootVisual();
            }

            void ContentTooltip_Loaded(object sender, RoutedEventArgs e)
            {
                BindRootVisual();
            }
            #endregion

            #region Popup Creation
            private static readonly Type PopupType = typeof(Popup);
            private static readonly Type PopupSecurityHelperType = PopupType.GetNestedType("PopupSecurityHelper", BindingFlags.Public | BindingFlags.NonPublic);

            private static readonly FieldInfo Popup_secHelper = PopupType.GetField("_secHelper", BindingFlags.Instance | BindingFlags.NonPublic);
            private static readonly FieldInfo PopupSecurityHelper_isChildPopupInitialized = PopupSecurityHelperType.GetField("_isChildPopupInitialized", BindingFlags.Instance | BindingFlags.NonPublic);
            private static readonly FieldInfo PopupSecurityHelper_isChildPopup = PopupSecurityHelperType.GetField("_isChildPopup", BindingFlags.Instance | BindingFlags.NonPublic);

            void HookupParentPopup()
            {
                //Create the Popup and attach the CustomControl to it.
                _parentPopup = new Popup();

                //THIS IS A HACK!
                //This enables transparency on the popup - needed for XBap versions!
                //NOTE - this requires that the xbap app will run in full trust
                if (BrowserInteropHelper.IsBrowserHosted)
                {
                    try
                    {
                        new ReflectionPermission(PermissionState.Unrestricted).Demand();

                        DoPopupHacks();
                    }
                    catch (SecurityException) { }
                }

                _parentPopup.AllowsTransparency = true;

                Popup.CreateRootPopup(_parentPopup, this);
            }

            void DoPopupHacks()
            {
                object secHelper = Popup_secHelper.GetValue(_parentPopup);
                PopupSecurityHelper_isChildPopupInitialized.SetValue(secHelper, true);
                PopupSecurityHelper_isChildPopup.SetValue(secHelper, false);
            }
            #endregion

            #region Commands
            public static RoutedCommand CloseCommand = new RoutedCommand("Close", typeof(RichToolTip));
            static void ExecuteCloseCommand(object sender, ExecutedRoutedEventArgs e)
            {
                HideLastShown(true);
            }
            #endregion

            #region Dependency Properties

            public static readonly DependencyProperty PlacementProperty =
                        ToolTipService.PlacementProperty.AddOwner(typeof(RichToolTip));

            public PlacementMode Placement
            {
                get { return (PlacementMode)GetValue(PlacementProperty); }
                set { SetValue(PlacementProperty, value); }
            }

            public static PlacementMode GetPlacement(DependencyObject obj)
            {
                return (PlacementMode)obj.GetValue(PlacementProperty);
            }

            public static void SetPlacement(DependencyObject obj, PlacementMode value)
            {
                obj.SetValue(PlacementProperty, value);
            }

            public static readonly DependencyProperty PlacementTargetProperty =
               ToolTipService.PlacementTargetProperty.AddOwner(typeof(RichToolTip));

            public UIElement PlacementTarget
            {
                get { return (UIElement)GetValue(PlacementTargetProperty); }
                set { SetValue(PlacementTargetProperty, value); }
            }

            public static UIElement GetPlacementTarget(DependencyObject obj)
            {
                return (UIElement)obj.GetValue(PlacementTargetProperty);
            }

            public static void SetPlacementTarget(DependencyObject obj, UIElement value)
            {
                obj.SetValue(PlacementTargetProperty, value);
            }

            public static readonly DependencyProperty PlacementRectangleProperty =
                        ToolTipService.PlacementRectangleProperty.AddOwner(typeof(RichToolTip));

            public Rect PlacementRectangle
            {
                get { return (Rect)GetValue(PlacementRectangleProperty); }
                set { SetValue(PlacementRectangleProperty, value); }
            }

            public static Rect GetPlacementRectangle(DependencyObject obj)
            {
                return (Rect)obj.GetValue(PlacementRectangleProperty);
            }

            public static void SetPlacementRectangle(DependencyObject obj, Rect value)
            {
                obj.SetValue(PlacementRectangleProperty, value);
            }

            public static readonly DependencyProperty HorizontalOffsetProperty =
                ToolTipService.HorizontalOffsetProperty.AddOwner(typeof(RichToolTip));

            public double HorizontalOffset
            {
                get { return (double)GetValue(HorizontalOffsetProperty); }
                set { SetValue(HorizontalOffsetProperty, value); }
            }

            public static double GetHorizontalOffset(DependencyObject obj)
            {
                return (double)obj.GetValue(HorizontalOffsetProperty);
            }

            public static void SetHorizontalOffset(DependencyObject obj, double value)
            {
                obj.SetValue(HorizontalOffsetProperty, value);
            }

            public static readonly DependencyProperty VerticalOffsetProperty =
                    ToolTipService.VerticalOffsetProperty.AddOwner(typeof(RichToolTip));

            public double VerticalOffset
            {
                get { return (double)GetValue(VerticalOffsetProperty); }
                set { SetValue(VerticalOffsetProperty, value); }
            }

            public static double GetVerticalOffset(DependencyObject obj)
            {
                return (double)obj.GetValue(VerticalOffsetProperty);
            }

            public static void SetVerticalOffset(DependencyObject obj, double value)
            {
                obj.SetValue(VerticalOffsetProperty, value);
            }

            public static readonly DependencyProperty IsOpenProperty =
                    Popup.IsOpenProperty.AddOwner(
                            typeof(RichToolTip),
                            new FrameworkPropertyMetadata(
                                    false,
                                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                    new PropertyChangedCallback(OnIsOpenChanged)));

            public bool IsOpen
            {
                get { return (bool)GetValue(IsOpenProperty); }
                set { SetValue(IsOpenProperty, value); }
            }

            private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                RichToolTip ctrl = (RichToolTip)d;

                if ((bool)e.NewValue)
                {
                    if (ctrl._parentPopup == null)
                    {
                        ctrl.HookupParentPopup();
                    }
                }
            }

            public static readonly DependencyProperty StaysOpenProperty =
                System.Windows.Controls.ToolTip.StaysOpenProperty.AddOwner(
                typeof(RichToolTip),
                new FrameworkPropertyMetadata(
                        true,
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

            public bool StaysOpen
            {
                get { return (bool)GetValue(StaysOpenProperty); }
                set { SetValue(StaysOpenProperty, value); }
            }

            public static readonly DependencyProperty CustomPopupPlacementCallbackProperty =
                System.Windows.Controls.ToolTip.CustomPopupPlacementCallbackProperty.AddOwner(
                typeof(RichToolTip),
                new FrameworkPropertyMetadata(
                default(CustomPopupPlacementCallback),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

            public CustomPopupPlacementCallback CustomPopupPlacementCallback
            {
                get { return (CustomPopupPlacementCallback)GetValue(CustomPopupPlacementCallbackProperty); }
                set { SetValue(CustomPopupPlacementCallbackProperty, value); }
            }

            #endregion

            #region Attached Properties

            #region HideOnClick
            public static bool GetHideOnClick(DependencyObject obj)
            {
                return (bool)obj.GetValue(HideOnClickProperty);
            }

            public static void SetHideOnClick(DependencyObject obj, bool value)
            {
                obj.SetValue(HideOnClickProperty, value);
            }

            public static readonly DependencyProperty HideOnClickProperty =
                DependencyProperty.RegisterAttached("HideOnClick", typeof(bool), typeof(RichToolTip), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));


            #endregion

            #region HideOnSelection

            public static bool GetHideOnSelection(DependencyObject obj)
            {
                return (bool)obj.GetValue(HideOnSelectionProperty);
            }

            public static void SeHideOnSelection(DependencyObject obj, bool value)
            {
                obj.SetValue(HideOnSelectionProperty, value);
            }

            public static readonly DependencyProperty HideOnSelectionProperty =
                DependencyProperty.RegisterAttached("HideOnSelection", typeof(bool), typeof(RichToolTip), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

            #endregion

            #region PopupContent
            public static object GetPopupContent(DependencyObject obj)
            {
                return obj.GetValue(PopupContentProperty);
            }

            public static void SetPopupContent(DependencyObject obj, object value)
            {
                obj.SetValue(PopupContentProperty, value);
            }

            public static readonly DependencyProperty PopupContentProperty =
                DependencyProperty.RegisterAttached("PopupContent", typeof(object), typeof(RichToolTip), new FrameworkPropertyMetadata(OnPopupContentChanged));

            private static void OnPopupContentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
            {
                UIElement element = o as UIElement;
                if (element == null)
                {
                    throw new InvalidOperationException("Can't hook to events other than UI Element");
                }

                if (e.NewValue != null)
                {
                    RichToolTip popup = e.NewValue as RichToolTip;

                    if (popup != null)
                    {
                        popup.Load(element);
                    }
                    else
                    {
                        popup = new RichToolTip(element);

                        Binding binding = new Binding
                        {
                            Path = new PropertyPath(PopupContentProperty),
                            Mode = BindingMode.OneWay,
                            Source = o,
                        };
                        popup.SetBinding(ContentProperty, binding);

                    }

                    SetContentTooltipWrapper(o, popup);
                }
            }
            #endregion

            #region ContentTooltipWrapper
            internal static RichToolTip GetContentTooltipWrapper(DependencyObject obj)
            {
                return (RichToolTip)obj.GetValue(ContentTooltipWrapperProperty);
            }

            internal static void SetContentTooltipWrapper(DependencyObject obj, RichToolTip value)
            {
                obj.SetValue(ContentTooltipWrapperProperty, value);
            }

            internal static readonly DependencyProperty ContentTooltipWrapperProperty =
                DependencyProperty.RegisterAttached("ContentTooltipWrapper", typeof(RichToolTip), typeof(RichToolTip));
            #endregion

            #endregion

            #region Root Visual Binding
            bool boundToRoot = false;
            bool hasParentWindow = false;
            Window parentWindow = null;

            void BindRootVisual()
            {
                if (!boundToRoot)
                {
                    if (!BrowserInteropHelper.IsBrowserHosted)
                    {
                        parentWindow = UIHelpers.FindLogicalAncestorByType<Window>(RelatedObject)
                            ?? UIHelpers.FindVisualAncestorByType<Window>(RelatedObject);

                        if (parentWindow != null)
                        {
                            hasParentWindow = true;

                            parentWindow.Deactivated += window_Deactivated;
                            parentWindow.LocationChanged += window_LocationChanged;
                        }
                    }

                    boundToRoot = true;
                }
            }

            void UnbindRootVisual()
            {
                if (boundToRoot)
                {
                    if (parentWindow != null)
                    {
                        parentWindow.Deactivated -= window_Deactivated;
                        parentWindow.LocationChanged -= window_LocationChanged;
                    }

                    boundToRoot = false;
                }
            }
            #endregion

            #region Animations & Intervals
            
            static DispatcherTimer _timer;

            /// <summary>
            /// Timer that starts when mouse leaves the object and allows user to reach a tooltip. 
            /// </summary>
            static DispatcherTimer m_timerToReachTooltip;

            static Storyboard showStoryboard;
            static Storyboard hideStoryboard;
            bool setRenderTransform;

            static void InitStoryboards()
            {
                showStoryboard = new Storyboard();
                hideStoryboard = new Storyboard();

                TimeSpan duration = TimeSpan.FromMilliseconds(AnimationDurationInMs);

                DoubleAnimation animation = new DoubleAnimation(1, duration, FillBehavior.HoldEnd);
                Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));
                showStoryboard.Children.Add(animation);

                //animation = new DoubleAnimation(0.1, 1, duration, FillBehavior.Stop);
                //Storyboard.SetTargetProperty(animation, new PropertyPath("(0).(1)", FrameworkElement.RenderTransformProperty, ScaleTransform.ScaleXProperty));
                //showStoryboard.Children.Add(animation);

                //animation = new DoubleAnimation(0.1, 1, duration, FillBehavior.Stop);
                //Storyboard.SetTargetProperty(animation, new PropertyPath("(0).(1)", FrameworkElement.RenderTransformProperty, ScaleTransform.ScaleYProperty));
                //showStoryboard.Children.Add(animation);

                animation = new DoubleAnimation(0, duration, FillBehavior.Stop);
                Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));
                hideStoryboard.Children.Add(animation);

                hideStoryboard.Completed += delegate { OnAnimationCompleted(); };
            }

            static void StartTimerToReachTooltip()
            {
                if (m_timerToReachTooltip == null)
                {
                    m_timerToReachTooltip = new DispatcherTimer();
                    m_timerToReachTooltip.Interval = TimeSpan.FromMilliseconds(ShowDeferredMilliseconds);
                    m_timerToReachTooltip.Tick += new EventHandler(ResetTimerToReachTooltip_Tick);
                }
                
                m_timerToReachTooltip.Start();
            }

            static void StopTimerToReachTooltip()
            {
                if (m_timerToReachTooltip != null)
                {
                    m_timerToReachTooltip.Stop();
                }
            }

            private static void ResetTimerToReachTooltip_Tick(object sender, System.EventArgs e)
            {
                HideLastShown();
                StopTimerToReachTooltip();
            }


            static void InitTimer()
            {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromMilliseconds(ShowDeferredMilliseconds);
            }

            static void ResetTimer(RichToolTip tooltip)
            {
                if (_timer != null)
                {
                    _timer.Tick -= tooltip.ShowDeferred;
                    _timer.Stop();
                }
            }

            void Animate(bool show)
            {
                if (show)
                {
                    if (!setRenderTransform)
                    {
                        RenderTransform = new ScaleTransform();
                        
                        setRenderTransform = true;
                    }

                    showStoryboard.Begin(this);
                }
                else
                {
                    hideStoryboard.Begin(this);
                }
            }

            static void OnAnimationCompleted()
            {
                HideLastShown(false);
            }
            #endregion

            #region Event Invocations
            void element_MouseEnter(object sender, MouseEventArgs e)
            {
                DependencyObject o = sender as DependencyObject;

                //implementation for static subscribing:
                //if (o != null)
                //{
                //    RichToolTip tooltip = GetContentTooltipWrapper(o);

                //    if (tooltip != null && (tooltip != lastShownPopup || tooltip._relatedObject != o))
                //    {
                //        tooltip.Show();
                //    }
                //}


                if (lastShownPopup != null && this != lastShownPopup)
                {
                    lastShownPopup.Hide(true);
                }

                if (!IsShown() && this != lastShownPopup)
                {
                    if (!hasParentWindow || parentWindow.IsActive)
                    {
                        PrepareToShow(sender as UIElement);
                        Show(true);
                    }
                }
            }


            void element_MouseLeave(object sender, MouseEventArgs e)
            {
                //implementation for static subscribing:
                //DependencyObject o = sender as DependencyObject;

                //if (o != null)
                //{
                //    RichToolTip tooltip = GetContentTooltipWrapper(o);

                //    if (tooltip != null)
                //    {
                //        ResetTimer(tooltip);
                //    }
                //}

                StartTimerToReachTooltip();
                ResetTimer(this);
            }
            
            void richToolTip_MouseEnter(object sender, MouseEventArgs e)
            {
                StopTimerToReachTooltip();
              
            }

            void richToolTip_MouseLeave(object sender, MouseEventArgs e)
            {
                StartTimerToReachTooltip();
            }

            static void OnNavigationWindowMouseLeaveEvent(object sender, RoutedEventArgs e)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() =>
                {
                    if (lastShownPopup != null && !lastShownPopup.IsMouseOver)
                    {
                        HideLastShown(false);
                    }
                }));
            }

            void window_LocationChanged(object sender, System.EventArgs e)
            {
                if (IsShown())
                {
                    HideLastShown(false);
                }
            }

            void window_Deactivated(object sender, System.EventArgs e)
            {
                if (IsShown())
                {
                    HideLastShown(false);
                }
            }

            static void OnWindowSizeChanged(object sender, RoutedEventArgs e)
            {
                HideLastShown();
            }

            static void OnElementMouseDown(object sender, MouseButtonEventArgs e)
            {
                if (lastShownPopup != null && lastShownPopup.IsShown())
                {
                    RichToolTip popup;

                    DependencyObject o = e.OriginalSource as DependencyObject;
                    if (!TryFindPopupParent(e.OriginalSource, out popup))
                    {
                        HideLastShown(true);
                    }
                }
            }

            static void OnButtonBaseClick(object sender, RoutedEventArgs e)
            {
                if (lastShownPopup != null && lastShownPopup.IsShown())
                {
                    DependencyObject o = e.OriginalSource as DependencyObject;

                    bool hide = GetHideOnClick(o);
                    if (hide)
                    {
                        HideLastShown(true);

                        e.Handled = true;
                    }
                }
            }

            static void selector_SelectionChangedEvent(object sender, SelectionChangedEventArgs e)
            {
                if (lastShownPopup != null && lastShownPopup.IsShown())
                {
                    DependencyObject o = e.OriginalSource as DependencyObject;

                    bool hide = GetHideOnSelection(o);
                    if (hide)
                    {
                        HideLastShown(true);

                        e.Handled = true;
                    }
                }
            }

            static bool TryFindPopupParent(object source, out RichToolTip popup)
            {
                popup = null;
                UIElement element = source as UIElement;

                if (element != null)
                {
                    popup = UIHelpers.FindVisualAncestorByType<RichToolTip>(element);

                    if (popup == null)
                    {
                        popup = UIHelpers.FindLogicalAncestorByType<RichToolTip>(element);
                    }

                    return popup != null;
                }

                return false;
            }
            #endregion

            #region Show / Hide
            bool showAnimate = AnimationEnabledDefault;

            bool IsShown()
            {
                return IsOpen;
            }

            public void Show(bool animate)
            {
                showAnimate = animate;

                if (_timer == null)
                {
                    InitTimer();
                }

                _timer.Tick += ShowDeferred;

                if (!_timer.IsEnabled)
                {
                    _timer.Start();
                }
            }

            private void ShowDeferred(object sender, System.EventArgs e)
            {
                ResetTimer(this);

                HideLastShown(false);

                ShowInternal();

                if (showAnimate && EnableAnimation)
                {
                    Animate(true);
                }
                else
                {
                    this.Opacity = 1;
                }

                lastShownPopup = this;
            }

            private void ShowInternal()
            {
                Visibility = Visibility.Visible;

                IsOpen = true;
            }

            static void HideLastShown()
            {
                HideLastShown(false);
            }

            static void HideLastShown(bool animate)
            {
                if (lastShownPopup != null)
                {
                    lastShownPopup.Hide(animate);
                }
            }

            public void Hide(bool animate)
            {
                if (animate && EnableAnimation)
                {
                    Animate(false);
                }
                else
                {
                    HideInternal();
                }
            }

            private void HideInternal()
            {
                Visibility = Visibility.Collapsed;

                IsOpen = false;

                lastShownPopup = null;
            }

            #endregion
        }


}
