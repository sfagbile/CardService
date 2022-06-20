using Shared.Extensions.ExtensionsModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shared.Extensions
{
	/// <summary>
	/// </summary>
	public static class EnumExtension
	{
		/// <summary>
		///     Gets the Description attribute of an Enum item.
		/// </summary>
		public static string GetDescription(this Enum value)
		{
			if (value == null)
			{
				return string.Empty;
			}

			var field = value.GetType().GetField(value.ToString());

			if (field == null)
			{
				return string.Empty;
			}

			return !(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute
				)
				? value.ToString()
				: attribute.Description;
		}

		/// <summary>
		/// Gets the Display short name attribute of an Enum item.
		/// </summary>
		public static string GetDisplayShortName(this Enum value)
		{
			if (value == null)
			{
				return string.Empty;
			}

			var field = value.GetType().GetField(value.ToString());

			if (field == null)
			{
				return string.Empty;
			}

			return
				!(Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
					? value.ToString()
					: attribute.ShortName;
		}

		/// <summary>
		/// </summary>
		public static List<string> GetDescriptions(this Type type)
		{
			var descriptions = new List<string>();
			var names = Enum.GetNames(type);

			foreach (var name in names)
			{
				var field = type.GetField(name);

				if (field is null)
				{
					continue;
				}

				var customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
				descriptions.AddRange(from DescriptionAttribute description in customAttributes
				                      select description.Description);
			}

			return descriptions;
		}

		public static List<EnumValue> GetValues<T>()
		{
			List<EnumValue> values = new List<EnumValue>();
			foreach (var itemType in Enum.GetValues(typeof(T)))
			{
				//For each value of this enumeration, add a new EnumValue instance
				values.Add(new EnumValue()
				{
					Name = Enum.GetName(typeof(T), itemType),
					Value = (int)itemType
				});
			}
			return values;
		}
	}
}