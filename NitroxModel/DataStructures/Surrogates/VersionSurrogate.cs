using System;

namespace NitroxModel.DataStructures.Surrogates
{
    public class VersionSurrogate
    {
        public string Version { get; private set; }

        public static implicit operator VersionSurrogate(Version v) => v == null ? null : new VersionSurrogate
        {
            Version = v.ToString()
        };

        public static implicit operator Version(VersionSurrogate v) => v == null ? null : new Version(v.Version);
    }
}
