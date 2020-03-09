using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace FertilityMapMode
{
	public static class FertilityDrawer
	{
		private static List<Thing> fertilityCountedThings = new List<Thing>();

		private static Gradient ColorGradient = GetColorGradient();

		private static Gradient GetColorGradient()
		{
			var gradient = new Gradient();

			var colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(Color.grey, 0.0f), // grey
				new GradientColorKey(Color.red, 0.2f), // red
				new GradientColorKey(new Color(1, 0.5f, 0), 0.7f), // orange
				new GradientColorKey(new Color(0.1f, 0.6f, 0.1f), 0.85f), // bland green
				new GradientColorKey(Color.green, 1.0f) // pure green
			};

			var alphaKeys = new GradientAlphaKey[]
			{
				new GradientAlphaKey(1, 0),
				new GradientAlphaKey(1, 1)
			};

			gradient.mode = GradientMode.Blend;
			gradient.SetKeys(colorKeys, alphaKeys);

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
			FertilityUtility.FillFertilityRelevantCells(UI.MouseCell(), Find.CurrentMap);
			for (int i = 0; i < FertilityUtility.fertilityRelevantCells.Count; i++)
			{
				IntVec3 intVec = FertilityUtility.fertilityRelevantCells[i];
				float num = FertilityUtility.CellFertility(intVec, Find.CurrentMap, FertilityDrawer.fertilityCountedThings);
				if (num != 0f)
				{
					Vector3 v = GenMapUI.LabelDrawPosFor(intVec);
					GenMapUI.DrawThingLabel(v, num.ToString("n1"), FertilityDrawer.FertilityColor(num, 1.4f));
				}
			}
			FertilityDrawer.fertilityCountedThings.Clear();
		}

		public static Color FertilityColor(float fertility, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, fertility);
			var color = ColorGradient.Evaluate(num);
			return color;
		}
	}
}
