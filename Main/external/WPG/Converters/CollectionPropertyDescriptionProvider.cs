using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace WPG.Controls.WPG.Converters
{
	internal class EnumerableElement
	{
		public EnumerableElement(int index, object element)
		{
			Name = string.Format("[{0:D2}]", index);
			DataType = element.GetType();
			Element = element;
			Index = index;
		}

		public string Name { get; private set; }
		public Type DataType { get; private set; }
		public int Index { get; private set; }
		public object Element { get; private set; }
	}

	class CollectionPropertyDescriptionProvider : TypeDescriptionProvider
	{
		private static TypeDescriptionProvider defaultTypeProvider = TypeDescriptor.GetProvider(typeof(object));

		public CollectionPropertyDescriptionProvider()
			: base(defaultTypeProvider)
		{
		}

		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType,
																object instance)
		{
			ICustomTypeDescriptor defaultDescriptor = base.GetTypeDescriptor(objectType, instance);

			if (instance != null && instance is ICollection)
			{
				defaultDescriptor = new CollectionPropertyCustomTypeDescriptor(defaultDescriptor, instance as ICollection);
			}

			return defaultDescriptor;
		}
	}

	class CollectionPropertyCustomTypeDescriptor : CustomTypeDescriptor
	{
		public CollectionPropertyCustomTypeDescriptor(ICustomTypeDescriptor parent, ICollection instance)
			: base(parent)
		{
			m_customFields.AddRange(CustomFieldsGenerator.GenerateCustomFields(instance)
				.Select(f => new CustomFieldPropertyDescriptor(f)).Cast<PropertyDescriptor>());
		}

		private List<PropertyDescriptor> m_customFields = new List<PropertyDescriptor>();

		public override PropertyDescriptorCollection GetProperties()
		{
			HashSet<PropertyDescriptor> hashCollection = new HashSet<PropertyDescriptor>(base.GetProperties().Cast<PropertyDescriptor>());
			hashCollection.UnionWith(m_customFields);
			PropertyDescriptorCollection collection = new PropertyDescriptorCollection(hashCollection.ToArray());
			return collection;
		}

		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			// note, we're ignoring existing properties and going ONLY with the expandable items.
			return new PropertyDescriptorCollection(m_customFields.ToArray());
		}
	}

	internal static class CustomFieldsGenerator
	{
		internal static IEnumerable<EnumerableElement> GenerateCustomFields(ICollection instance)
		{
			List<EnumerableElement> customFields = new List<EnumerableElement>();

			int i = 0;
			foreach(object obj in instance)
			{
				customFields.Add(new EnumerableElement(i, obj));
				++i;
			}

			return customFields;
		}
	}

	class CustomFieldPropertyDescriptor : PropertyDescriptor
	{
		public EnumerableElement Field { get; private set; }

		public override int GetHashCode()
		{
			return Field.Index;
		}

		public override bool Equals(object obj)
		{
			if (obj is CustomFieldPropertyDescriptor)
			{
				return Field.Index == (obj as CustomFieldPropertyDescriptor).Field.Index;
			}

			return false;
		}

		public CustomFieldPropertyDescriptor(EnumerableElement customField)
			: base(customField.Name, new Attribute[0])
		{
			Field = customField;
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override Type ComponentType
		{
			get
			{
				return typeof(object);
			}
		}

		public override object GetValue(object component)
		{
			ICollection instance = (ICollection)component;
			object obj = null;
			int i = 0;
			foreach(object objectInCollection in instance)
			{
				if(i == Field.Index)
				{
					obj = objectInCollection;
					break;
				}
				i++;
			}

			return obj;
		}

		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public override Type PropertyType
		{
			get
			{
				return Field.DataType;
			}
		}

		public override void ResetValue(object component)
		{
			throw new NotImplementedException();
		}

		public override void SetValue(object component, object value)
		{
			throw new NotImplementedException();
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
	}
}
