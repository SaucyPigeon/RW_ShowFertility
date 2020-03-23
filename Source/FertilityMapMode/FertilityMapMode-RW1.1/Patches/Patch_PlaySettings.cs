using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;
using System.Reflection;
using System.Reflection.Emit;

namespace FertilityMapMode.Patches
{
	[HarmonyPatch(typeof(PlaySettings))]
	[HarmonyPatch(nameof(PlaySettings.DoPlaySettingsGlobalControls))]
	public static class Patch_PlaySettings
	{
		/*
		Replace vanilla texture with mod texture toggle via mod settings.
		*/
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return FertilityMod.Settings.OverrideVanillaTexture;
		}

		/*
		Replace vanilla texture with mod texture.
		*/
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var customTextureField = AccessTools.Field(typeof(FertilityLoader), nameof(FertilityLoader.FertilityButtonTexture));
			var showFertilityOverlayField = FertilityMod.ShowFertilityOverlayTextureField;

			return instructions.ReplaceFieldLoad(target: showFertilityOverlayField, value: customTextureField);
		}
	}
}
