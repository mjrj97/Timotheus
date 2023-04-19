using Ical.Net.CalendarComponents;
using System;

namespace Timotheus.Schedule
{
	public static class CalendarEventExtensions
	{
		public static bool In(this CalendarEvent ev, Period period)
		{
			long a1, a2, b1, b2;

			a1 = ev.Start.Ticks;
			a2 = ev.End.Ticks;

			b1 = period.Start.Ticks;
			b2 = period.End.Ticks;

			return Math.Max(a1, b1) <= Math.Min(a2, b2);
		}
	}
}
