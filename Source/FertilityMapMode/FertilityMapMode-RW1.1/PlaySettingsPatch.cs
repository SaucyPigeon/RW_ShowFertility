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


namespace FertilityMapMode
{
	[HarmonyPatch(typeof(PlaySettings))]
	[HarmonyPatch(nameof(PlaySettings.DoPlaySettingsGlobalControls))]
	public static class PlaySettingsPatch
	{
		private static FieldInfo GetField(ModContentPack pack, string typeName, string fieldName)
		{
			if (pack == null)
			{
				throw new ArgumentNullException(nameof(pack));
			}
			if (typeName == null)
			{
				throw new ArgumentNullException(nameof(typeName));
			}
			if (fieldName == null)
			{
				throw new ArgumentNullException(nameof(fieldName));
			}

			foreach (var assembly in pack.assemblies.loadedAssemblies)
			{
				var type = assembly.GetType(typeName);
				if (type != null)
				{
					return AccessTools.Field(type, fieldName);
				}
			}
			return null;
		}

		private static FieldInfo GetShowFertilityOverlayTextureField()
		{
			var pack = LoadedModManager.RunningMods.FirstOrDefault(x => x.IsCoreMod);

			if (pack == null)
			{
				throw new InvalidOperationException($"Could not find Core in LoadedModManager.RunningMods. Ensure that Core is enabled.");
			}

			const string typeName = "Verse.TexButton";
			const string fieldName = "ShowFertilityOverlay";

			var field = GetField(pack, typeName, fieldName);

			if (field == null)
			{
				Log.Error($"ShowFertility was unable to find {typeName}::{fieldName} in Core. Please contact the mod author if you see this.");
				throw new MissingFieldException(typeName, fieldName);
			}
			return field;
		}

		/*
		Replace vanilla texture with mod texture.
		*/
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var customTextureField = AccessTools.Field(typeof(FertilityLoader), nameof(FertilityLoader.fertilityTexture));
			var showFertilityOverlayField = GetShowFertilityOverlayTextureField();

			return instructions.ReplaceFieldLoad(target: showFertilityOverlayField, value: customTextureField);
		}
	}
}
