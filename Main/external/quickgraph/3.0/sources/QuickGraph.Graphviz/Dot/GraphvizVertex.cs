namespace QuickGraph.Graphviz.Dot
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Collections.Generic;

    public class GraphvizVertex
    {
        private string bottomLabel = null;
        private string comment = null;
        private double distorsion = 0;
        private GraphvizColor fillColor = GraphvizColor.White;
        private bool fixedSize = false;
        private GraphvizFont font = null;
        private GraphvizColor fontColor = GraphvizColor.Black;
        private string group = null;
        private string label = null;
        private GraphvizLayer layer = null;
        private double orientation = 0;
        private int peripheries = -1;
        private GraphvizRecord record = new GraphvizRecord();
        private bool regular = false;
        private GraphvizVertexShape shape = GraphvizVertexShape.Unspecified;
        private int sides = 4;
        private GraphvizSizeF size = new GraphvizSizeF(0f, 0f);
        private double skew = 0;
        private GraphvizColor strokeColor = GraphvizColor.Black;
        private GraphvizVertexStyle style = GraphvizVertexStyle.Unspecified;
        private string toolTip = null;
        private string topLabel = null;
        private string url = null;
        private double z = -1;
        private GraphvizPoint position;

        internal string GenerateDot(Dictionary<string, object> pairs)
        {
            bool flag = false;
            var writer = new StringWriter();
            foreach (var entry in pairs)
            {
                if (flag)
                {
                    writer.Write(", ");
                }
                else
                {
                    flag = true;
                }
                if (entry.Value is string)
                {
                    writer.Write("{0}=\"{1}\"", entry.Key, entry.Value.ToString());
                    continue;
                }
                if (entry.Value is GraphvizVertexShape)
                {
                    writer.Write("{0}={1}", entry.Key, ((GraphvizVertexShape) entry.Value).ToString().ToLower());
                    continue;
                }
                if (entry.Value is GraphvizVertexStyle)
                {
                    writer.Write("{0}={1}", entry.Key, ((GraphvizVertexStyle) entry.Value).ToString().ToLower());
                    continue;
                }
                if (entry.Value is GraphvizColor)
                {
                    var color = (GraphvizColor) entry.Value;
                    writer.Write("{0}=\"#{1}{2}{3}{4}\"", entry.Key, color.R.ToString("x2").ToUpper(), color.G.ToString("x2").ToUpper(), color.B.ToString("x2").ToUpper(), color.A.ToString("x2").ToUpper());
                    continue;
                }
                if (entry.Value is GraphvizRecord)
                {
                    writer.WriteLine("{0}=\"{1}\"", entry.Key, ((GraphvizRecord) entry.Value).ToDot());
                    continue;
                }
                writer.Write(" {0}={1}", entry.Key, entry.Value.ToString().ToLower());
            }
            return writer.ToString();
        }

        public string ToDot()
        {
            var pairs = new Dictionary<string, object>();
            if (this.Font != null)
            {
                pairs["fontname"] = this.Font.Name;
                pairs["fontsize"] = this.Font.SizeInPoints;
            }
            if (!this.FontColor.Equals(GraphvizColor.Black))
            {
                pairs["fontcolor"] = this.FontColor;
            }
            if (this.Shape != GraphvizVertexShape.Unspecified)
            {
                pairs["shape"] = this.Shape;
            }
            if (this.Style != GraphvizVertexStyle.Unspecified)
            {
                pairs["style"] = this.Style;
            }
            if (this.Shape == GraphvizVertexShape.Record)
            {
                pairs["label"] = this.Record;
            }
            else if (this.Label != null)
            {
                pairs["label"] = this.Label;
            }
            if (this.FixedSize)
            {
                pairs["fixedsize"] = true;
                if (this.Size.Height > 0f)
                {
                    pairs["height"] = this.Size.Height;
                }
                if (this.Size.Width > 0f)
                {
                    pairs["width"] = this.Size.Width;
                }
            }
            if (!this.StrokeColor.Equals(GraphvizColor.Black))
            {
                pairs["color"] = this.StrokeColor;
            }
            if (!this.FillColor.Equals(GraphvizColor.White))
            {
                pairs["fillcolor"] = this.FillColor;
            }
            if (this.Regular)
            {
                pairs["regular"] = this.Regular;
            }
            if (this.Url != null)
            {
                pairs["URL"] = this.Url;
            }
            if (this.ToolTip != null)
            {
                pairs["tooltip"] = this.ToolTip;
            }
            if (this.Comment != null)
            {
                pairs["comment"] = this.Comment;
            }
            if (this.Group != null)
            {
                pairs["group"] = this.Group;
            }
            if (this.Layer != null)
            {
                pairs["layer"] = this.Layer.Name;
            }
            if (this.Orientation > 0)
            {
                pairs["orientation"] = this.Orientation;
            }
            if (this.Peripheries >= 0)
            {
                pairs["peripheries"] = this.Peripheries;
            }
            if (this.Z > 0)
            {
                pairs["z"] = this.Z;
            }
            if (this.position != null)
            {
                pairs["pos"] = String.Format("{0},{1}!", this.position.X, this.position.Y);
            }
            if (((this.Style == GraphvizVertexStyle.Diagonals) || (this.Shape == GraphvizVertexShape.MCircle)) || ((this.Shape == GraphvizVertexShape.MDiamond) || (this.Shape == GraphvizVertexShape.MSquare)))
            {
                if (this.TopLabel != null)
                {
                    pairs["toplabel"] = this.TopLabel;
                }
                if (this.BottomLabel != null)
                {
                    pairs["bottomlable"] = this.BottomLabel;
                }
            }
            if (this.Shape == GraphvizVertexShape.Polygon)
            {
                if (this.Sides != 0)
                {
                    pairs["sides"] = this.Sides;
                }
                if (this.Skew != 0)
                {
                    pairs["skew"] = this.Skew;
                }
                if (this.Distorsion != 0)
                {
                    pairs["distorsion"] = this.Distorsion;
                }
            }

            return this.GenerateDot(pairs);
        }

        public override string ToString()
        {
            return this.ToDot();
        }

        public GraphvizPoint Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public string BottomLabel
        {
            get
            {
                return this.bottomLabel;
            }
            set
            {
                this.bottomLabel = value;
            }
        }

        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = value;
            }
        }

        public double Distorsion
        {
            get
            {
                return this.distorsion;
            }
            set
            {
                this.distorsion = value;
            }
        }

        public GraphvizColor FillColor
        {
            get
            {
                return this.fillColor;
            }
            set
            {
                this.fillColor = value;
            }
        }

        public bool FixedSize
        {
            get
            {
                return this.fixedSize;
            }
            set
            {
                this.fixedSize = value;
            }
        }

        public GraphvizFont Font
        {
            get
            {
                return this.font;
            }
            set
            {
                this.font = value;
            }
        }

        public GraphvizColor FontColor
        {
            get
            {
                return this.fontColor;
            }
            set
            {
                this.fontColor = value;
            }
        }

        public string Group
        {
            get
            {
                return this.group;
            }
            set
            {
                this.group = value;
            }
        }

        public string Label
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
            }
        }

        public GraphvizLayer Layer
        {
            get
            {
                return this.layer;
            }
            set
            {
                this.layer = value;
            }
        }

        public double Orientation
        {
            get
            {
                return this.orientation;
            }
            set
            {
                this.orientation = value;
            }
        }

        public int Peripheries
        {
            get
            {
                return this.peripheries;
            }
            set
            {
                this.peripheries = value;
            }
        }

        public GraphvizRecord Record
        {
            get
            {
                return this.record;
            }
            set
            {
                this.record = value;
            }
        }

        public bool Regular
        {
            get
            {
                return this.regular;
            }
            set
            {
                this.regular = value;
            }
        }

        public GraphvizVertexShape Shape
        {
            get
            {
                return this.shape;
            }
            set
            {
                this.shape = value;
            }
        }

        public int Sides
        {
            get
            {
                return this.sides;
            }
            set
            {
                this.sides = value;
            }
        }

        public GraphvizSizeF Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        public double Skew
        {
            get
            {
                return this.skew;
            }
            set
            {
                this.skew = value;
            }
        }

        public GraphvizColor StrokeColor
        {
            get
            {
                return this.strokeColor;
            }
            set
            {
                this.strokeColor = value;
            }
        }

        public GraphvizVertexStyle Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }

        public string ToolTip
        {
            get
            {
                return this.toolTip;
            }
            set
            {
                this.toolTip = value;
            }
        }

        public string TopLabel
        {
            get
            {
                return this.topLabel;
            }
            set
            {
                this.topLabel = value;
            }
        }

        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }

        public double Z
        {
            get
            {
                return this.z;
            }
            set
            {
                this.z = value;
            }
        }
    }
}

