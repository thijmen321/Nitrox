﻿using NitroxModel.DataStructures.GameLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using NitroxModel.DataStructures.Util;

namespace NitroxModel.Packets
{
    [Serializable]
    public class InitialPlayerSync : Packet
    {
        public List<EscapePodModel> EscapePodsData { get; } = new List<EscapePodModel>();
        public string AssignedEscapePodGuid;
        public List<EquippedItemData> EquippedItems { get; } = new List<EquippedItemData>();
        public List<BasePiece> BasePieces { get; } = new List<BasePiece>();
        public List<VehicleModel> Vehicles { get; } = new List<VehicleModel>();
        public List<ItemData> InventoryItems { get; } = new List<ItemData>();
        public string PlayerGuid { get; }
        public bool FirstTimeConnecting { get; }
        public InitialPdaData PDAData { get; }
        public Vector3 PlayerSpawnData { get; }
        public Optional<string> PlayerSubRootGuid { get; }
        public PlayerStatsData PlayerStatsData { get; }
        public List<InitialRemotePlayerData> RemotePlayerData { get; } = new List<InitialRemotePlayerData>();
        public List<Entity> GlobalRootEntities { get; } = new List<Entity>();
        public string GameMode { get; }
        public Perms Permissions { get; }

        public InitialPlayerSync()
        {

        }

        public InitialPlayerSync(string playerGuid, bool firstTimeConnecting, List<EscapePodModel> escapePodsData, string assignedEscapePodGuid, List<EquippedItemData> equipment, List<BasePiece> basePieces, List<VehicleModel> vehicles, List<ItemData> inventoryItems, InitialPdaData pdaData, Vector3 playerSpawnData, Optional<string> playerSubRootGuid, PlayerStatsData playerStatsData, List<InitialRemotePlayerData> remotePlayerData, List<Entity> globalRootEntities, string gameMode, Perms perms)
        {
            EscapePodsData = escapePodsData;
            AssignedEscapePodGuid = assignedEscapePodGuid;
            PlayerGuid = playerGuid;
            FirstTimeConnecting = firstTimeConnecting;
            EquippedItems = equipment;
            BasePieces = basePieces;
            Vehicles = vehicles;
            InventoryItems = inventoryItems;
            PDAData = pdaData;
            PlayerSpawnData = playerSpawnData;
            PlayerSubRootGuid = playerSubRootGuid;
            PlayerStatsData = playerStatsData;
            RemotePlayerData = remotePlayerData;
            GlobalRootEntities = globalRootEntities;
            GameMode = gameMode;
            Permissions = perms;
        }

        public override string ToString()
        {
            return "[InitialPlayerSync - EquippedItems: " + EquippedItems + " BasePieces: " + BasePieces + " Vehicles: " + Vehicles + " InventoryItems: " + InventoryItems + " PDAData: " + PDAData + "]";
        }
    }
}
