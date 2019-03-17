using System.Runtime.Serialization;
using UnityEngine;

namespace NitroxModel.DataStructures.Surrogates
{
    public class Vector3Surrogate
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public static implicit operator Vector3Surrogate(Vector3 c) => c == null ? null : new Vector3Surrogate
        {
            X = c.x,
            Y = c.y,
            Z = c.z,
        };

        public static implicit operator Vector3(Vector3Surrogate c) => new Vector3(c.X, c.Y, c.Z);
    }
}
