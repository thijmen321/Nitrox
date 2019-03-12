using System;

namespace NitroxModel.Helper
{
    [Flags]
    public enum TypeModelMembers
    {
        None = 0,
        PublicFields = 1,
        PrivateFields = 2,
        PublicProperties = 4,
        PrivateProperties = 8,
    }
}
