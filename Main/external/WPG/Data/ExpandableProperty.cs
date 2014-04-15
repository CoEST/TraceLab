using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;


namespace WPG.Data
{
    class ExpandableProperty : Property
    {
        private PropertyCollection _propertyCollection;
        private bool _automaticlyExpandObjects;
        private string _filter;
        public ExpandableProperty(object instance, PropertyDescriptor property, bool automaticlyExpandObjects, string filter, bool readOnly)
            : base(instance, property, readOnly)
        {
            _automaticlyExpandObjects = automaticlyExpandObjects;
            _filter = filter;
        }

        public ObservableCollection<Item> Items
        {
            get
            {

                if (_propertyCollection == null)
                {
                    //Lazy initialisation prevent from deep search and looping
                    _propertyCollection = new PropertyCollection(_property.GetValue(_instance), true, _automaticlyExpandObjects, _filter, _readOnly);
                }

                return _propertyCollection.Items;
            }
        }

		public string Count
		{
			get
			{
				if (_property.PropertyType.GetInterface("ICollection") != null)
					return string.Format("Count: {0}", Items.Count);
				return string.Empty;
			}
		}
    }
}
