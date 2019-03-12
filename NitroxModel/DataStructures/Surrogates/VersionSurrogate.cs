using System;

namespace NitroxModel.DataStructures.Surrogates
{
    public class VersionSurrogate
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }

        public static implicit operator VersionSurrogate(Version v) => new VersionSurrogate
        {
            Major = v.Major,
            Minor = v.Minor,
            Build = v.Build,
            Revision = v.Revision,
        };

        public static implicit operator Version(VersionSurrogate v) => new Version(v.Major, v.Minor, v.Build, v.Revision);
    }
}
