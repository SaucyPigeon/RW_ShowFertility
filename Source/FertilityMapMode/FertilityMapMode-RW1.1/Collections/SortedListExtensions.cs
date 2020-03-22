using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityMapMode.Collections
{
	public static class SortedListExtensions
	{
		public static void GetClosestValues<TKey, TValue>(this SortedList<TKey, TValue> list, TKey value, out KeyValuePair<TKey, TValue> before, out KeyValuePair<TKey, TValue> after)
		{
			if (list == null)
			{
				throw new ArgumentNullException(nameof(list));
			}
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			var keys = list.Keys.ToList();

			var index = keys.BinarySearch(value);

			Verse.Log.Warning($"Binary search result: {index}");
			Verse.Log.Warning($"~index: {~index}");

			before = list.ElementAt((~index) - 1);
			after = list.ElementAt(~index);
		}
	}
}
