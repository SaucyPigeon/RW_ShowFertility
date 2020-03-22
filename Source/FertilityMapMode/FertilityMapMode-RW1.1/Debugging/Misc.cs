using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using System.Globalization;

namespace FertilityMapMode.Debugging
{
	// Miscellaneous class for testing, etc.
	internal static class Misc
	{
		internal static string ToCssColorRgba(float r, float g, float b, float a)
		{
			return $"rgba({r*255}, {g*255}, {b*255}, {a*255})";
		}

		internal static string GetCssGradient(Gradient gradient)
		{
			if (gradient == null)
			{
				throw new ArgumentNullException(nameof(gradient));
			}
			var sb = new StringBuilder();

			sb.Append("#grad {\n");
			sb.Append("\tbackground-image: linear-gradient(to right");

			foreach (var colorKey in gradient.colorKeys)
			{
				var a = gradient.Evaluate(colorKey.time).a;
				var color = colorKey.color;
				var cssRgba = ToCssColorRgba(color.r, color.g, color.b, a);
				sb.Append(", ");
				sb.Append(cssRgba);
				sb.Append(" ");
				sb.Append(colorKey.time.ToString("0%"));
			}
			sb.Append(");\n");
			sb.Append("}\n");

			return sb.ToString();
		}

		internal static void PrintCssGradient(Gradient gradient)
		{

			Log.Message(GetCssGradient(gradient));
		}
	}
}
