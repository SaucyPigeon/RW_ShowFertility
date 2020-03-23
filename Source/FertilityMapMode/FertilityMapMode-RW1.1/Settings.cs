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
		public bool OverrideVanillaTexture = OverrideVanillaTexture_default;

		public float FertilitySampleRadius = FertilitySampleRadius_default;

		#region Constants

		private const bool OverrideVanillaTexture_default = true;

		private const float FertilitySampleRadius_default = 7.9f;

		private const float SampleRadiusMinimum = 5.0f;

		private const float SampleRadiusMaximum = 20.0f;

		#endregion

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


			string buffer = FertilitySampleRadius.ToString();
			list.TextFieldNumeric(ref FertilitySampleRadius, ref buffer, SampleRadiusMinimum, SampleRadiusMaximum);
			FertilitySampleRadius = list.Slider(FertilitySampleRadius, SampleRadiusMinimum, SampleRadiusMaximum);

			label = "ShowFertility.ResetToDefault";

			list.Gap(24);

			if (list.ButtonText(label.Translate()))
			{
				ResetToDefault();
			}

			list.End();
		}

		private void ResetToDefault()
		{
			OverrideVanillaTexture = OverrideVanillaTexture_default;
			FertilitySampleRadius = FertilitySampleRadius_default;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref OverrideVanillaTexture, "overrideVanillaTexture", OverrideVanillaTexture_default);
			Scribe_Values.Look(ref FertilitySampleRadius, "fertilitySampleRadius", FertilitySampleRadius_default);
		}
	}
}
