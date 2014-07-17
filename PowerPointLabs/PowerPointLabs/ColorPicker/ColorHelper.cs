﻿using System.Collections.Generic;
using System.Drawing;

namespace PowerPointLabs.ColorPicker
{
    class ColorHelper
    {
        public static int ReverseRGBToArgb(int x)
        {
            int R = 0xff & x;
            int G = (0xff00 & x) >> 8;
            int B = (0xff0000 & x) >> 16;
            return (int)(0xff << 24 | R << 16 | G << 8 | B);
        }

        public static Color GetColorShiftedByAngle(HSLColor originalColor, float angle)
        {
            if (angle < 0)
            {
                while (angle < 0)
                {
                    angle += 360.0f;
                }
            }

            var baseAngle = (float) originalColor.Hue;
            var finalAngle = baseAngle + (angle % 360);
            
            if (finalAngle > 360.0f)
            {
                finalAngle -= 360.0f;
            }
            Color finalColor = new HSLColor(finalAngle, originalColor.Saturation, originalColor.Luminosity);

            return Color.FromArgb(255,
                    finalColor.R,
                    finalColor.G,
                    finalColor.B);
        }

        public static Color GetComplementaryColor(HSLColor originalColor)
        {
            return GetColorShiftedByAngle(originalColor, 180.0f);
        }

        public static List<Color> GetAnalogousColorsForColor(HSLColor originalColor)
        {
            var analogousColors = new List<Color>
            {
                GetColorShiftedByAngle(originalColor, -30.0f),
                GetColorShiftedByAngle(originalColor, 30.0f)
            };

            return analogousColors;
        }

        public static List<Color> GetTriadicColorsForColor(HSLColor originalColor)
        {
            var triadicColors = new List<Color>
            {
                GetColorShiftedByAngle(originalColor, -120.0f),
                GetColorShiftedByAngle(originalColor, 120.0f)
            };

            return triadicColors;
        }

        public static List<Color> GetTetradicColorsForColor(HSLColor originalColor)
        {
            var tetradicColors = new List<Color>
            {
                GetColorShiftedByAngle(originalColor, -90.0f),
                GetColorShiftedByAngle(originalColor, 90.0f),
                GetComplementaryColor(originalColor)
            };

            return tetradicColors;
        }

        public static List<Color> GetSplitComplementaryColorsForColor(HSLColor originalColor)
        {
            var splitComplementaryColors = new List<Color>
            {
                GetColorShiftedByAngle(originalColor, 150.0f),
                GetColorShiftedByAngle(originalColor, 210.0f)
            };

            return splitComplementaryColors;
        }
    }
}
