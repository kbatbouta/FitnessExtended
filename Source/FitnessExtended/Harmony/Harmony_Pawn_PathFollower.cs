using System;
using HarmonyLib;
using UnityEngine.UIElements;
using Verse.AI;

namespace FitnessExtended.HarmonyFE
{
    public static class Harmony_Pawn_PathFollower
    {       
        [HarmonyPatch(typeof(Pawn_PathFollower), nameof(Pawn_PathFollower.CostToPayThisTick))]
        [HarmonyPriority(Priority.High)]
        public static class Harmony_Pawn_PathFollower_CostToPayThisTick
        {
            public static void Postfix(Pawn_PathFollower __instance, ref float __result)
            {
                if (Current.Pawn == __instance.pawn && Current.Fitness != null)                
                    Current.Fitness.stamina.AdjustPathingCost(ref __result);                                    
            }
        }        
    }
}

