using Harmony;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace FertilityMapMode
{
	[HarmonyPatch(typeof(PlaySettings))]
	[HarmonyPatch("DoPlaySettingsGlobalControls")]
	public class PlaySettingsPatch
	{
		public static bool showFertilityOverlay;

		public static void Postfix(WidgetRow row, bool worldView)
		{
			if (!worldView)
			{
				row.ToggleableIcon(ref showFertilityOverlay, FertilityLoader.fertilityTexture, "ShowFertilityToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle, null);
			}
		}
	}
}
