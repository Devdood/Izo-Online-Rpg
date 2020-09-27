using System;
using System.Collections.Generic;
using System.Text;

namespace WebSocketMMOServer.GameServer.Packets.Outgoing
{
    public class OpenUIPanelPacket : Packet
    {
        public OpenUIPanelPacket(byte panel) : base()
        {
            writer.Write((byte)40);
            writer.Write((byte)panel);
        }
    }
}