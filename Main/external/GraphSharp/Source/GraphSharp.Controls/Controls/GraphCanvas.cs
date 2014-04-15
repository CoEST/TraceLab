﻿using System;
using System.Windows;
using System.Windows.Controls;
using GraphSharp.Controls.Animations;
using System.Windows.Media;

namespace GraphSharp.Controls
{
    public class GraphCanvas : Canvas
    {
        #region Attached Dependency Property registrations
        public static readonly DependencyProperty XProperty =
            DependencyProperty.RegisterAttached("X", typeof(double), typeof(GraphCanvas),
                                                 new FrameworkPropertyMetadata(double.NaN,
                                                                                FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                                FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                                FrameworkPropertyMetadataOptions.AffectsRender |
                                                                                FrameworkPropertyMetadataOptions.AffectsParentMeasure |
                                                                                FrameworkPropertyMetadataOptions.AffectsParentArrange |
                                                                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                                X_PropertyChanged));

        private static void X_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var xChange = (double)e.NewValue - (double)e.OldValue;
            PositionChanged(d, xChange, 0);
        }


        public static readonly DependencyProperty YProperty =
            DependencyProperty.RegisterAttached("Y", typeof(double), typeof(GraphCanvas),
                                                 new FrameworkPropertyMetadata(double.NaN,
                                                                                FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                                FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                                FrameworkPropertyMetadataOptions.AffectsRender |
                                                                                FrameworkPropertyMetadataOptions.AffectsParentMeasure |
                                                                                FrameworkPropertyMetadataOptions.AffectsParentArrange |
                                                                                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                                                                Y_PropertyChanged));

