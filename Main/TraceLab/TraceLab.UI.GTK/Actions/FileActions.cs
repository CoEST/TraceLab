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
using Mono.Unix;
using Gtk;

namespace TraceLab.UI.GTK
{
    public class FileActions
    {
        public Gtk.Action New { get; private set; }
        public Gtk.Action Open { get; private set; }
        public Gtk.Action Close { get; private set; }
        public Gtk.Action Save { get; private set; }
        public Gtk.Action SaveAs { get; private set; }
        public Gtk.Action Exit { get; private set; }


        public Gtk.Action Settings { get; private set; }


        public FileActions()
        {
            New = new Gtk.Action("New", Catalog.GetString("New..."), null, Stock.New);
            Open = new Gtk.Action("Open", Catalog.GetString("Open..."), null, Stock.Open);
            Close = new Gtk.Action("Close", Catalog.GetString("Close"), null, Stock.Close);
            Save = new Gtk.Action("Save", Catalog.GetString("Save"), null, Stock.Save);
            SaveAs = new Gtk.Action("SaveAs", Catalog.GetString("Save As..."), null, Stock.SaveAs);
            Exit = new Gtk.Action("Exit", Catalog.GetString("Quit"), null, Stock.Quit);


            New = new Gtk.Action("New", Catalog.GetString("New..."), null, Stock.New);

            New.ShortLabel = Catalog.GetString("New");
            Open.ShortLabel = Catalog.GetString("Open");
            Open.IsImportant = true;
            Save.IsImportant = true;


            Settings = new Gtk.Action("Settings", Catalog.GetString("Settings..."), null, Stock.Properties);
            

            Close.Sensitive = false;

        }

        public void RegisterHandlers()
        {

        }
    }
}

