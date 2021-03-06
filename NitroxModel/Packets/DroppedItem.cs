﻿using System;
using NitroxModel.DataStructures.Util;
using UnityEngine;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Helper;
using NitroxModel.DataStructures;

namespace NitroxModel.Packets
{
    [Serializable]
    public class DroppedItem : Packet
    {
        public string Guid { get; }
        public Optional<string> WaterParkGuid { get; }
        public TechType TechType { get; }
        public Vector3 ItemPosition { get; }
        public Quaternion ItemRotation { get; }
        public byte[] Bytes { get; } = new byte[0];

        public DroppedItem()
        {

        }

        public DroppedItem(string guid, Optional<string> waterParkGuid, TechType techType, Vector3 itemPosition, Quaternion itemRotation, byte[] bytes)
        {
            Guid = guid;
            WaterParkGuid = waterParkGuid;
            ItemPosition = itemPosition;
            ItemRotation = itemRotation;
            TechType = techType;
            Bytes = bytes;
        }

        public override Optional<AbsoluteEntityCell> GetDeferredCell()
        {
            // Items that are maintained in the global root should never have their pickup event
            // deferred because other players should be able to see their state change.  An example
            // of this is a player picking up a far away beacon.
            if (Map.Main.GlobalRootTechTypes.Contains(TechType))
            {
                return Optional<AbsoluteEntityCell>.Empty();
            }

            // All other pickup events should only happen when the cell is loaded.  
            return Optional<AbsoluteEntityCell>.Of(new AbsoluteEntityCell(ItemPosition, Map.Main.ItemLevelOfDetail));
        }

        public override string ToString()
        {
            return "[DroppedItem - guid: " + Guid + " WaterParkGuid: " + WaterParkGuid + " techType: " + TechType + " itemPosition: " + ItemPosition + "]";
        }
    }
}
