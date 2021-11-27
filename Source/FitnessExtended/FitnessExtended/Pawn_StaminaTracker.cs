using System;
using Mono.Security.Protocol.Tls;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace FitnessExtended
{
    public class Pawn_StaminaTracker : IExposable
    {
        private static float[] statePathingFactor = new float[4]
        {
            1.4f,
            1.0f,
            0.9f,
            0.75f,            
        };

        #region Variables

        private StaminaState state = StaminaState.Normal;
        private float staminaInt = 1;        
        private int pathingTicks = 0;

        public FitnessComp comp;

        #endregion

        #region Properties

        /// <summary>
        /// The current stamina level.
        /// </summary>
        public float Stamina
        {
            get => staminaInt;
            set => staminaInt = Mathf.Clamp(value, 0, 0.5f * (comp.fitness + Mathf.Abs(80f - comp.weight) / 80f));
        }

        #endregion

        public Pawn_StaminaTracker()
        {
        }

        #region Methods

        public void ExposeData()
        {
            Scribe_Values.Look(ref staminaInt, "stamina");
            Scribe_Values.Look(ref pathingTicks, "pathingTicks");            
            Scribe_Values.Look(ref state, "state");
        }

        public void StaminaTickRarer()
        {
            float d = 0f;
            if (pathingTicks != 0)
            {
                if (comp.Pawn?.jobs?.curJob != null)
                {
                    LocomotionUrgency urgency = comp.Pawn.jobs.curJob.locomotionUrgency;
                    if (urgency == Verse.AI.LocomotionUrgency.Jog)
                        d = -0.016f;
                    else if (urgency == Verse.AI.LocomotionUrgency.Walk)
                        d = -0.013f;
                    else if (urgency == Verse.AI.LocomotionUrgency.Amble)
                        d = -0.0075f;
                    else
                        d = -0.020f;
                }
                else d = -0.020f;
                d *= comp.WorkoutFactor;
            }
            if (state == StaminaState.Normal || state == StaminaState.Boost)
            {
                d += 0.0115f;
            }
            else if (state == StaminaState.Slowed)
            {                
                d += 0.0085f;
            }
            else if(state == StaminaState.Tiered)
            {
                d += 0.0050f;
            }
            if (pathingTicks != 0)
                pathingTicks =  0;
            
            Stamina += d;

            if (staminaInt > 0.5f)
                return;

            // the idea here is to have a state machine.
            if (staminaInt >= 1.0f)
            {
                state = StaminaState.Boost;
            }
            else if (state == StaminaState.Normal)
            {
                if (staminaInt <= 0.05f)
                    state = StaminaState.Tiered;
                else if (staminaInt <= 0.25f)
                    state = StaminaState.Slowed;
            }
            else if (state == StaminaState.Slowed)
            {
                if (staminaInt > 0.30f)
                    state = StaminaState.Normal;
                else if (0.05f >= staminaInt)
                    state = StaminaState.Tiered;
            }
            else if (state == StaminaState.Tiered)
            {
                if (staminaInt > 0.4f)
                    state = StaminaState.Normal;
            }
            if (state != StaminaState.Normal && state != StaminaState.Boost)            
                FleckMaker.ThrowAirPuffUp(comp.Pawn.DrawPos + new Vector3(0, 1f, 0f), comp.Pawn.Map);            
        }
        
        /// <summary>
        /// Adjust the cost of entering the next cell.
        /// </summary>
        /// <param name="cost">Cell cost</param>
        public void AdjustPathingCost(ref float cost)
        {
            cost = cost * statePathingFactor[(int) state];
            pathingTicks++;            
        }        

        /// <summary>
        /// Prepare after loading or initialization.
        /// </summary>
        /// <param name="comp">Parent comp</param>
        public void Prepare(FitnessComp comp)
        {
            this.comp = comp;
            this.staminaInt = Mathf.Clamp(staminaInt, 0, this.comp.fitness);                       
        }        

        #endregion
    }
}

