using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;

namespace FertilityMapMode
{
	[HarmonyPatch(typeof(FertilityGrid))]
	[HarmonyPatch(nameof(FertilityGrid.FertilityGridUpdate))]
	public static class FertilityGridPatch
	{
		[HarmonyPrefix]
		public static void Prefix()
		{
			throw new NotSupportedException("FertilityGrid.FertilityGridUpdate is no longer used since the mod ShowFertility uses a different method.");
		}
	}
}
