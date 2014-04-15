using System.Diagnostics;
using System.Xml.Serialization;
using System.Windows;
namespace GraphSharp.Sample
{
	/// <summary>
	/// A simple identifiable vertex.
	/// </summary>
	[DebuggerDisplay( "{ID}" )]
	public class PocVertex : DependencyObject
	{
        private string m_id;
		public string ID
		{
            get { return m_id; }
            private set { m_id = value; }
		}

        [XmlAttribute("desc")]
        public string Desc
        {
            get;
            set;
        }

        public static DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(PocVertex), new PropertyMetadata(0.0, null, ConstrainDouble));
        public static DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(PocVertex), new PropertyMetadata(0.0, null, ConstrainDouble));

        private static object ConstrainDouble(DependencyObject d, object baseValue)
        {
            double val = (double)baseValue;
            val = System.Math.Round(val, 3);
            
            return val;
        }

        [XmlAttribute("X")]
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(YProperty, value); }
        }

        [XmlAttribute("Y")]
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

		public PocVertex( string id )
		{
			ID = id;
		}

        public override string ToString()
        {
            return ID;
        }
	}
}