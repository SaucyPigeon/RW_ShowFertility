using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;

namespace FertilityMapMode
{
	[StaticConstructorOnStartup]
	public static class FertilityLoader
	{
		public static Texture2D FertilityButtonTexture;

		static FertilityLoader()
		{
			FertilityButtonTexture = ContentFinder<Texture2D>.Get("Fertility", true);
		}
	}
}
