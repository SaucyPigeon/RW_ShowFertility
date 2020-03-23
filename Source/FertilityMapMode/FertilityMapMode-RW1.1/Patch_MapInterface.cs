using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using System.Reflection.Emit;

namespace FertilityMapMode
{
	[HarmonyPatch(typeof(MapInterface))]
	public class Patch_MapInterface
	{
		/*
		Add fertility drawer call right after beauty drawer call. Helps with
		proper UI ordering of fertility display numbers, so they don't overlap
		with map interface.
		*/
		[HarmonyPatch("MapInterfaceOnGUI_BeforeMainTabs")]
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> MapInterfaceOnGUI_BeforeMainTabs_Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var beautyDrawerOnGUIMethod = AccessTools.Method(typeof(BeautyDrawer), nameof(BeautyDrawer.BeautyDrawerOnGUI));
			var fertilityDrawerOnGUIMethod = AccessTools.Method(typeof(FertilityDrawer), nameof(FertilityDrawer.FertilityDrawerOnGUI));

			return instructions.AddCallAfter(target: beautyDrawerOnGUIMethod, value: fertilityDrawerOnGUIMethod);
		}
		
		/*
		Prevents fertility grid update method from being called. Not essential,
		but could potentially improve performance and unexpected errors.
		*/
		[HarmonyPatch("MapInterfaceUpdate")]
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> MapInterfaceUpdate_Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var fertilityGridUpdateMethod = AccessTools.Method(typeof(FertilityGrid), nameof(FertilityGrid.FertilityGridUpdate));

			var fertilityGridField = AccessTools.Field(typeof(Map), nameof(Map.fertilityGrid));
			var currentMapGetter = AccessTools.PropertyGetter(typeof(Find), nameof(Find.CurrentMap));

			return instructions
				.SkipMethod(fertilityGridUpdateMethod)
				.SkipFieldAndMethod(fertilityGridField, currentMapGetter, count: 1);
		}
	}
}
