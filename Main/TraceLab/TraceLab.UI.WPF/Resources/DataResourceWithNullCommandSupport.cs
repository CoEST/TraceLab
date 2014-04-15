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
using System.Windows.Data;
using System.Windows.Markup;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace WpfMentor.CommandParameterBindings
{
	public class DataResource : Freezable
	{
		/// <summary>
		/// Identifies the <see cref="BindingTarget"/> dependency property.
		/// </summary>
		/// <value>
		/// The identifier for the <see cref="BindingTarget"/> dependency property.
		/// </value>
		public static readonly DependencyProperty BindingTargetProperty = DependencyProperty.Register("BindingTarget", typeof(object), typeof(DataResource), new UIPropertyMetadata(null));

		/// <summary>
		/// Initializes a new s_instance of the <see cref="DataResource"/> class.
		/// </summary>
		public DataResource()
		{
		}

		/// <summary>
		/// Gets or sets the binding target.
		/// </summary>
		/// <value>The binding target.</value>
		public object BindingTarget
		{
			get { return (object)GetValue(BindingTargetProperty); }
			set { SetValue(BindingTargetProperty, value); }
		}

		/// <summary>
		/// Creates an s_instance of the specified type using that type's default constructor. 
		/// </summary>
		/// <returns>
		/// A reference to the newly created object.
		/// </returns>
		protected override Freezable CreateInstanceCore()
		{
			return (Freezable)Activator.CreateInstance(GetType());
		}

		/// <summary>
		/// Makes the s_instance a clone (deep copy) of the specified <see cref="Freezable"/>
		/// using base (non-animated) property values. 
		/// </summary>
		/// <param name="sourceFreezable">
		/// The object to clone.
		/// </param>
		protected sealed override void CloneCore(Freezable sourceFreezable)
		{
			base.CloneCore(sourceFreezable);
		}
	}

    [ContentProperty("DataResource")]
	public class DataResourceBindingExtension : MarkupExtension
	{
		private object mTargetObject;
		private object mTargetProperty;
		private DataResource mDataResouce;

		/// <summary>
		/// Gets or sets the data resource.
		/// </summary>
		/// <value>The data resource.</value>
		public DataResource DataResource
		{
			get
			{
				return mDataResouce;
			}
			set
			{
				if (mDataResouce != value)
				{
					if (mDataResouce != null)
					{
						mDataResouce.Changed -= DataResource_Changed;
					}
					mDataResouce = value;

					if (mDataResouce != null)
					{
						mDataResouce.Changed += DataResource_Changed;
					}
				}
			}
		}

		/// <summary>
		/// Initializes a new s_instance of the <see cref="DataResourceBindingExtension"/> class.
		/// </summary>
		public DataResourceBindingExtension()
		{
		}

		/// <summary>
		/// When implemented in a derived class, returns an object that is set as the value of the target property for this markup extension.
		/// </summary>
		/// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
		/// <returns>
		/// The object value to set on the property where the extension is applied.
		/// </returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			IProvideValueTarget target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (target.TargetObject is DependencyObject)
            {

                mTargetObject = target.TargetObject;
                mTargetProperty = target.TargetProperty;

                // mTargetProperty can be null when this is called in the Designer.
                Debug.Assert(mTargetProperty != null || DesignerProperties.GetIsInDesignMode(new DependencyObject()));

                if (DataResource.BindingTarget == null && mTargetProperty != null)
                {
                    PropertyInfo propInfo = mTargetProperty as PropertyInfo;
                    if (propInfo != null)
                    {
                        try
                        {
                            // extra support for commands here. Some classes don't allow a null
                            // command for example InputBinding. In this case we use a dummy
                            // command (NullCommand) instead of setting a null value.

                            if (propInfo.PropertyType.IsAssignableFrom(typeof(ICommand)))
                            {
                                return new NullCommand();
                            }

                            return Activator.CreateInstance(propInfo.PropertyType);
                        }
                        catch (MissingMethodException)
                        {
                            // there isn't a default constructor
                        }
                    }

                    DependencyProperty depProp = mTargetProperty as DependencyProperty;
                    if (depProp != null)
                    {
                        DependencyObject depObj = (DependencyObject)mTargetObject;
                        return depObj.GetValue(depProp);
                    }
                }

                return DataResource.BindingTarget;
            }
            else
            {
                return this;
            }
		}

		private void DataResource_Changed(object sender, EventArgs e)
		{
			// Ensure that the bound object is updated when DataResource changes.
			DataResource dataResource = (DataResource)sender;
			DependencyProperty depProp = mTargetProperty as DependencyProperty;
			
			if (depProp != null)
			{
				DependencyObject depObj = (DependencyObject)mTargetObject;
				object value = Convert(dataResource.BindingTarget, depProp.PropertyType);
				depObj.SetValue(depProp, value);
			}
			else
			{
				PropertyInfo propInfo = mTargetProperty as PropertyInfo;
				if (propInfo != null)
				{
					object value = Convert(dataResource.BindingTarget, propInfo.PropertyType);

                    // extra support for commands here. Some classes don't allow a null
                    // command for example InputBinding. In this case we use a dummy
                    // command (NullCommand) instead of setting a null value.
                    
                    if (value == null && propInfo.PropertyType.IsAssignableFrom( typeof( ICommand ) ) )
                    {
                        value = new NullCommand();
                    }

                    propInfo.SetValue(mTargetObject, value, new object[0]);
				}
			}
		}

		private object Convert(object obj, Type toType)
		{
			try
			{
				return System.Convert.ChangeType(obj, toType);
			}
			catch (InvalidCastException)
			{
				return obj;
			}
		}

        private class NullCommand : ICommand
        {
            #region ICommand Members

            public bool CanExecute( object parameter )
            {
                return false;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute( object parameter )
            {
            }

            private void Raise()
            {
                if (CanExecuteChanged != null)
                    CanExecuteChanged(this, new EventArgs());
            }

            #endregion
        }
	}
}
