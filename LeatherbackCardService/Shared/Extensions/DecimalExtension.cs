using System;

namespace Shared.Extensions
{
	public static class DecimalExtension
	{
		public static decimal Round(this decimal value, int decimalPlaces)
		{
			return Math.Round(value, decimalPlaces);
		}
	}
}