using System;
using Verse;

namespace FitnessExtended
{
    public static class Current
    {
        private static Pawn pawn;
        private static FitnessComp fitness;

        public static Pawn Pawn
        {
            get => pawn;
        }

        public static FitnessComp Fitness
        {
            get => fitness;
        }

        public static void Reset()
        {
            Current.pawn = null;
            Current.fitness = null;
        }

        public static void Set(Pawn pawn)
        {
            Current.Reset();
            Current.pawn = pawn;            
        }

        public static void Set(FitnessComp fitnessComp)
        {
            Current.Reset();
            Current.pawn = fitnessComp.Pawn;
            Current.fitness = fitnessComp;
        }
    }
}

