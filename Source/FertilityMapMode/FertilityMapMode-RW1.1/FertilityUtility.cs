﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace FertilityMapMode
{
	[StaticConstructorOnStartup]
	public static class FertilityUtility
	{
		public static List<IntVec3> fertilityRelevantCells = new List<IntVec3>();

		private static List<Room> visibleRooms = new List<Room>();

		public static int SampleNumCells_Fertility
		{
			get
			{
				return GenRadial.NumCellsInRadius(FertilityMod.Settings.FertilitySampleRadius); 
			}
		}
		
		private static List<Thing> tempCountedThings = new List<Thing>();

		public static void FillFertilityRelevantCells(IntVec3 root, Map map)
		{
			FertilityUtility.fertilityRelevantCells.Clear();
			Room room = root.GetRoom(map, RegionType.Set_Passable);
			if (room == null)
			{
				return;
			}
			FertilityUtility.visibleRooms.Clear();
			FertilityUtility.visibleRooms.Add(room);
			if (room.Regions.Count == 1 && room.Regions[0].type == RegionType.Portal)
			{
				foreach (Region current in room.Regions[0].Neighbors)
				{
					if (!FertilityUtility.visibleRooms.Contains(current.Room))
					{
						FertilityUtility.visibleRooms.Add(current.Room);
					}
				}
			}
			for (int i = 0; i < FertilityUtility.SampleNumCells_Fertility; i++)
			{
				IntVec3 intVec = root + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && !intVec.Fogged(map))
				{
					Room room2 = intVec.GetRoom(map, RegionType.Set_Passable);
					if (!FertilityUtility.visibleRooms.Contains(room2))
					{
						bool flag = false;
						for (int j = 0; j < 8; j++)
						{
							IntVec3 loc = intVec + GenAdj.AdjacentCells[j];
							if (FertilityUtility.visibleRooms.Contains(loc.GetRoom(map, RegionType.Set_Passable)))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							goto IL_181;
						}
					}
					FertilityUtility.fertilityRelevantCells.Add(intVec);
				}
			IL_181:;
			}
			FertilityUtility.visibleRooms.Clear();
		}

		public static float CellFertility(IntVec3 c, Map map, List<Thing> countedThings = null)
		{
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (FertilityUtility.FertilityRelevant(thing.def.category))
				{
					if (countedThings != null)
					{
						if (countedThings.Contains(thing))
						{
							break;
						}
						countedThings.Add(thing);
					}
					SlotGroup slotGroup = thing.GetSlotGroup();
					if (slotGroup == null)
					{
						float num3 = map.fertilityGrid.FertilityAt(thing.Position);
						if (thing.def.Fillage == FillCategory.Full)
						{
							flag = true;
							num2 += num3;
						}
						else
						{
							num += num3;
						}
					}
				}
			}
			if (flag)
			{
				return num2;
			}
			return num + map.fertilityGrid.FertilityAt(c);
		}

		public static bool FertilityRelevant(ThingCategory cat)
		{
			return cat == ThingCategory.None;
		}
	}
}
