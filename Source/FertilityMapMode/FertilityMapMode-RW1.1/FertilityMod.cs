using HarmonyLib;
using RimWorld;
using System.Reflection;
using UnityEngine;
using Verse;
using System;

namespace FertilityMapMode
{
	public class FertilityMod : Mod
	{
		private const string Id = "uk.saucypigeon.rimworld.mod.showfertility";
		
		public static FieldInfo ShowFertilityOverlayTextureField
		{
			get;
			private set;
		}

		private static void LoadShowFertilityOverlayTextureField()
		{
			const string namespaceName = "Verse";
			const string typeName = "TexButton";
			const string fieldName = "ShowFertilityOverlay";

			try
			{
				var type = GenTypes.GetTypeInAnyAssembly(typeName, namespaceName);
				var field = type.GetField(fieldName, AccessTools.all);
				ShowFertilityOverlayTextureField = field;
			}
			catch (ArgumentNullException)
			{
				Log.Error($"{Id} was unable to find {namespaceName}.{typeName}::{fieldName}." +
					$"Please contact the mod author if you see this.");
				throw;
			}
		}

		public static Settings Settings;

		public override string SettingsCategory()
		{
			return "ShowFertility.ModName".Translate();
		}

		public override void DoSettingsWindowContents(Rect inRect)
		{
			Settings.DoWindowContents(inRect);
		}

		public FertilityMod(ModContentPack content) : base(content)
		{
			LoadShowFertilityOverlayTextureField();
			Settings = GetSettings<Settings>();

			var harmony = new Harmony(Id);
			harmony.PatchAll(Assembly.GetExecutingAssembly());

		}
	}
}
