using System;
using System.Xml;
using UnityEngine;
using Verse;

namespace FitnessExtended
{
    public class INormalRange
    {
        /// <summary>
        /// Mean value.
        /// </summary>
        public float u;
        /// <summary>
        /// Standard diviation.
        /// </summary>
        public float std;
        /// <summary>
        /// Min value.
        /// </summary>
        public float min;
        /// <summary>
        /// Max value.
        /// </summary>
        public float max;

        /// <summary>
        /// Return a random value.
        /// </summary>
        public float Value
        {
            get
            {
                return FE_Utility.RandomGaussian(min, max, u, std);
            }
        }

        /// <summary>
        /// Return a random normalized by the mean.
        /// </summary>
        public float ValueNormalied
        {
            get
            {
                return FE_Utility.RandomGaussian(min, max, u, std) / u;
            }
        }

        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            if (xmlRoot.ChildNodes.Count == 0)
            {
                Log.Error("Misconfigured INormalRange: " + xmlRoot.OuterXml);
                return;
            }
            bool minSet = false, maxSet = false, meanSet = false, stdSet = false;            
            foreach (XmlNode node in xmlRoot.ChildNodes)
            {
                if (node.Name == "u")
                {
                    meanSet = true;
                    u = float.Parse(node.InnerText.Trim());                    
                }
                else if (node.Name == "std")
                {
                    stdSet = true;
                    std = float.Parse(node.InnerText.Trim());
                }
                else if (node.Name == "min")
                {
                    minSet = true;
                    min = float.Parse(node.InnerText.Trim());
                }
                else if (node.Name == "max")
                {
                    maxSet = true;
                    max = float.Parse(node.InnerText.Trim());
                }
            }
            if(!minSet || !maxSet)
            {
                throw new Exception("Misconfigured INormalRange: Both min and max values need to be defined " + xmlRoot.OuterXml);
            }
            if (!meanSet)
            {
                u = (min + max) / 2.0f;
            }
            if (!stdSet)
            {
                 std = Mathf.Sqrt(Mathf.Abs((max - u) / 3.0f));
            }
        }    
    }
}

