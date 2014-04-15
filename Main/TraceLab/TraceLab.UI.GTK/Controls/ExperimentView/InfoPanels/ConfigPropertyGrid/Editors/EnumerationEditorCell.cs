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
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using MonoDevelop.Components.PropertyGrid;

namespace TraceLab.UI.GTK.PropertyGridEditors
{
    /// <summary>
    /// Enumeration editor cell.
    /// Note, that this is not original MonoDevelop editor.
    /// This editor is specific for TraceLab component purpose.
    /// </summary>
	[PropertyEditorType(typeof(System.Enum))]
	public class EnumerationEditorCell: PropertyEditorCell
	{
		protected override string GetValueText ()
		{
			if (Value == null)
				return "";

			string val = Value.ToString ();
			
			// If the enum value has a Description attribute, return the description
			foreach (FieldInfo f in Property.PropertyType.GetFields ()) {
				if (f.Name == val) {
					DescriptionAttribute att = (DescriptionAttribute) Attribute.GetCustomAttribute (f, typeof(DescriptionAttribute));
					if (att != null)
						return att.Description;
					else
						return val;
				}
			}
			return val;
		}
		
		protected override IPropertyEditor CreateEditor (Gdk.Rectangle cell_area, Gtk.StateType state)
		{
			return new EnumerationEditor ();
		}
	}
	
	public class EnumerationEditor : Gtk.HBox, IPropertyEditor {

		Gtk.EventBox ebox;
		Gtk.ComboBoxEntry combo;
		Array values;
        PropertyDescriptor prop;

		public EnumerationEditor () : base (false, 0)
		{
		}
		
		public void Initialize (EditSession session)
		{
            prop = session.Property;
           
            ICollection valuesCollection = prop.Converter.GetStandardValues();
            values = new TraceLab.Core.Components.EnumValue[valuesCollection.Count];
            valuesCollection.CopyTo(values, 0);

			//values = System.Enum.GetValues (prop.PropertyType);
			Hashtable names = new Hashtable ();
			foreach (FieldInfo f in prop.PropertyType.GetFields ()) {
				DescriptionAttribute att = (DescriptionAttribute) Attribute.GetCustomAttribute (f, typeof(DescriptionAttribute));
				if (att != null)
					names [f.Name] = att.Description;
				else
					names [f.Name] = f.Name;
			}
				       
			
			ebox = new Gtk.EventBox ();
			ebox.Show ();
			PackStart (ebox, true, true, 0);

			combo = Gtk.ComboBoxEntry.NewText ();
			combo.Changed += combo_Changed;
			combo.Entry.IsEditable = false;
			combo.Entry.CanFocus = false;
			combo.Entry.HasFrame = false;
			combo.Entry.HeightRequest = combo.SizeRequest ().Height;
			combo.Show ();
			ebox.Add (combo);

            foreach (TraceLab.Core.Components.EnumValue value in values) {
				string str = prop.Converter.ConvertToString (value);
				if (names.Contains (str))
					str = (string) names [str];
				combo.AppendText (str);
			}
		}
		
		protected override void OnDestroyed ()
		{
			base.OnDestroyed ();
			((IDisposable)this).Dispose ();
		}

		void IDisposable.Dispose ()
		{
		}

		public object Value {
			get {
                return values.GetValue (combo.Active);
			}
			set {
				int i = Array.IndexOf (values, value);
				if (i != -1)
					combo.Active = i;
			}
		}

		public event EventHandler ValueChanged;

		void combo_Changed (object o, EventArgs args)
		{
			if (ValueChanged != null)
				ValueChanged (this, EventArgs.Empty);
			ebox.TooltipText = Value != null ? Value.ToString () : null;
		}
	}
}
