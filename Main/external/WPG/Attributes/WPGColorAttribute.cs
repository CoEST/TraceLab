using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WPG.Controls.WPG.Attributes
{
	/// <summary>
	/// Properties with this attribute will be able to have custom colors applied in the WPG grid.
	/// 
	/// Note: These colors will apply to the name of the property *only*
	/// </summary>
	public sealed class WPGIsModified : Attribute
	{
		public WPGIsModified()
		{
		}
	}

	public class ColorizedPropertyDescriptionProvider : TypeDescriptionProvider
	{
		private static TypeDescriptionProvider defaultTypeProvider = TypeDescriptor.GetProvider(typeof(object));

		public ColorizedPropertyDescriptionProvider()
			: base(defaultTypeProvider)
		{
		}

		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType,
																object instance)
		{
			ICustomTypeDescriptor defaultDescriptor = base.GetTypeDescriptor(objectType, instance);

			if (instance != null)
			{
				defaultDescriptor = new ColorizedPropertyCustomTypeDescriptor(defaultDescriptor, instance);
			}

			return defaultDescriptor;
		}
	}

	class ColorizedPropertyCustomTypeDescriptor : CustomTypeDescriptor
	{
		private static Attribute[] m_colorAttribute = new Attribute[] { new WPGIsModified() };

		public ColorizedPropertyCustomTypeDescriptor(ICustomTypeDescriptor parent, object instance)
			: base(parent)
		{
		}

		private List<PropertyDescriptor> m_customFields = new List<PropertyDescriptor>();

		public override AttributeCollection GetAttributes()
		{
			return new AttributeCollection(base.GetAttributes().Cast<Attribute>().Union(m_colorAttribute).ToArray());
		}
	}
}
