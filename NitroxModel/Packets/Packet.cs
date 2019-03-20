using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NitroxModel.DataStructures.Surrogates;
using NitroxModel.Logger;
using NitroxModel.DataStructures.Util;
using NitroxModel.DataStructures.GameLogic;
using LZ4;
using NitroxModel.Networking;
using System.Collections.Generic;
using ProtoBufNet.Meta;
using NitroxModel.Helper;
using UnityEngine;
using ProtoBufNet;

namespace NitroxModel.Packets
{
    [Serializable]
    public abstract class Packet
    {
        private static readonly RuntimeTypeModel serializer;
        
        static Packet()
        {
            serializer = new TypeModelBuilder(TypeModelMembers.PublicProperties | TypeModelMembers.PublicFields)
                .AddTypesWithAttribute<SerializableAttribute>(typeof(Packet).Assembly)
                .AddSurrogate<ColorSurrogate, Color>()
                .AddSurrogate<QuaternionSurrogate, Quaternion>()
                .AddSurrogate<Vector3Surrogate, Vector3>()
                .AddSurrogate<VersionSurrogate, Version>()
                .AddType<Optional<string>>()
                .Compile();
        }

        public NitroxDeliveryMethod.DeliveryMethod DeliveryMethod { get; protected set; } = NitroxDeliveryMethod.DeliveryMethod.ReliableOrdered;
        public UdpChannelId UdpChannel { get; protected set; } = UdpChannelId.DEFAULT;

        public enum UdpChannelId
        {
            DEFAULT = 0,
            PLAYER_MOVEMENT = 1,
            VEHICLE_MOVEMENT = 2,
            PLAYER_STATS = 3
        }

        public static byte[] Serialize(Packet packet)
        {
            using (MemoryStream ms = new MemoryStream())
            //using (LZ4Stream lz4s = new LZ4Stream(ms, LZ4StreamMode.Compress))
            {
                serializer.SerializeWithLengthPrefix(ms, packet, typeof(Packet), PrefixStyle.Fixed32BigEndian, -1);

                return ms.ToArray();
            }
        }

        public static WrapperPacket SerializeToWrapper(Packet packet)
        {
            return new WrapperPacket(Serialize(packet));
        }

        public static Packet Deserialize(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            //using (LZ4Stream lz4s = new LZ4Stream(ms, LZ4StreamMode.Decompress))
            {
                return (Packet)serializer.DeserializeWithLengthPrefix(ms, null, typeof(Packet), PrefixStyle.Fixed32BigEndian, -1);
            }
        }

        public static bool IsTypeSerializable(Type type)
        {
            return serializer.CanSerialize(type);
        }

        // Deferred cells are a replacement for the old DeferredPacket class.  The idea
        // is that some packets should not be replayed until a player enters close proximity.
        // when the player enters a deferred cell, the DeferredPacketReceiver will automatically
        // allow the packet to be processed. This method is virtual as some packets may have
        // complex logic to decide if it needs to defer.
        public virtual Optional<AbsoluteEntityCell> GetDeferredCell()
        {
            return Optional<AbsoluteEntityCell>.Empty();
        }
    }
}
