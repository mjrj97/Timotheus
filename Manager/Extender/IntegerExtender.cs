using System;

namespace Timotheus.Extender
{
	public static class IntegerExtender
	{
		public static int ToInt32Value(this object Value, int DefaultValue = 0)
		{
			if (Value == null || DBNull.Value == Value)
				return DefaultValue;

			try
			{
				if (Value is string)
				{
					return int.TryParse(Value.ToString(), out int Result) ? Result : DefaultValue;
				}
				else
				{
					return Convert.ToInt32(Value);
				}
			}
			catch { return DefaultValue; }
		}
	}
}
