using System;
using Verse;

namespace FitnessExtended
{
    public class BodyGenConfigDef : Def
    {        
        public INormalRange weightNormal;

        public INormalRange fitnessNormal;

        public INormalRange heightNormal;        

        public override void PostLoad()
        {
            base.PostLoad();
            if(weightNormal.max == weightNormal.min)
            {
                Log.Error($"Misconfigured BodyGenConfigDef {defName}:weight");
            }
            if (fitnessNormal.max == fitnessNormal.min)
            {
                Log.Error($"Misconfigured BodyGenConfigDef {defName}:fitness");
            }
            if (heightNormal.max == heightNormal.min)
            {
                Log.Error($"Misconfigured BodyGenConfigDef {defName}:heightNormal");
            }
        }
    }
}

