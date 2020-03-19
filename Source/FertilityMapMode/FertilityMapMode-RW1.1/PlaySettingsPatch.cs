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
		private static FieldInfo GetShowFertilityOverlayTextureField()
		{
			const string textureFieldName = "Verse.TexButton";

			// Naughty stuff
			// Let's replace this with something cleverer
			try
			{
				var assembly = typeof(Verse.AbilityCompProperties).Assembly;
				var type = assembly.GetType(textureFieldName, throwOnError: true);
				var showFertilityOverlayField = AccessTools.Field(type, "ShowFertilityOverlay");
				return showFertilityOverlayField;
			}
			catch (TypeLoadException)
			{
				Log.Error($"ShowFertility failed to find {textureFieldName} in game assembly. Ensure that the class is spelt correctly and that the class has not been moved or renamed.");
				throw;
			}
			catch
			{
				throw;
			}
		}

		/*
		Replace vanilla texture with mod texture.
		Need to test overwriting vanilla textures without code.
		*/
		[HarmonyTranspiler]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var customTextureField = AccessTools.Field(typeof(FertilityLoader), nameof(FertilityLoader.fertilityTexture));
			var showFertilityOverlayField = GetShowFertilityOverlayTextureField();

			foreach (var instruction in instructions)
			{
				if (instruction.LoadsField(showFertilityOverlayField))
				{
					instruction.operand = customTextureField;
				}
				yield return instruction;
			}
		}
	}
}
