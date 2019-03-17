using System.Runtime.Serialization;
using UnityEngine;

namespace NitroxModel.DataStructures.Surrogates
{
    public class ColorSurrogate
    {
        public float R { get; private set; }
        public float G { get; private set; }
        public float B { get; private set; }
        public float A { get; private set; }

        public static implicit operator ColorSurrogate(Color c) => c == null ? null : new ColorSurrogate
        {
            R = c.r,
            G = c.g,
            B = c.b,
            A = c.a,
        };

        public static implicit operator Color(ColorSurrogate c) => new Color(c.R, c.G, c.B, c.A);
    }
}
