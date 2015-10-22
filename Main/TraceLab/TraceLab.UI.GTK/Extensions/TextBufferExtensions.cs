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
//
using System;
using System.Collections.Generic;
using Gtk;
using Gdk;

namespace TraceLab.UI.GTK.Extensions
{
    public static class TextBufferExtensions
    {
        public static void InitTags(this TextBuffer buffer)
        {
            TextTag tag  = new TextTag ("heading");
            tag.Weight = Pango.Weight.Bold;
            tag.Size = (int) Pango.Scale.PangoScale * 15;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("italic");
            tag.Style = Pango.Style.Italic;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("bold");
            tag.Weight = Pango.Weight.Bold;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("big");
            tag.Size = (int) Pango.Scale.PangoScale * 20;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("xx-small");
            tag.Scale = Pango.Scale.XXSmall;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("x-large");
            tag.Scale = Pango.Scale.XLarge;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("monospace");
            tag.Family = "monospace";
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("blue_foreground");
            tag.Foreground = "blue";
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("red_background");
            tag.Background = "red";
            buffer.TagTable.Add (tag);

            // The C gtk-demo passes NULL for the drawable param, which isn't
            // multi-head safe, so it seems bad to allow it in the C# API.
            // But the Window isn't realized at this point, so we can't get
            // an actual Drawable from it. So we kludge for now.
            Pixmap stipple = Pixmap.CreateBitmapFromData (Gdk.Screen.Default.RootWindow, gray50_bits, gray50_width, gray50_height);

            tag  = new TextTag ("background_stipple");
            tag.BackgroundStipple = stipple;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("foreground_stipple");
            tag.ForegroundStipple = stipple;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("big_gap_before_line");
            tag.PixelsAboveLines = 30;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("big_gap_after_line");
            tag.PixelsBelowLines = 30;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("double_spaced_line");
            tag.PixelsInsideWrap = 10;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("not_editable");
            tag.Editable = false;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("word_wrap");
            tag.WrapMode = WrapMode.Word;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("char_wrap");
            tag.WrapMode = WrapMode.Char;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("no_wrap");
            tag.WrapMode = WrapMode.None;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("center");
            tag.Justification = Justification.Center;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("right_justify");
            tag.Justification = Justification.Right;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("wide_margins");
            tag.LeftMargin = 50;
            tag.RightMargin = 50;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("strikethrough");
            tag.Strikethrough = true;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("underline");
            tag.Underline = Pango.Underline.Single;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("double_underline");
            tag.Underline = Pango.Underline.Double;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("superscript");
            tag.Rise = (int) Pango.Scale.PangoScale * 10;
            tag.Size = (int) Pango.Scale.PangoScale * 8;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("subscript");
            tag.Rise = (int) Pango.Scale.PangoScale * -10;
            tag.Size = (int) Pango.Scale.PangoScale * 8;
            buffer.TagTable.Add (tag);

            tag  = new TextTag ("rtl_quote");
            tag.WrapMode = WrapMode.Word;
            tag.Direction = TextDirection.Rtl;
            tag.Indent = 30;
            tag.LeftMargin = 20;
            tag.RightMargin = 20;
            buffer.TagTable.Add (tag);
        }

        const int gray50_width = 2;
        const int gray50_height = 2;
        const string gray50_bits = "\x02\x01";
    }
}

