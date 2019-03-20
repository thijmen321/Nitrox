using NitroxClient.Communication.Packets.Processors.Abstract;
using NitroxClient.GameLogic;
using NitroxClient.GameLogic.Helper;
using NitroxModel.DataStructures.Util;
using NitroxModel.Packets;
using UnityEngine;

namespace NitroxClient.Communication.Packets.Processors
{
    public class SubRootChangedProcessor : ClientPacketProcessor<SubRootChanged>
    {
        private readonly PlayerManager remotePlayerManager;

        public SubRootChangedProcessor(PlayerManager remotePlayerManager)
        {
            this.remotePlayerManager = remotePlayerManager;
        }

        public override void Process(SubRootChanged packet)
        {
            Optional<RemotePlayer> remotePlayer = remotePlayerManager.Find(packet.PlayerId);

            if (remotePlayer.HasValue)
            {
                SubRoot subRoot = null;

                if (packet.SubRootGuid.HasValue)
                {
                    GameObject sub = GuidHelper.RequireObjectFrom(packet.SubRootGuid.Get());
                    subRoot = sub.GetComponent<SubRoot>();
                }

                remotePlayer.Get().SetSubRoot(subRoot);
            }
        }
    }
}
