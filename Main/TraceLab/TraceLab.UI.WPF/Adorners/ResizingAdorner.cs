using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace TraceLab.UI.WPF.Adorners
{
    public class ResizingAdorner : Adorner
    {
        #region Constructor

        // To store and manage the adorner's visual children.
        private VisualCollection visualChildren;
        
        private Thumb topLeft, topRight, bottomLeft, bottomRight;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizingAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The element to bind the adorner to.</param>
        /// <exception cref="T:System.ArgumentNullException">adornedElement is null.</exception>
        public ResizingAdorner(UIElement adornedElement) : base(adornedElement)
        {
            visualChildren = new VisualCollection(this);
            
            // Call a helper method to initialize the Thumbs
            // with a customized cursors.
            BuildAdornerCorner(ref topLeft, Cursors.SizeNWSE);
            BuildAdornerCorner(ref topRight, Cursors.SizeNESW);
            BuildAdornerCorner(ref bottomLeft, Cursors.SizeNESW);
            BuildAdornerCorner(ref bottomRight, Cursors.SizeNWSE);

            // Add handlers for resizing.
            bottomLeft.DragDelta += new DragDeltaEventHandler(HandleBottomLeft);
            bottomRight.DragDelta += new DragDeltaEventHandler(HandleBottomRight);
            topLeft.DragDelta += new DragDeltaEventHandler(HandleTopLeft);
            topRight.DragDelta += new DragDeltaEventHandler(HandleTopRight);
        }

        /// <summary>
        /// Helper method to instantiate the corner Thumbs, set the Cursor property, 
        /// set some appearance properties, and add the elements to the visual tree.
        /// </summary>
        /// <param name="cornerThumb">The corner thumb.</param>
        /// <param name="customizedCursor">The customized cursor.</param>
        private void BuildAdornerCorner(ref Thumb cornerThumb, Cursor customizedCursor)
        {
            if (cornerThumb != null) return;

            cornerThumb = new Thumb();

            // Set some arbitrary visual characteristics.
            cornerThumb.Cursor = customizedCursor;
            cornerThumb.Height = cornerThumb.Width = 10;
            cornerThumb.Opacity = 0.40;
            cornerThumb.Background = new SolidColorBrush(Colors.MediumBlue);

            visualChildren.Add(cornerThumb);
        }

        #endregion Constructor

        #region Notify Resizing

        /// <summary>
        /// Occurs when control is being resized horizontally.
        /// </summary>
        public event DragDeltaEventHandler ResizingHorizontally;

        /// <summary>
        /// Occurs when control is being resized vertically.
        /// </summary>
        public event DragDeltaEventHandler ResizingVertically;

        private void NotifyResizingHorizontally(DragDeltaEventArgs args)
        {
            if (ResizingHorizontally != null)
                ResizingHorizontally(this, args);
        }

        private void NotifyResizingVertically(DragDeltaEventArgs args)
        {
            if (ResizingVertically != null)
                ResizingVertically(this, args);
        }

        /// <summary>
        /// Notifies that adorner element is being resizing either vertically or horizonally.
        /// Vertical and horizontal delta change is adjusted accordindly to minimum height and minimum width.
        /// Each corner computes it slighly differently, thus required delegates delegates.
        /// </summary>
        /// <param name="minWidth">Minimum width</param>
        /// <param name="oldWidth">Original width of the control</param>
        /// <param name="computedWidth">Width that has been computed with delta args, not necesarilly final widht, as it might have been set to minimum width</param>
        /// <param name="finalWidth">The final new width of the control.</param>
        /// <param name="minHeight">Minimum height.</param>
        /// <param name="oldHeight">Original height of the control</param>
        /// <param name="computedHeight">Height that has been computed with delta args, not necesarilly final height, as it might have been set to minimum height</param>
        /// <param name="finalHeight">The final new height of the control.</param>
        /// <param name="args">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        /// <param name="computeHorizontalChangeFunc">The function that must compute horizontal change adjusted to minimum</param>
        /// <param name="computeVerticalChangeFunc">The function that must compute vertical change adjusted to minimum</param>
        private void NotifyResizing(double minWidth, double oldWidth, double computedWidth, double finalWidth, 
                                    double minHeight, double oldHeight, double computedHeight, double finalHeight, 
                                    DragDeltaEventArgs args, 
                                    CalculateHorizontalChange computeHorizontalChangeFunc,
                                    CalculateVerticalChange computeVerticalChangeFunc)
        {
            double verticalChange = args.VerticalChange;
            double horizontalChange = args.HorizontalChange;
            
            if (finalWidth != oldWidth)
            {
                if (finalWidth == minWidth && computedWidth < minWidth) 
                {
                    horizontalChange = computeHorizontalChangeFunc(minWidth, oldWidth); 
                }

                NotifyResizingHorizontally(new DragDeltaEventArgs(horizontalChange, verticalChange));
            }

            if (finalHeight != oldHeight)
            {
                if (finalHeight == minHeight && computedHeight < minHeight) 
                {
                    verticalChange = computeVerticalChangeFunc(minHeight, oldHeight);
                }

                NotifyResizingVertically(new DragDeltaEventArgs(horizontalChange, verticalChange));
            }
        }

        #endregion Notify Resizing

        #region Thumbs Handlers

        /// <summary>
        /// Handler for resizing from the bottom-right.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        private void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            CalculateWidth calculateWidthFunc = (oldWidth) => { return oldWidth + args.HorizontalChange; };
            CalculateHeight calculateHeightFunc = (oldHeight) => { return oldHeight + args.VerticalChange; };

            CalculateHorizontalChange calculateHorizontalChangeFunc = (minWidth, oldWidth) => { return minWidth - oldWidth; };
            CalculateVerticalChange calculateVerticalChangeFunc = (minHeight, oldHeight) => { return minHeight - oldHeight; };

            // bottom right corner does not need to adjust the position of the element canvas
            SetAdornedElementCanvas setAdornedElementCanvas = (adornedElement, finalHeight, oldHeight, finalWidth, oldWidth) => { };

            HandleResizing(sender, args, calculateWidthFunc, calculateHeightFunc,
                                      calculateHorizontalChangeFunc, calculateVerticalChangeFunc, setAdornedElementCanvas);
        }

        /// <summary>
        /// Handler for resizing from the top-right.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        private void HandleTopRight(object sender, DragDeltaEventArgs args)
        {
            CalculateWidth calculateWidthFunc = (oldWidth) => { return oldWidth + args.HorizontalChange; };
            CalculateHeight calculateHeightFunc = (oldHeight) => { return oldHeight - args.VerticalChange; };

            CalculateHorizontalChange calculateHorizontalChangeFunc = (minWidth, oldWidth) => { return minWidth - oldWidth; };
            CalculateVerticalChange calculateVerticalChangeFunc = (minHeight, oldHeight) => { return oldHeight - minHeight; };

            //top right corner needs to adjust position of top property of the element canvas
            SetAdornedElementCanvas setAdornedElementCanvas =
                (adornedElement, finalHeight, oldHeight, finalWidth, oldWidth) =>
                {
                    double oldCanvasTop = Canvas.GetTop(adornedElement);
                    Canvas.SetTop(adornedElement, oldCanvasTop - (finalHeight - oldHeight));
                };

            HandleResizing(sender, args, calculateWidthFunc, calculateHeightFunc,
                                      calculateHorizontalChangeFunc, calculateVerticalChangeFunc, setAdornedElementCanvas);
        }


        /// <summary>
        /// Handler for resizing from the top-left.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        private void HandleTopLeft(object sender, DragDeltaEventArgs args)
        {
            CalculateWidth calculateWidthFunc = (oldWidth) => { return oldWidth - args.HorizontalChange; };
            CalculateHeight calculateHeightFunc = (oldHeight) => { return oldHeight - args.VerticalChange; };

            CalculateHorizontalChange calculateHorizontalChangeFunc = (minWidth, oldWidth) => { return oldWidth - minWidth; };
            CalculateVerticalChange calculateVerticalChangeFunc = (minHeight, oldHeight) => { return oldHeight - minHeight; };

            //top left corner needs to adjust positions of both top end left properties of the element canvas
            SetAdornedElementCanvas setAdornedElementCanvas =
                (adornedElement, finalHeight, oldHeight, finalWidth, oldWidth) =>
                {
                    double oldCanvasTop = Canvas.GetTop(adornedElement);
                    Canvas.SetTop(adornedElement, oldCanvasTop - (finalHeight - oldHeight));

                    double oldCanvasLeft = Canvas.GetLeft(adornedElement);
                    Canvas.SetLeft(adornedElement, oldCanvasLeft - (finalWidth - oldWidth));
                };

            HandleResizing(sender, args, calculateWidthFunc, calculateHeightFunc,
                                      calculateHorizontalChangeFunc, calculateVerticalChangeFunc, setAdornedElementCanvas);
        }

        /// <summary>
        /// Handler for resizing from the bottom-left.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        private void HandleBottomLeft(object sender, DragDeltaEventArgs args)
        {
            CalculateWidth calculateWidthFunc = (oldWidth) => { return oldWidth - args.HorizontalChange; };
            CalculateHeight calculateHeightFunc = (oldHeight) => { return oldHeight + args.VerticalChange; };

            CalculateHorizontalChange calculateHorizontalChangeFunc = (minWidth, oldWidth) => { return oldWidth - minWidth; };
            CalculateVerticalChange calculateVerticalChangeFunc = (minHeight, oldHeight) => { return minHeight - oldHeight; };

            //bottom left corner needs to adjust position of left property of the element canvas
            SetAdornedElementCanvas setAdornedElementCanvas =
                (adornedElement, finalHeight, oldHeight, finalWidth, oldWidth) =>
                {
                    double oldCanvasLeft = Canvas.GetLeft(adornedElement);
                    Canvas.SetLeft(adornedElement, oldCanvasLeft - (finalWidth - oldWidth));
                };

            HandleResizing(sender, args, calculateWidthFunc, calculateHeightFunc,
                                      calculateHorizontalChangeFunc, calculateVerticalChangeFunc, setAdornedElementCanvas);
        }

        /// <summary>
        /// Handles the delta change of the thumb
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.Controls.Primitives.DragDeltaEventArgs"/> instance containing the event data.</param>
        /// <param name="computeWidthFunc">The function that must compute the new width.</param>
        /// <param name="computeHeightFunc">The function that must compute new height.</param>
        /// <param name="computeHorizontalChangeFunc">The function that must compute horizontal change adjusted to minimum</param>
        /// <param name="computeVerticalChangeFunc">The function that must compute vertical change adjusted to minimum</param>
        /// <param name="setAdornedElementCanvasFunc">The set adorned element canvas func.</param>
        private void HandleResizing(object sender, DragDeltaEventArgs args,
                                 CalculateWidth computeWidthFunc,
                                 CalculateHeight computeHeightFunc,
                                 CalculateHorizontalChange computeHorizontalChangeFunc,
                                 CalculateVerticalChange computeVerticalChangeFunc,
                                 SetAdornedElementCanvas setAdornedElementCanvasFunc)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            double minWidth, oldWidth, computedWidth;
            double minHeight, oldHeight, computedHeight;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);
            
            minWidth = adornedElement.MinWidth == 0 ? hitThumb.DesiredSize.Width : adornedElement.MinWidth;
            minHeight = adornedElement.MinHeight == 0 ? hitThumb.DesiredSize.Height : adornedElement.MinHeight;

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            oldWidth = adornedElement.Width;
            oldHeight = adornedElement.Height;

            //compute width based on delta args
            computedWidth = computeWidthFunc(oldWidth);
            computedHeight = computeHeightFunc(oldHeight);

            double finalWidth = Math.Max(computedWidth, minWidth);
            double finalHeight = Math.Max(computedHeight, minHeight);

            adornedElement.Width = finalWidth;
            adornedElement.Height = finalHeight;
            
            setAdornedElementCanvasFunc(adornedElement, finalHeight, oldHeight, finalWidth, oldWidth);

            double oldCanvasTop = Canvas.GetTop(adornedElement);
            Canvas.SetTop(adornedElement, oldCanvasTop - (finalHeight - oldHeight));

            NotifyResizing(minWidth, oldWidth, computedWidth, finalWidth,
                           minHeight, oldHeight, computedHeight, finalHeight,
                           args, computeHorizontalChangeFunc, computeVerticalChangeFunc);
        }

        #endregion Thumbs Hanglers

        #region Private Delegates

        /// <summary>
        /// Function that must calculate the new width
        /// </summary>
        /// <param name="oldWidth">The old width.</param>
        /// <returns>new potential width</returns>
        private delegate double CalculateWidth(double oldWidth);
        
        /// <summary>
        /// Function that must calculate the new height
        /// </summary>
        /// <param name="oldHeight">The old height.</param>
        /// <returns>new potential height</returns>
        private delegate double CalculateHeight(double oldHeight);

        /// <summary>
        /// Function that must calculate adjusted horizontal change, that does not go over minimum width
        /// </summary>
        /// <param name="minWidth">Width of the min.</param>
        /// <param name="oldWidth">The old width.</param>
        /// <returns>adjusted horizontal change</returns>
        private delegate double CalculateHorizontalChange(double minWidth, double oldWidth);


        /// <summary>
        /// Function that must calculate adjusted vertical change, that does not go over minimum height
        /// </summary>
        /// <param name="minHeight">Height of the min.</param>
        /// <param name="oldHeight">The old height.</param>
        /// <returns></returns>
        private delegate double CalculateVerticalChange(double minHeight, double oldHeight);

        /// <summary>
        /// Function that must adjust the canvas of adorned element, as canvas position top left
        /// shoult be moved to new position, if top or left canvas edge moves. 
        /// </summary>
        /// <param name="adornedElement">The adorned element.</param>
        /// <param name="finalHeight">The final height.</param>
        /// <param name="oldHeight">The old height.</param>
        /// <param name="finalWidth">The final width.</param>
        /// <param name="oldWidth">The old width.</param>
        private delegate void SetAdornedElementCanvas(FrameworkElement adornedElement, 
                                              double finalHeight, double oldHeight,
                                              double finalWidth, double oldWidth);

        #endregion Private Delegates

        #region Other

        /// <summary>
        /// Arrange the Adorners.
        /// </summary>
        /// <param name="finalSize">
        ///     The final area within the parent that this element should use to arrange itself and its children.
        /// </param>
        /// <returns>
        /// The actual size used.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
            // These will be used to place the ResizingAdorner at the corners of the adorned element.  
            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;
            // adornerWidth & adornerHeight are used for placement as well.
            double adornerWidth = this.DesiredSize.Width;
            double adornerHeight = this.DesiredSize.Height;

            topLeft.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            topRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            bottomLeft.Arrange(new Rect(-adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            bottomRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));

            // Return the final size.
            return finalSize;
        }


        /// <summary>
        /// This method ensures that the Widths and Heights are initialized.  Sizing to content produces
        /// Width and Height values of Double.NaN.  Because this Adorner explicitly resizes, the Width and Height
        /// need to be set first.  It also sets the maximum size of the adorned element.
        /// </summary>
        /// <param name="adornedElement">The adorned element.</param>
        private void EnforceSize(FrameworkElement adornedElement)
        {
            if (adornedElement.Width.Equals(Double.NaN))
                adornedElement.Width = adornedElement.DesiredSize.Width;
            if (adornedElement.Height.Equals(Double.NaN))
                adornedElement.Height = adornedElement.DesiredSize.Height;

            FrameworkElement parent = adornedElement.Parent as FrameworkElement;
            if (parent != null)
            {
                adornedElement.MaxHeight = parent.ActualHeight;
                adornedElement.MaxWidth = parent.ActualWidth;
            }
        }

        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        #endregion Other
    }
}
