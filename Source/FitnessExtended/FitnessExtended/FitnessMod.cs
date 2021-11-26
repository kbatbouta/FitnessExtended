using System;
using HarmonyLib;
using Verse;

namespace FitnessExtended
{
    public class FitnessMod : Mod
    {
        public FitnessMod(ModContentPack content) : base(content)
        {
            Finder.Mod = this;
            Finder.ModContentPack = content;
            if (Finder.Harmony == null)
            {
                Finder.Harmony = new Harmony(Finder.PackageId);
                Finder.Harmony.PatchAll(typeof(FitnessMod).Assembly);
            }
        }
    }
}

