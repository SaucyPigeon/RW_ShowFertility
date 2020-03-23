using UnityEngine;
using System;
using System.Collections.Generic;

namespace FertilityMapMode.Coloring
{
	public interface IGradient<T>
	{
		SortedList<T, Color> Colors { get; }
		SortedList<T, float> Alphas { get; }

		void AddColor(T key, Color value);
		void AddAlpha(T key, float value);

		Color Evaluate(T value);
	}
}
