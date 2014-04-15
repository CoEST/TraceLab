using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GraphSharp.Helpers;
using System;

namespace GraphSharp.Controls
{
	/// <summary>
	/// Logical representation of a vertex.
	/// </summary>
    [TemplatePart(Name="PART_VertexArea")]
    public class VertexControl : Control, IPoolObject, IDisposable
	{
		public object Vertex
		{
			get { return GetValue( VertexProperty ); }
			set { SetValue( VertexProperty, value ); }
		}

		public static readonly DependencyProperty VertexProperty =
			DependencyProperty.Register( "Vertex", typeof( object ), typeof( VertexControl ), new UIPropertyMetadata( null ) );


        public GraphCanvas RootCanvas
        {
            get { return (GraphCanvas)GetValue(RootCanvasProperty); }
            set { SetValue(RootCanvasProperty, value); }
        }

        public static readonly DependencyProperty RootCanvasProperty =
            DependencyProperty.Register("RootCanvas", typeof(GraphCanvas), typeof(VertexControl), new UIPropertyMetadata(null));

	    public FrameworkElement VertexArea
	    {
            get { return (FrameworkElement) GetValue(VertexAreaProperty); }
            set { SetValue(VertexAreaProperty, value); }
	    }

	    public static readonly DependencyProperty VertexAreaProperty = DependencyProperty.Register("VertexArea",
	                                                                                       typeof (FrameworkElement),
	                                                                                       typeof (VertexControl),
	                                                                                       new UIPropertyMetadata(null));

        #region Positioning

        public static readonly DependencyProperty CenterXProperty;
        public static readonly DependencyProperty CenterYProperty;
        public static readonly DependencyProperty TopLeftXProperty;
        public static readonly DependencyProperty TopLeftYProperty;

        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        public double TopLeftX
        {
            get { return (double)GetValue(TopLeftXProperty); }
            set { SetValue(TopLeftXProperty, value); }
        }

        public double TopLeftY
        {
            get { return (double)GetValue(TopLeftYProperty); }
            set { SetValue(TopLeftYProperty, value); }
        }

        #endregion 

        static VertexControl()
		{
			//override the StyleKey Property
			DefaultStyleKeyProperty.OverrideMetadata( typeof( VertexControl ), new FrameworkPropertyMetadata( typeof( VertexControl ) ) );


            const FrameworkPropertyMetadataOptions options = FrameworkPropertyMetadataOptions.AffectsRender;

            CenterXProperty = DependencyProperty.Register("CenterX", typeof(double), typeof(VertexControl), new FrameworkPropertyMetadata(0.0, options, OnCenterXChanged));
            CenterYProperty = DependencyProperty.Register("CenterY", typeof(double), typeof(VertexControl), new FrameworkPropertyMetadata(0.0, options, OnCenterYChanged));
            TopLeftXProperty = DependencyProperty.Register("TopLeftX", typeof(double), typeof(VertexControl), new FrameworkPropertyMetadata(0.0, options, OnTopLeftXChanged));
            TopLeftYProperty = DependencyProperty.Register("TopLeftY", typeof(double), typeof(VertexControl), new FrameworkPropertyMetadata(0.0, options, OnTopLeftYChanged));
        }

        private bool m_isChanging;
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (!m_isChanging && (e.Property == ActualHeightProperty || e.Property == ActualWidthProperty))
            {
                var actualControl = UiHelper.FindChild<FrameworkElement>(this, "PART_VertexControl");
                VertexArea = actualControl ?? this;

                m_isChanging = true;

                ComputeProperty(this, TopLeftXProperty, CenterX, TopLeftX, VertexArea.ActualWidth);
                ComputeProperty(this, TopLeftYProperty, CenterY, TopLeftY, VertexArea.ActualHeight);

                //ComputeProperty(this, CenterXProperty, TopLeftX, CenterX, -VertexArea.ActualWidth);
                //ComputeProperty(this, CenterYProperty, TopLeftY, CenterY, -VertexArea.ActualHeight);
                m_isChanging = false;
            }
        }

        private static void ComputeProperty(VertexControl control, DependencyProperty prop, double newValue, double oldValue, double dimension)
        {
            if (control != null)
            {
                newValue = newValue - (dimension / 2);
                if (0.0001 < Math.Abs(newValue - (double)control.GetValue(prop)))
                {
                    control.SetValue(prop, newValue);
                }
            }
        }

        private static void OnCenterXChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as VertexControl;
            if(control != null && !control.m_isChanging)
            {
                var actualControl = UiHelper.FindChild<FrameworkElement>(control, "PART_VertexControl");
                actualControl = actualControl ?? control;

                control.m_isChanging = true;
                ComputeProperty(control, TopLeftXProperty, (double)args.NewValue, (double)args.OldValue, actualControl.ActualWidth);
                control.m_isChanging = false;
            }
        }

        private static void OnCenterYChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as VertexControl;
            if (control != null && !control.m_isChanging)
            {
                var actualControl = UiHelper.FindChild<FrameworkElement>(control, "PART_VertexControl");
                actualControl = actualControl ?? control;

                control.m_isChanging = true;
                ComputeProperty(control, TopLeftYProperty, (double)args.NewValue, (double)args.OldValue, actualControl.ActualHeight);
                control.m_isChanging = false;
            }
        }

        private static void OnTopLeftXChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as VertexControl;
            if (control != null && !control.m_isChanging)
            {
                var actualControl = UiHelper.FindChild<FrameworkElement>(control, "PART_VertexControl");
                actualControl = actualControl ?? control;

                control.m_isChanging = true;
                ComputeProperty(control, CenterXProperty, (double)args.NewValue, (double)args.OldValue, -actualControl.ActualWidth);
                control.m_isChanging = false;
            }
        }

        private static void OnTopLeftYChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as VertexControl;
            if (control != null && !control.m_isChanging)
            {
                var actualControl = UiHelper.FindChild<FrameworkElement>(control, "PART_VertexControl");
                actualControl = actualControl ?? control;

                control.m_isChanging = true;
                ComputeProperty(control, CenterYProperty, (double)args.NewValue, (double)args.OldValue, -actualControl.ActualHeight);
                control.m_isChanging = false;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

		#region IPoolObject Members

		public void Reset()
		{
			Vertex = null;
		}

		public void Terminate()
		{
			//nothing to do, there are no unmanaged resources
		}

		public event DisposingHandler Disposing;

		public void Dispose()
		{
			if ( Disposing != null )
				Disposing( this );
		}

		#endregion
	}
}