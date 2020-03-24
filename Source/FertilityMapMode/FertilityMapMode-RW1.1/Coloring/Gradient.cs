using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FertilityMapMode.Collections;

namespace FertilityMapMode.Coloring
{
	public class Gradient : IGradient<float>
	{
		public SortedList<float, Color> Colors { get; }

		public SortedList<float, float> Alphas { get; }

		private Color EvaluateColorRgb(float key)
		{
			if (Colors.ContainsKey(key))
			{
				return Colors[key];
			}
			var first = Colors.First();
			if (first.Key >= key)
			{
				return first.Value;
			}
			var last = Colors.Last();
			if (last.Key <= key)
			{
				return last.Value;
			}
			Colors.GetClosestValues(key, out var before, out var after);

			float t = Mathf.Lerp(before.Key, after.Key, key);
			return Color.Lerp(before.Value, after.Value, t);
		}

		private float EvaluateAlpha(float key)
		{
			if (Alphas.ContainsKey(key))
			{
				return Alphas[key];
			}
			var first = Alphas.First();
			if (first.Key >= key)
			{
				return first.Value;
			}
			var last = Alphas.Last();
			if (last.Key <= key)
			{
				return last.Value;
			}

			Alphas.GetClosestValues(key, out var before, out var after);
			float t = Mathf.Lerp(before.Key, after.Key, key);
			return Mathf.Lerp(before.Value, after.Value, t);
		}

		public Color Evaluate(float key)
		{
			Color color = EvaluateColorRgb(key);
			float alpha = EvaluateAlpha(key);

			return new Color(color.r, color.g, color.b, alpha);
		}

		public void AddColor(float key, Color value)
		{
			Colors.Add(key, value);
		}

		public void AddAlpha(float key, float value)
		{
			Alphas.Add(key, value);
		}

		public void AddKeys(IDictionary<float, Color> colors, IDictionary<float, float> alphas)
		{
			foreach (var color in colors)
			{
				AddColor(color.Key, color.Value);
			}
			foreach (var alpha in alphas)
			{
				AddAlpha(alpha.Key, alpha.Value);
			}
		}

		public Gradient() : this(new SortedList<float, Color>(), new SortedList<float, float>())
		{
		}

		public Gradient(SortedList<float, Color> colors, SortedList<float, float> alphas)
		{
			Colors = colors ?? throw new ArgumentNullException(nameof(colors));
			Alphas = alphas ?? throw new ArgumentNullException(nameof(alphas));
		}
	}
}
