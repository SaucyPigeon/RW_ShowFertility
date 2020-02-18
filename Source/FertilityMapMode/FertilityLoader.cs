using Harmony;
using RimWorld;
using System.Reflection;
using UnityEngine;
using Verse;

namespace FertilityMapMode
{
	[StaticConstructorOnStartup]
	internal class FertilityLoader
	{
		public static readonly Texture2D fertilityTexture;

		static FertilityLoader()
		{
			fertilityTexture = ContentFinder<Texture2D>.Get("Fertility", true);
			var harmony = HarmonyInstance.Create("uk.saucypigeon.rimworld.mod.showfertility");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}

	}
}
