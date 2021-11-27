using System;
using Verse;

namespace FitnessExtended
{
    public static class BodyGenUtility
    {
        public static void Generate(FitnessComp comp)
        {
            Pawn pawn = comp.Pawn;
            BodyGenConfigDef genOptions = GetBodyGenConfigFor(pawn);
            float height, fitness, weight, baseFitness, baseWeight;
            bool isAnimal = pawn.RaceProps.Animal && !pawn.RaceProps.Humanlike;
            if (genOptions != null && !isAnimal)
            {
                height = genOptions.heightNormal.Value;
                weight = genOptions.weightNormal.Value;
                baseWeight = genOptions.weightNormal.Value;
                fitness = genOptions.fitnessNormal.Value;                              
                baseFitness = genOptions.fitnessNormal.Value;
            }
            else
            {
                isAnimal = true;
                fitness = genOptions.fitnessNormal.Value;
                height = weight = baseFitness = baseWeight  = - 1f;                       
            }
            comp.bodyConfigDef = genOptions;
            comp.fitness = fitness;            
            comp.weight = weight;
            comp.SetBase(height, baseWeight, baseFitness);
        }

        private static BodyGenConfigDef GetBodyGenConfigFor(Pawn pawn)
        {
            if (pawn.RaceProps.Humanlike)
            {
                if (pawn.kindDef?.HasModExtension<BodyGenExtension>() ?? false)
                {
                    return pawn.kindDef.GetModExtension<BodyGenExtension>().genConfig;
                }
                else if (pawn.kindDef != null)
                {
                    if (pawn.kindDef.isFighter) return FE_BodyGenConfigDefOf.BodyGenFighters;
                    else return FE_BodyGenConfigDefOf.BodyGenCivil;
                }
                else
                {
                    return Rand.Chance(0.5f) ? FE_BodyGenConfigDefOf.BodyGenCivil : FE_BodyGenConfigDefOf.BodyGenFighters;
                }
            }
            return null;
        }
    }
}

