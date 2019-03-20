using System;
using System.Collections.Generic;

namespace NitroxModel.DataStructures.GameLogic
{
    [Serializable]
    public class InitialPdaData
    {
        public List<TechType> UnlockedTechTypes { get; set; } = new List<TechType>();
        public List<TechType> KnownTechTypes { get; set; } = new List<TechType>();
        public List<string> EncyclopediaEntries { get; set; } = new List<string>();
        public List<PDAEntry> PartiallyUnlockedTechTypes { get; set; } = new List<PDAEntry>();
        public List<PDALogEntry> PDALogEntries { get; set; } = new List<PDALogEntry>();

        public InitialPdaData()
        {
            // Constructor for serialization
        }

        public InitialPdaData(List<TechType> unlockedTechTypes, List<TechType> knownTechTypes, List<string> encyclopediaEntries, List<PDAEntry> partiallyUnlockedTechTypes, List<PDALogEntry> pdaLogEntries)
        {
            UnlockedTechTypes = unlockedTechTypes;
            KnownTechTypes = knownTechTypes;
            EncyclopediaEntries = encyclopediaEntries;
            PartiallyUnlockedTechTypes = partiallyUnlockedTechTypes;
            PDALogEntries = pdaLogEntries;
        }
    }
}
