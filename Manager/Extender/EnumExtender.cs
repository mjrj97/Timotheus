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
			var fi = t.GetField(Value.ToString());
			DescriptionAttribute[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
			if (attrs != null && attrs.Length > 0)
				return attrs[0].Description;
			return "";
		}
	}
}
