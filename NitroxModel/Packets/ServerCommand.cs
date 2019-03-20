using System;
using NitroxModel.Logger;

namespace NitroxModel.Packets
{
    [Serializable]
    public class ServerCommand : Packet
    {
        public string[] CmdArgs { get; } = new string[0];

        public ServerCommand()
        {

        }

        public ServerCommand(string[] args)
        {
            CmdArgs = args;
        }

        public ServerCommand(string cmd)
        {
            CmdArgs = cmd.Split(' ');
        }
    }
}
