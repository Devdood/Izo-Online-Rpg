using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RegisterPacket : Packet
{
    public RegisterPacket(RegisterData data) : base()
    {
        writer.Write((byte)24);

        writer.Write(data.username);
        writer.Write(data.pass1);
        writer.Write(data.pass2);
        writer.Write(data.email);
    }
}