        private static void Y_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var yChange = (double)e.NewValue - (double)e.OldValue;
            PositionChanged(d, 0, yChange);
        }

        private static void PositionChanged(DependencyObject d, double xChange, double yChange)
        {
            UIElement e = d as UIElement;
            if (e != null)
                e.RaiseEvent(new PositionChangedEventArgs(PositionChangedEvent, e, xChange, yChange));
        }

        #endregion

        #region Attached Properties
        [AttachedPropertyBrowsableForChildren]
        public static double GetX(DependencyObject obj)
        {
            return (double)obj.GetValue(XProperty);
        }

        public static void SetX(DependencyObject obj, double value)
        {
            obj.SetValue(XProperty, value);
        }

        [AttachedPropertyBrowsableForChildren]
        public static double GetY(DependencyObject obj)
        {
            return (double)obj.GetValue(YProperty);
        }

        public static void SetY(DependencyObject obj, double value)
        {
            obj.SetValue(YProperty, value);
        }

        #endregion

        #region Attached Routed Events
        public static readonly RoutedEvent PositionChangedEvent =
            EventManager.RegisterRoutedEvent("PositionChanged", RoutingStrategy.Bubble, typeof(PositionChangedEventHandler), typeof(GraphCanvas));
        public static void AddPositionChangedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement e = d as UIElement;
            if (e != null)
                e.AddHandler(PositionChangedEvent, handler);
        }

        public static void RemovePositionChangedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement e = d as UIElement;
            if (e != null)
                e.RemoveHandler(PositionChangedEvent, handler);
        }
        #endregion

        #region DP - Animation length

        /// <summary>
        /// Gets or sets the length of the animation.
        /// If the length of the animations is 0:0:0.000, 
        /// there won't be any animations.
        /// 
        /// Default value is 500 ms.
        /// </summary>
        public TimeSpan AnimationLength
        {
            get { return (TimeSpan)GetValue(AnimationLengthProperty); }
            set { SetValue(AnimationLengthProperty, value); }
        }

        public static readonly DependencyProperty AnimationLengthProperty =
            DependencyProperty.Register("AnimationLength", typeof(TimeSpan), typeof(GraphCanvas), new UIPropertyMetadata(new TimeSpan(0, 0, 0, 0, 500)));
        #endregion

        #region DP - CreationAnimation

        /// <summary>
        /// Gets or sets the animation controller for the 'Control Creation' animation.
        /// </summary>
        public ITransition CreationTransition
        {
            get { return (ITransition)GetValue(CreationTransitionProperty); }
            set { SetValue(CreationTransitionProperty, value); }
        }

        public static readonly DependencyProperty CreationTransitionProperty =
            DependencyProperty.Register("CreationTransition", typeof(ITransition), typeof(GraphCanvas), new UIPropertyMetadata(new FadeInTransition()));

        #endregion

        #region IsAnimationEnabled

        /// <summary>
        /// If this property is true, and the other animation disabler 
        /// properties are also true, the animation is enabled.
        /// If this is false, the animations will be disabled.
        /// </summary>
        public bool IsAnimationEnabled
        {
            get { return (bool)GetValue(IsAnimationEnabledProperty); }
            set { SetValue(IsAnimationEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsAnimationEnabledProperty =
            DependencyProperty.Register("IsAnimationEnabled", typeof(bool), typeof(GraphCanvas), new UIPropertyMetadata(true));

        #endregion

        #region DP - MoveAnimation

        /// <summary>
        /// Gets or sets the animation controller for the 'Control Moving' animation.
        /// </summary>
        public IAnimation MoveAnimation
        {
            get { return (IAnimation)GetValue(MoveAnimationProperty); }
            set { SetValue(MoveAnimationProperty, value); }
        }

        public static readonly DependencyProperty MoveAnimationProperty =
            DependencyProperty.Register("MoveAnimation", typeof(IAnimation), typeof(GraphCanvas), new UIPropertyMetadata(new SimpleMoveAnimation()));

        #endregion

        #region DP - DestructionAnimation

        /// <summary>
        /// Gets or sets the transition controller for the 'Control Descruction' animation.
        /// </summary>
        public ITransition DestructionTransition
        {
            get { return (ITransition)GetValue(DestructionTransitionProperty); }
            set { SetValue(DestructionTransitionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DestructionAnimation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DestructionTransitionProperty =
            DependencyProperty.Register("DestructionTransition", typeof(ITransition), typeof(GraphCanvas), new UIPropertyMetadata(new FadeOutTransition()));


        #endregion

        #region DP - Origo

        /// <summary>
        /// Gets or sets the virtual origo of the canvas.
        /// </summary>
        public Point Origo
        {
            get { return (Point)GetValue(OrigoProperty); }
            set { SetValue(OrigoProperty, value); }
        }

        public static readonly DependencyProperty OrigoProperty =
            DependencyProperty.Register("Origo", typeof(Point), typeof(GraphCanvas),
                new FrameworkPropertyMetadata(
                    new Point(),
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure |
                    FrameworkPropertyMetadataOptions.AffectsParentArrange));

        #endregion



        public Vector Translation
        {
            get { return (Vector)GetValue(TranslationProperty); }
            protected set { SetValue(TranslationPropertyKey, value); }
        }

        public static readonly DependencyProperty TranslationProperty;
        protected static readonly DependencyPropertyKey TranslationPropertyKey =
            DependencyProperty.RegisterReadOnly("Translation", typeof(Vector), typeof(GraphCanvas), new UIPropertyMetadata(new Vector()));

        static GraphCanvas()
        {
            TranslationProperty = TranslationPropertyKey.DependencyProperty;
        }

        public GraphCanvas()
        {
        }

        /// <summary>
        /// The layout process will be initialized with the current 
        /// vertex positions.
        /// </summary>
        public virtual void ContinueLayout()
        {
        }

        /// <summary>
        /// The layout process will be started without initial
        /// vertex positions.
        /// </summary>
        public virtual void Relayout()
        {
        }


        private IAnimationContext animationContext;

        /// <summary>
        /// Gets the context of the animation.
        /// </summary>
        public virtual IAnimationContext AnimationContext
        {
            get
            {
                if (animationContext == null)
                    animationContext = new AnimationContext(this);

                return animationContext;
            }
        }

        /// <summary>
        /// Gets whether the animation could be run, or not.
        /// </summary>
        public virtual bool CanAnimate
        {
            get
            {
                return IsAnimationEnabled;
            }
        }

        /// <summary>
        /// Does a transition for the control which has been already added
        /// to this container.
        /// </summary>
        /// <param name="control">The control which has been added.</param>
        protected virtual void RunCreationTransition(Control control)
        {
            if (CreationTransition == null)
                return;

            CreationTransition.Run(AnimationContext, control, AnimationLength);
        }

        /// <summary>
        /// Animates the position of the given control to
        /// the given positions.
        /// </summary>
        /// <param name="control">The control which should be moved.</param>
        /// <param name="x">The new horizontal position of the control.</param>
        /// <param name="y">The new vertical position of the control.</param>
        protected virtual void RunMoveAnimation(Control control, double x, double y)
        {
            if (MoveAnimation != null && CanAnimate)
                //animate to the given position
                MoveAnimation.Animate(AnimationContext, control, x, y, AnimationLength);
            else
            {
                //cannot animate, only the given positions
                SetX(control, x);
                SetY(control, y);
            }
        }

        /// <summary>
        /// Transites a control which gonna' be removed from this 
        /// container.
        /// </summary>
        /// <param name="control">The control which will be removed.</param>
        /// <param name="dontRemoveAfter">If it's true, the control won't be removed
        /// automatically from this container's Children.</param>
        protected virtual void RunDestructionTransition(Control control, bool dontRemoveAfter)
        {
            if (DestructionTransition == null)
                return;

            if (dontRemoveAfter)
                DestructionTransition.Run(AnimationContext, control, AnimationLength);
            else
                DestructionTransition.Run(AnimationContext, control, AnimationLength, Children.Remove);
        }
    }
}
