using HarmonyLib;
using RimWorld;
using System.Reflection;
using UnityEngine;
using Verse;

namespace FertilityMapMode
{
	[StaticConstructorOnStartup]
	public static class FertilityLoader
	{
		private const string Id = "uk.saucypigeon.rimworld.mod.showfertility";
		public static readonly Texture2D fertilityTexture;

		static FertilityLoader()
		{
			fertilityTexture = ContentFinder<Texture2D>.Get("Fertility", true);
			var harmony = new Harmony(Id);
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
