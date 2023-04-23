using System;
using System.ComponentModel;
using System.Linq;

namespace Timotheus.Extender
{
	public static class EnumExtender
	{
		public static string GetStringValue(this Enum Value)
		{
			var t = Value.GetType();
			var type = t.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (type != null && type.Any())
				return type.GetValue(0).ToString();
			return "";
		}
	}
}
