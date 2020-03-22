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

		public void DoWindowContents(Rect canvas)
		{
			var list = new Listing_Standard();
			list.ColumnWidth = canvas.width;
			list.Begin(canvas);
			list.Gap(24);

			string label = "ShowFertility.OverrideVanillaTexture";

			list.CheckboxLabeled(label.Translate(), ref OverrideVanillaTexture, (label + "Tip").Translate());

			list.End();
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref OverrideVanillaTexture, "overrideVanillaTexture", defaultValue: false);
		}
	}
}
