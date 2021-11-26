using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace FitnessExtended
{
    public class FitnessComp : ThingComp
    {
        #region Variables        
        private float fitness = -1f;
        private float height = -1f;
        #endregion

        #region BaseValues
        private bool isHumanlike;
        private float weight = -1f;
        private float baseWeight = -1f;
        private float baseFitness = -1f;                
        #endregion

        public Pawn Pawn => (Pawn)parent;
        
        public float Weight
        {
            get
            {
                if (weight == -1)
                {
                    BodyGenUtility.Generate(this);
                }                
                return weight;
            }
            set
            {                
                weight = Mathf.Max(value, 0.01f);
                Notify_WeightChanged();
            }
        }

        public bool IsHumanlike
        {
            get => isHumanlike;
            set => isHumanlike = value && Pawn.RaceProps.Humanlike;
        }

        public float Fitness
        {
            get
            {
                if (fitness == -1)
                {
                    BodyGenUtility.Generate(this);
                }
                return weight;
            }
            set
            {
                fitness = Mathf.Max(value, 0.01f);
                Notify_FitnessChanged();
            }
        }

        public float BaseWeight
        {
            get
            {
                if (baseWeight == -1)
                {
                    BodyGenUtility.Generate(this);
                }
                return baseWeight;
            }
        }

        public float BaseFitness
        {
            get
            {
                if (baseFitness == -1)
                {
                    BodyGenUtility.Generate(this);
                }
                return baseFitness;
            }            
        }

        public float Height
        {
            get
            {
                if (height == -1)
                {
                    BodyGenUtility.Generate(this);                 
                }
                return height;
            }            
        }

        public FitnessComp()
        {
        }       

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref fitness, "fitness");
            Scribe_Values.Look(ref weight, "weight");
            Scribe_Values.Look(ref height, "height");
            Scribe_Values.Look(ref baseFitness, "baseFitness");
            Scribe_Values.Look(ref baseWeight, "baseWeight");
            Scribe_Values.Look(ref isHumanlike, "isHumanlike");
        }

        public override void CompTick()
        {            
            base.CompTick();
            //
            // Attemp to speedup stuff..
            Current.Set(this);
        }

        public override void CompTickRare()
        {
            base.CompTickRare();
            if (!isHumanlike)
            {
                return;
            }

        }

        public override void PostDeSpawn(Map map)
        {            
            base.PostDeSpawn(map);
            //
            // Remove from the cache..
            FitnessUtility.Dirty(this);
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            isHumanlike = Pawn.RaceProps.Humanlike && !Pawn.RaceProps.IsMechanoid;
            base.PostSpawnSetup(respawningAfterLoad);            
        }

        public void Notify_WeightChanged()
        {
        }

        public void Notify_FitnessChanged()
        {
        }

        public void SetBase(float height, float baseWeight, float baseFitness)
        {
            this.baseFitness = baseFitness;
            this.baseWeight = BaseWeight;
            this.height = height;
        }
    }
}

