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

static class InfoPanelUtils
{
    static InfoPanelUtils()
    {
        redTag = new Gtk.TextTag("red-foreground");
        redTag.Foreground = "#ff2828";
        
        blueTag = new Gtk.TextTag("blue-foreground");
        blueTag.Foreground = "#3333ff";
    }

    private static Gtk.TextTag redTag;
    private static Gtk.TextTag blueTag;

    internal static void InitColorTags(this Gtk.TextView textView) 
    {
        textView.Buffer.TagTable.Add(blueTag);
        textView.Buffer.TagTable.Add(redTag);
    }

    internal static void ShowError(this Gtk.TextView textView, string errorMessage)
    {
        ShowTextView(textView, errorMessage, redTag);
    }

    internal static void ShowInfoMessage(this Gtk.TextView textView, string message)
    {
        ShowTextView(textView, message, blueTag);
    }

    private static void ShowTextView (Gtk.TextView textView, string errorMessage, Gtk.TextTag tag)
    {
        textView.Visible = true;
        textView.Buffer.Text = errorMessage;
        Gtk.TextIter start, end;
        textView.Buffer.GetBounds (out start, out end);
        textView.Buffer.ApplyTag (tag, start, end);
    }
}