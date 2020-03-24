using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;
using FertilityMapMode.Coloring;

namespace FertilityMapMode
{
	public static class FertilityDrawer
	{
		private static List<Thing> fertilityCountedThings = new List<Thing>();

		private static Coloring.Gradient ColorGradient = GetColorGradient();

		private static Coloring.Gradient GetColorGradient()
		{
			var gradient = new Coloring.Gradient();

			var colorKeys = new SortedDictionary<float, Color>
			{
				{ 0.0f, Color.grey },

				// orange/brown
				{ 0.7f, new Color(0.6f, 0.5f, 0.0f) },

				// bland green
				{ 1.0f, new Color(0.1f, 0.6f, 0.1f) },

				{ 1.4f, Color.green },

				// For modded fertility levels (highest vanilla=1.4)
				{ 2.5f, Color.cyan }
			};

			var alphaKeys = new SortedDictionary<float, float>
			{
				{ 0.0f, 1.0f },

				{ 1.0f, 1.0f }
			};

			gradient.AddKeys(colorKeys, alphaKeys);

			return gradient;
		}

		public static void FertilityDrawerOnGUI()
		{
			if (Event.current.type != EventType.Repaint || !FertilityDrawer.ShouldShow())
			{
				return;
			}
			FertilityDrawer.DrawFertilityAroundMouse();
		}

		private static bool ShouldShow()
		{
			return Find.PlaySettings.showFertilityOverlay && !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap);
		}

		private static void DrawFertilityAroundMouse()
		{
			if (!Find.PlaySettings.showFertilityOverlay)
			{
				return;
			}
			// Prevent overlap with beauty display
			if (Find.PlaySettings.showBeauty)
			{
				return;
			}
			FertilityUtility.FillFertilityRelevantCells(UI.MouseCell(), Find.CurrentMap);
			foreach (var cell in FertilityUtility.fertilityRelevantCells)
			{
				float num = FertilityUtility.CellFertility(cell, Find.CurrentMap, FertilityDrawer.fertilityCountedThings);
				if (num != 0f)
				{
					Vector3 v = GenMapUI.LabelDrawPosFor(cell);
					GenMapUI.DrawThingLabel(v, num.ToString("n1"), FertilityDrawer.FertilityColor(num, 1.4f));
				}
			}
			FertilityDrawer.fertilityCountedThings.Clear();
		}

		public static Color FertilityColor(float fertility, float scale)
		{		
			var color = ColorGradient.Evaluate(fertility);
			return color;
		}
	}
}
