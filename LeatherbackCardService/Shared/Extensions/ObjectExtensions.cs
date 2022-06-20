using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Newtonsoft.Json;

namespace Shared.Extensions
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Returns the Object if not null. If it is, initializes a new Object of the same type.
		/// </summary>
		/// <returns><see cref="T"/></returns>
		public static T AsNotNull<T>(this T obj) where T : new()
		{
			return obj == null ? new T() : obj;
		}

		public static T TryDeserializeObject<T>(this string jsonString, JsonSerializerSettings settings = null)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(jsonString, settings);
			}
			catch (Exception)
			{
				return default;
			}
		}

		public static string ToJson<T>(this T obj, JsonSerializerSettings settings = null)
		{
			return JsonConvert.SerializeObject(obj, settings ?? new JsonSerializerSettings());
		}
		
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
		{
			return source == null || !source.Any();
		}

		public static string ToSnakeCase(this string source)
		{
			return string.IsNullOrEmpty(source) ?
				string.Empty :
				string.Concat(source.Select((@char, index) => index > 0 && char.IsUpper(@char) ? "_" + @char : @char.ToString())).ToLower();
		}
		
		public static List<KeyValuePair<string, string>> ToFormData(this object source)
		{
			return source?.GetType()
			             .GetProperties()
			             .Select(property => new KeyValuePair<string, string>(property.Name, property.GetValue(source)?.ToString()))
			             .ToList();
		}

		public static List<KeyValuePair<string, string>> ToSnakeCaseFormData(this object source, bool excludeNullValues = false)
		{
			return source?.GetType()
			             .GetProperties()
			             .SelectMany(property =>
			             {
				             var propertyValue = property.GetValue(source);
				             
				             if (propertyValue is IEnumerable<object> propertyValues)
				             {
					             return propertyValues.Select(value => new KeyValuePair<string, string>($"{property.Name.ToSnakeCase()}[]", 
						             value?.ToString()));
				             }
				             
				             return new List<KeyValuePair<string, string>>{ new KeyValuePair<string, string>(property.Name.ToSnakeCase(),
						             propertyValue?.ToString()) };
			             })
			             .Where(x => !excludeNullValues || !string.IsNullOrEmpty(x.Value))
			             .ToList();
		}

		public static string ToQueryString(this object parameters)
		{
			if (parameters == null)
			{
				return string.Empty;
			}
			
			var queryStringParameters = new List<string>();

			foreach (var property in parameters.GetType().GetProperties())
			{
				var encodedPropertyName = HttpUtility.UrlEncode(property.Name).ToLower();
				var propertyValue = property.GetValue(parameters);

				if (propertyValue is IEnumerable<object> propertyValues)
				{
					foreach (var item in propertyValues)
					{
						var encodedItemValue = HttpUtility.UrlEncode(item?.ToString());

						if (!string.IsNullOrEmpty(encodedItemValue))
						{
							queryStringParameters.Add($"{encodedPropertyName}={encodedItemValue}");
						}
					}

					continue;
				}

				var encodedPropertyValue = HttpUtility.UrlEncode(propertyValue?.ToString());

				if (!string.IsNullOrEmpty(encodedPropertyValue))
				{
					queryStringParameters.Add($"{encodedPropertyName}={encodedPropertyValue}");
				}
			}

			return "?" + string.Join("&", queryStringParameters);
		}

		public static string ToSnakeCaseQueryString(this object parameters)
		{
			if (parameters == null)
			{
				return string.Empty;
			}

			var queryStringParameters = new List<string>();

			foreach (var property in parameters.GetType().GetProperties())
			{
				var encodedPropertyName = HttpUtility.UrlEncode(property.Name).ToSnakeCase();
				var propertyValue = property.GetValue(parameters);

				if (propertyValue is IEnumerable<object> propertyValues)
				{
					foreach (var item in propertyValues)
					{
						var encodedItemValue = HttpUtility.UrlEncode(item?.ToString());

						if (!string.IsNullOrEmpty(encodedItemValue))
						{
							queryStringParameters.Add($"{encodedPropertyName}={encodedItemValue}");
						}
					}

					continue;
				}

				var encodedPropertyValue = HttpUtility.UrlEncode(propertyValue?.ToString());

				if (!string.IsNullOrEmpty(encodedPropertyValue))
				{
					queryStringParameters.Add($"{encodedPropertyName}={encodedPropertyValue}");
				}
			}

			return "?" + string.Join("&", queryStringParameters);
		}

		public static string ToTitleCase(this string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}

			var textInfo = Thread.CurrentThread?.CurrentCulture?.TextInfo;

			return textInfo.ToTitleCase(text);
		}
	}
}