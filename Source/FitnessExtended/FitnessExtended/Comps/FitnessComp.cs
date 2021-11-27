using RimWorld;
using Verse;

namespace FitnessExtended
{
    public class FitnessComp : ThingComp
    {
        #region BaseValues

        private float height;
        private float baseWeight;
        private float baseFitness;

        #endregion

        #region Variables

        private bool bodyInitialized;
        
        public float fitness;
        public float weight;
        public Pawn_StaminaTracker stamina;
        public BodyGenConfigDef bodyConfigDef;

        #endregion

        #region Properties

        /// <summary>
        /// Parent Pawn.
        /// </summary>
        public Pawn Pawn
        {
            get => (Pawn) parent;
        }        
      
        /// <summary>
        /// Pawn height in meters. Cannot be changed.
        /// </summary>
        public float Height
        {
            get => height;
        }

        /// <summary>
        /// Pawn target weight in kilograms. Pawn weight will slowly return to this value.
        /// </summary>
        public float BaseWeight
        {
            get => baseWeight;
        }

        /// <summary>
        /// Pawn target fitness. Pawn fitness will slowly return to this value.
        /// </summary>
        public float BaseFitness
        {
            get => baseFitness;         
        }

        /// <summary>
        /// A multiplier that affect the amount of energy cosumed during an activity
        /// </summary>
        public float WorkoutFactor
        {
            get => 1f;
        }

        #endregion

        public FitnessComp()
        {
            stamina = new Pawn_StaminaTracker();
        }       

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref fitness, "fitness");
            Scribe_Values.Look(ref weight, "weight");
            Scribe_Values.Look(ref height, "height");
            Scribe_Values.Look(ref baseFitness, "baseFitness");
            Scribe_Values.Look(ref baseWeight, "baseWeight");
            Scribe_Values.Look(ref bodyInitialized, "bodyInitialized");

            Scribe_Defs.Look(ref bodyConfigDef, "genConfig");
            Scribe_Deep.Look(ref stamina, "staminaTracker");
            InitializeTrackers();
        }

        #region Methods

        /// <summary>
        /// The comp ticker. Called every tick.
        /// </summary>
        public override void CompTick()
        {            
            base.CompTick();
            //
            // Attemp to speedup stuff..            
            Current.Set(this);
            if (!parent.Spawned)            
                return;
            if ((GenTicks.TicksGame + parent.thingIDNumber + 13) % 45 == 0)            
                stamina.StaminaTickRarer();            
        }

        /// <summary>
        /// The comp ticker. Called every 2000 ticks.
        /// </summary>
        public override void CompTickLong()
        {
            base.CompTickLong();            
            if (!parent.Spawned)
                return;
            //
            // Try to fix the pawn body
            RefreshBody();
        }

        /// <summary>
        /// Called after the pawn is despawned/removed from a map.
        /// </summary>
        /// <param name="map">The old map</param>
        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            //
            // Remove from the cache..
            FitnessUtility.Dirty(this);
        }

        /// <summary>
        /// Called after the pawn is spawned in a map.
        /// </summary>
        /// <param name="respawningAfterLoad">respawningAfterLoad</param>
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {            
            base.PostSpawnSetup(respawningAfterLoad);
            //
            // intialize this incase it's not already intialized
            Generate();            
            InitializeTrackers(); // make sure all trackers are working            
        }

        /// <summary>
        /// Generate the body config
        /// </summary>
        public void Generate()
        {
            if (!bodyInitialized)
            {
                BodyGenUtility.Generate(this);
                bodyInitialized = true;
            }            
        }

        /// <summary>
        /// Referesh body texture. Will set body texture with the value appropite for the weight.
        /// </summary>
        public void RefreshBody()
        {
            if (Pawn.story != null)
            {
                BodyTypeDef bodyType;
                float bmi = weight / height;
                if (bmi >= 1.6)
                {
                    bodyType = BodyTypeDefOf.Fat;
                }
                else if (bmi >= 1.2f)
                {
                    if (fitness > 1.0f)
                        bodyType = BodyTypeDefOf.Hulk;
                    else
                        bodyType = BodyTypeDefOf.Fat;
                }
                else if (bmi > 0.8f)
                {
                    if (Pawn.gender == Gender.Male)
                        bodyType = BodyTypeDefOf.Male;
                    else
                        bodyType = BodyTypeDefOf.Female;
                }
                else
                {
                    bodyType = BodyTypeDefOf.Thin;
                }
                if (bodyType != Pawn.story.bodyType)
                    Pawn.SetBodyType(bodyType);
            }
        }

        public void SetBase(float height, float baseWeight, float baseFitness)
        {
            if(bodyInitialized)
            {
                Log.Warning($"FE: Pawn {Pawn} base being <color=red>set again!</color>\n" +
                    $"height\f({this.height})->({height})\n" +
                    $"baseWeight\f({this.baseWeight})->({baseWeight})\n" +
                    $"baseFitness\f({this.baseFitness})->({baseFitness})\n");
            }            
            this.baseFitness = baseFitness;
            this.baseWeight = baseWeight;
            this.height = height;
        }       

        private void InitializeTrackers()
        {            
            if (stamina == null)
                stamina = new Pawn_StaminaTracker();
            stamina.Prepare(this);
        }

        #endregion
    }
}

