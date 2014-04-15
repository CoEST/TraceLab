using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using WPG.Controls.WPG.Converters;

namespace WPG.Data
{
    public class PropertyCollection : CompositeItem
    {
        #region Initialization

        public PropertyCollection() { }

        public PropertyCollection(object instance, bool noCategory, bool automaticlyExpandObjects, string filter, bool readOnly)
        {
            List<Property> propertyCollection = new List<Property>();

			// If this instance is a collection, then we want to display the items inside.
			if(instance is System.Collections.ICollection)
			{
				TypeDescriptor.AddProvider(new CollectionPropertyDescriptionProvider(), instance);
				TypeDescriptor.Refresh(instance);
			}
			BuildNormalCollection(instance, noCategory, automaticlyExpandObjects, propertyCollection, filter, readOnly);
        }

		private void BuildNormalCollection(object instance, bool noCategory, bool automaticlyExpandObjects, List<Property> propertyCollection, string filter, bool readOnly)
		{
			Dictionary<string, PropertyCategory> groups = new Dictionary<string, PropertyCategory>();
			PropertyDescriptorCollection properties;

			if (instance != null)
				properties = TypeDescriptor.GetProperties(instance);  //I changed here from instance to instance.GetType, so that only the Direct Properties are shown!
			else
				properties = new PropertyDescriptorCollection(new PropertyDescriptor[] { });

			foreach (PropertyDescriptor propertyDescriptor in properties)
			{
				CollectProperties(instance, propertyDescriptor, propertyCollection, automaticlyExpandObjects, filter, readOnly);
				if (noCategory)
					propertyCollection.Sort(Property.CompareByName);
				else
					propertyCollection.Sort(Property.CompareByCategoryThenByName);
			}

			if (noCategory)
			{

				foreach (Property property in propertyCollection)
				{
					if (filter == "" || property.Name.ToLower().Contains(filter))
						Items.Add(property);
				}
			}
			else
			{
				foreach (Property property in propertyCollection)
				{
					if (filter == "" || property.Name.ToLower().Contains(filter))
					{
						PropertyCategory propertyCategory;
						if (groups.ContainsKey(property.Category))
						{
							propertyCategory = groups[property.Category];
						}
						else
						{
							propertyCategory = new PropertyCategory(property.Category);
							groups[property.Category] = propertyCategory;
							Items.Add(propertyCategory);
						}
						propertyCategory.Items.Add(property);
					}
				}
			}
		}

		private Property CreatePropertyObject(bool automaticlyExpandObjects, object instance, PropertyDescriptor descriptor, string filter, bool readOnly)
		{
			Property newProp = null;
			//Add a property with Name: AutomaticlyExpandObjects
			Type propertyType = descriptor.PropertyType;
			if (automaticlyExpandObjects && propertyType.IsClass && !propertyType.IsArray && propertyType != typeof(string))
			{
				newProp = new ExpandableProperty(instance, descriptor, automaticlyExpandObjects, filter, readOnly);
			}
			else if (descriptor.Converter.GetType().IsSubclassOf(typeof(ExpandableObjectConverter)))
			{
				newProp = new ExpandableProperty(instance, descriptor, automaticlyExpandObjects, filter, readOnly);
			}
			else
				newProp = new Property(instance, descriptor, readOnly);

			return newProp;
		}

        private void CollectProperties(object instance, PropertyDescriptor descriptor, List<Property> propertyCollection, bool automaticlyExpandObjects, string filter, bool readOnly)
        {
            
            if (descriptor.Attributes[typeof(FlatAttribute)] == null)
            {
                if (descriptor.IsBrowsable)
                {
					propertyCollection.Add(CreatePropertyObject(automaticlyExpandObjects, instance, descriptor, filter, readOnly));
                }
            }
            else
            {
                instance = descriptor.GetValue(instance);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(instance);
                foreach (PropertyDescriptor propertyDescriptor in properties)
                {
                    CollectProperties(instance, propertyDescriptor, propertyCollection, automaticlyExpandObjects, filter, readOnly);
                }
            }
        }

        #endregion
    }
}
