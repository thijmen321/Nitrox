using System.Runtime.Serialization;
using UnityEngine;

namespace NitroxModel.DataStructures.Surrogates
{
    public class QuaternionSurrogate
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public static implicit operator QuaternionSurrogate(Quaternion c) => new QuaternionSurrogate
        {
            X = c.x,
            Y = c.y,
            Z = c.z,
            W = c.w,
        };

        public static implicit operator Quaternion(QuaternionSurrogate c) => new Quaternion(c.X, c.Y, c.Z, c.W);
    }
}
