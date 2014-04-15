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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using GraphSharp.Controls;
using TraceLab.UI.WPF.Utilities;

namespace TraceLab.UI.WPF.Views.Nodes
{
    /// <summary>
    /// Interaction logic for NodeInfoContainer.xaml
    /// </summary>
    public partial class NodeInfoContainer : UserControl
    {
        public NodeInfoContainer()
        {
            InitializeComponent();
            Focusable = true;
        }

        static NodeInfoContainer()
        {
            const FrameworkPropertyMetadataOptions frameworkOptions = FrameworkPropertyMetadataOptions.AffectsRender |
                                                          FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                          FrameworkPropertyMetadataOptions.AffectsArrange;
            OriginElementProperty = DependencyProperty.Register("OriginElement", typeof(VertexControl),
                                                    typeof(NodeInfoContainer),
                                                    new FrameworkPropertyMetadata(default(VertexControl),
                                                    frameworkOptions, OriginElementChanged));

            IsVisualValidProperty = DependencyProperty.Register("IsVisualValid", typeof(bool),
                                                            typeof(NodeInfoContainer),
                                                            new FrameworkPropertyMetadata(false, frameworkOptions, InvalidateVisualChanged, CoerceInvalidateVisual));
        }

        private static void InvalidateVisualChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as NodeInfoContainer;
            if (control != null)
            {
                control.InvalidateVisual();
            }
        }

        private static object CoerceInvalidateVisual(DependencyObject d, object baseValue)
        {
            var control = d as NodeInfoContainer;
            if (control != null)
            {
                control.InvalidateVisual();
            }

            return false;
        }

        private static void OriginElementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as NodeInfoContainer;
            if (control != null)
            {
                control.RemoveBindings(control);
                control.AttachBindings(args.NewValue, control);
            }
        }

        private void RemoveBindings(object rawObject)
        {
            var obj = rawObject as DependencyObject;
            if (obj != null)
            {
                BindingOperations.ClearBinding(this, IsVisualValidProperty);
            }
        }

        private Binding CreateBinding(DependencyObject source, DependencyProperty property)
        {
            Binding b = new Binding();
            var propertyPath = new PropertyPath(property);
            b.Source = source;
            b.Path = propertyPath;

            return b;
        }

        private void AttachBindings(object firstObject, object secondObject)
        {
            var first = firstObject as DependencyObject;
            var second = secondObject as DependencyObject;
            if (first != null && second != null)
            {
                MultiBinding bind = new MultiBinding();
                bind.Converter = new AnyChangeIsTrueConverter();

                bind.Bindings.Add(CreateBinding(first, Canvas.LeftProperty));
                bind.Bindings.Add(CreateBinding(first, Canvas.TopProperty));

                bind.Bindings.Add(CreateBinding(second, Canvas.LeftProperty));
                bind.Bindings.Add(CreateBinding(second, Canvas.TopProperty));

                bind.Bindings.Add(CreateBinding(first, UIElement.IsMouseOverProperty));
                bind.Bindings.Add(CreateBinding(second, UIElement.IsMouseOverProperty));

                this.SetBinding(IsVisualValidProperty, bind);
            }
        }

        public static readonly DependencyProperty OriginElementProperty;
        public VertexControl OriginElement
        {
            get { return (VertexControl)GetValue(OriginElementProperty); }
            set { SetValue(OriginElementProperty, value); }
        }

        public static readonly DependencyProperty IsVisualValidProperty;
        public bool IsVisualValid
        {
            get { return (bool)GetValue(IsVisualValidProperty); }
            set { SetValue(IsVisualValidProperty, value); }
        }
        
        /// <summary>
        /// When overridden in a derived class, participates in rendering operations that are directed by the layout system. 
        /// In this case the rendering draws additional line between info box and its origin vertex control, when
        /// the mouse is over either vertex control or its info box.
        /// </summary>
        /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            var element = OriginElement as FrameworkElement;

            if (element != null && (IsMouseOver || element.IsMouseOver))
            {
                var mainWindow = this.GetParent<DockableGraph>(null);
                
                var gt = element.TransformToVisual(this);
                var elementCenter = gt.Transform(new Point(element.ActualWidth / 2, element.ActualHeight / 2));
                var elementSize = new Size(element.ActualWidth, element.ActualHeight);
                var elementRect = new Rect(gt.Transform(new Point(0, 0)), elementSize);

                var thisCenter = new Point(ActualWidth / 2, ActualHeight / 2);

                // The total area we'll be drawing in.
                var totalRect = new Rect(elementRect.Location, elementRect.Size);
                totalRect.Union(new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight)));

                // Create geometry objects to work with.
                var elementRectangleGeometry = new RectangleGeometry(elementRect);
                var totalRectangleGeometry = new RectangleGeometry(totalRect);

                // We'll now remove the element's rectangle from where we're drawing, so that we 
                // don't draw on top of it, but will draw on top of everything else.
                CombinedGeometry clipGeometry = new CombinedGeometry(GeometryCombineMode.Exclude, totalRectangleGeometry, elementRectangleGeometry);

                drawingContext.PushClip(clipGeometry);

                //var node = OriginElement.Vertex as TraceLab.Core.Experiments.ExperimentNode;
                //Point vertexCenter = new Point(node.Data.X, node.Data.Y);

                Pen solidPen = new Pen(System.Windows.Media.Brushes.LightCoral, 1.0);
                drawingContext.DrawLine(solidPen, elementCenter, thisCenter);

                // Pop off the RectangleGeometry
                drawingContext.Pop();
            }

            base.OnRender(drawingContext);
        }

        private void NodeInfoContainer_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            NodeInfoContainer container = (NodeInfoContainer)sender;
            if ((bool)e.NewValue == true)
            {
                container.BeginStoryboard((System.Windows.Media.Animation.Storyboard)container.FindResource("SelectedStoryboard"));
            }
            else
            {
                container.BeginStoryboard((System.Windows.Media.Animation.Storyboard)container.FindResource("UnselectedStoryboard"));
            }
        }

        private void NodeInfoContainer_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dependencyObject = (DependencyObject)e.OriginalSource;
            var hitControl = dependencyObject.GetParent<Control>(this);
            if(hitControl == null || hitControl == this)
            {
                NodeInfoContainer control = (NodeInfoContainer)sender;
                control.Focus();
            }
        }
    }

    internal class AnyChangeIsTrueConverter : IMultiValueConverter
    {
        private object[] m_oldValues;

        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from sourceEnum bindings to the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding"/> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the sourceEnum binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value.  If the method returns null, the valid null value is used.  A return value of <see cref="T:System.Windows.DependencyProperty"/>.<see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding"/>.<see cref="F:System.Windows.Data.Binding.DoNothing"/> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> or the default value.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool found = false;
            if (values != null && m_oldValues != null && values.Length == m_oldValues.Length)
            {
                for (int i = 0; i < values.Length && found == false; ++i)
                {
                    if (object.Equals(values[i], m_oldValues[i]) == false)
                    {
                        found = true;
                    }
                }
            }
            else
            {
                found = true;
            }

            m_oldValues = values;
            return found;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
