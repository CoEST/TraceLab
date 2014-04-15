using System.Windows;

namespace GraphSharp.Algorithms.OverlapRemoval
{
	public static class OverlapRemovalHelper
	{
		public static Point GetCenter( this Rect r )
		{
			return new Point( r.Left + r.Width / 2, r.Top + r.Height / 2 );
		}
	}
}