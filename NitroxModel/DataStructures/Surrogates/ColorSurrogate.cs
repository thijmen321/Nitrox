using System.Runtime.Serialization;
using UnityEngine;

namespace NitroxModel.DataStructures.Surrogates
{
    public class ColorSurrogate
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public static implicit operator ColorSurrogate(Color c) => new ColorSurrogate
        {
            R = c.r,
            G = c.g,
            B = c.b,
            A = c.a,
        };

        public static implicit operator Color(ColorSurrogate c) => new Color(c.R, c.G, c.B, c.A);
    }
}
