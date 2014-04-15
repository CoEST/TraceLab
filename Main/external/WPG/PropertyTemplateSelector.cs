using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

using WPG.Data;

namespace WPG
{
	public class PropertyTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			Property property = item as Property;
			if (property == null)
			{
				throw new ArgumentException("item must be of type Property");
			}
			FrameworkElement element = container as FrameworkElement;
			if (element == null)
			{
				return base.SelectTemplate(property.Value, container);
			}
			DataTemplate template = FindDataTemplate(property, element);
			return template;
		}		

		private DataTemplate FindDataTemplate(Property property, FrameworkElement element)
		{
			Type propertyType = property.PropertyType;
			DataTemplate template = TryFindDataTemplate(element, propertyType);
			while (template == null && propertyType.BaseType != null)
			{
				propertyType = propertyType.BaseType;
				template = TryFindDataTemplate(element, propertyType);
			}

			if(template == null)
			{
				template = FindDataTemplateForInterfaces(property.PropertyType, element);
			}

			if (template == null)
			{
				template = TryFindDataTemplate(element, "default");
			}
			return template;
		}

		private DataTemplate FindDataTemplateForInterfaces(Type propertyType, FrameworkElement element)
		{
			// Now go through the interfaces for this type.
			Type[] interfaces = propertyType.GetInterfaces();

			DataTemplate template = null;
			foreach(Type interfaceType in interfaces)
			{
				template = TryFindDataTemplate(element, interfaceType);
				if(template != null)
					break;
			}
			return template;
		}

		private static DataTemplate TryFindDataTemplate(FrameworkElement element, object dataTemplateKey)
		{
			object dataTemplate = element.TryFindResource(dataTemplateKey);
			if (dataTemplate == null)
			{
				dataTemplateKey = new ComponentResourceKey(typeof(PropertyGrid), dataTemplateKey);
				dataTemplate = element.TryFindResource(dataTemplateKey);
			}
			return dataTemplate as DataTemplate;
		}
	}
}
