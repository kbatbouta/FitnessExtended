using System;
using System.Collections.Generic;
using Verse;

namespace FitnessExtended
{
    public static class FitnessUtility
    {
        private static Dictionary<int, FitnessComp> _fitnessComp = new Dictionary<int, FitnessComp>(10000);

        public static FitnessComp GetFitnessComp(this Pawn pawn)
        {            
            if(Current.Pawn == pawn && Current.Fitness != null)
            {
                return Current.Fitness;
            }
            if(pawn == null)
            {
                return null;
            }
            if (_fitnessComp.TryGetValue(pawn.thingIDNumber, out FitnessComp comp))
            {
                return comp;
            }                        
            Current.Set(comp = pawn.GetComp<FitnessComp>());

            if (comp != null)
            {
                return _fitnessComp[pawn.thingIDNumber] = comp;
            }
            return null;
        }        

        public static void Dirty(FitnessComp comp)
        {
            if (_fitnessComp.ContainsKey(comp.Pawn.thingIDNumber))
            {
                _fitnessComp.Remove(comp.Pawn.thingIDNumber);
            }
        }       
    }
}

