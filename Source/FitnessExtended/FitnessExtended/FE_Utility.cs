using RimWorld;
using UnityEngine;
using Verse;

namespace FitnessExtended
{
    public static class FE_Utility
    {


        #region Math

        /// <summary>
        /// Return a normal random distrubuted value value within a given range.
        /// </summary>
        /// <param name="minValue">Min</param>
        /// <param name="maxValue">Max</param>
        /// <returns>Random value</returns>
        public static float RandomGaussian(float minValue, float maxValue)
        {
            float u, v, S;
            do
            {
                u = 2.0f * UnityEngine.Random.value - 1.0f;
                v = 2.0f * UnityEngine.Random.value - 1.0f;
                S = u * u + v * v;
            }
            while (S >= 1.0f);
            float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
            float mean = (minValue + maxValue) / 2.0f;
            float sigma = (maxValue - mean) / 3.0f;
            return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
        }

        /// <summary>
        /// Return a normal random distrubuted value value within a given range.
        /// </summary>
        /// <param name="minValue">Min</param>
        /// <param name="maxValue">Max</param>
        /// <param name="mean">Mean</param>
        /// <param name="std">Std</param>
        /// <returns>Random value</returns>
        public static float RandomGaussian(float minValue, float maxValue, float mean, float std)
        {
            float u, v, S;
            do
            {
                u = 2.0f * UnityEngine.Random.value - 1.0f;
                v = 2.0f * UnityEngine.Random.value - 1.0f;
                S = u * u + v * v;
            }
            while (S >= 1.0f);
            float istd = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
            float imean = 0.2f * (minValue + maxValue ) / 2.0f + mean * 0.8f;            
            float sigma = Mathf.Min(std * std, (maxValue - mean) / 3.0f);
            return Mathf.Clamp(istd * sigma + imean, minValue, maxValue);
        }

        #endregion

        #region Rendering

        public static void SetBodyType(this Pawn pawn, BodyTypeDef bodyTypeDef)
        {
            pawn.story.bodyType = bodyTypeDef;
            pawn.Drawer.renderer.graphics.SetAllGraphicsDirty();
            pawn.Drawer.renderer.graphics.ResolveAllGraphics();
            PortraitsCache.SetDirty(pawn);
        }

        #endregion
    }
}

