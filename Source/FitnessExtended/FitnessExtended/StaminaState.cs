using System;
namespace FitnessExtended
{
    public enum StaminaState
    {
        /// <summary>
        /// Pawn is very energetic.
        /// </summary>
        Boost = 0,

        /// <summary>
        /// Pawns are feeling active and fresh.
        /// </summary>
        Normal = 1,

        /// <summary>
        ///  Pawn is slowed from heavy activity.
        /// </summary>
        Slowed = 2,

        /// <summary>
        /// Pawn is exhasted.
        /// </summary>
        Tiered = 3,
    }
}

