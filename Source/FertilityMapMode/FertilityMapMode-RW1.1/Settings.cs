using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using UnityEngine;

namespace FertilityMapMode
{
	public class Settings : ModSettings
	{
		public bool OverrideVanillaTexture = true;

		public float FertilitySampleRadius = 7.9f;

		// Uselessly small
		private const float SampleRadiusMinimum = 1.0f;

		// Laggingly big
		private const float SampleRadiusMaximum = 100.0f;

		public void DoWindowContents(Rect canvas)
		{
			var list = new Listing_Standard();
			list.ColumnWidth = canvas.width;
			list.Begin(canvas);

			list.Gap(24);
			string label = "ShowFertility.OverrideVanillaTexture";
			list.CheckboxLabeled(label.Translate(), ref OverrideVanillaTexture, (label + "Tip").Translate());

			list.Gap(24);
			// TextFieldNumeric is very dodgy to use with mins and maxes (e.g.
			// nearly impossible to enter specific numbers due to autofill).
			// Also lacks tooltips.
			label = "ShowFertility.FertilitySampleRadius";
			list.Label(label.Translate(), tooltip: (label + "Tip").Translate());
			FertilitySampleRadius = list.Slider(FertilitySampleRadius, SampleRadiusMinimum, SampleRadiusMaximum);

			list.End();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref OverrideVanillaTexture, "overrideVanillaTexture", defaultValue: false);
			Scribe_Values.Look(ref FertilitySampleRadius, "fertilitySampleRadius", defaultValue: 7.9f);
		}
	}
}
