using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace FitnessExtended
{
    public class StatWorker_BodyHeight : StatWorker
    {
        /// <summary>
        /// Return the height of the pawn
        /// </summary>
        /// <param name="req">Request</param>
        /// <param name="applyPostProcess">Apply post processing</param>
        /// <returns>Value</returns>
        public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
        {
            if (!req.HasThing || !(req.Thing is Pawn pawn))
                return 0f;
            FitnessComp comp = pawn.GetFitnessComp();
            if (comp == null)
                return 0f;
            return comp.Height;
        }

        public override string ValueToString(float val, bool finalized, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
        {           
            return $"{(int)(val * 100)} cm";
        }
    }
}

