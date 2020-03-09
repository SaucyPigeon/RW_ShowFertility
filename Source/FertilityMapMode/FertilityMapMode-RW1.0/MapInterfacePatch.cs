using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace FertilityMapMode
{
	[HarmonyPatch(typeof(MapInterface))]
	[HarmonyPatch("MapInterfaceOnGUI_BeforeMainTabs")]
	public class MapInterfacePatch
	{
		public static void Postfix()
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				FertilityDrawer.FertilityDrawerOnGUI();
			}
		}
	}
}
