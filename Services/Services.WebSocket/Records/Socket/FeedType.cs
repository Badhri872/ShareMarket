using System;
using System.Collections.Generic;
using System.Text;

namespace Services.WebSocket.Records
{
    public enum TICK_TYPE
    {
        Tick_Ack = 1,
        Tick = 2,
        Tick_Depth_Ack = 3,
        Tick_Depth = 4,        
    }
}
