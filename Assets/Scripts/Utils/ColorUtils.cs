using System;
using UnityEngine;

namespace TowerBuilder.Utils
{
    public static class ColorUtils
    {
        public static Color ColorFromHex(string hexString)
        {
            string slicedString = hexString;

            if (hexString[0] == '#')
            {
                slicedString = hexString.Substring(1);
            }

            if (slicedString.Length != 6)
            {
                throw new Exception("Hex string must be 6 characters long");
            }

            string r = slicedString.Substring(0, 2);
            string g = slicedString.Substring(2, 2);
            string b = slicedString.Substring(4, 2);

            int rAsInt = Convert.ToInt32(r, 16);
            int gAsInt = Convert.ToInt32(g, 16);
            int bAsInt = Convert.ToInt32(b, 16);

            float rAsFloat = (float)rAsInt / 255f;
            float gAsFloat = (float)gAsInt / 255f;
            float bAsFloat = (float)bAsInt / 255f;

            Color color = new Color(rAsFloat, gAsFloat, bAsFloat);

            return color;
        }
    }
}