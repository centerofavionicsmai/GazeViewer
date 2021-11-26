using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Net;

namespace GazeViewer.Infastructure
{
    class UDPController
    {
        private UdpClient client { get; }

        public UDPController(int port)
        {
            if (client == null)
            {
                client = new UdpClient(port);
            }
        }


        public async Task<byte[]> ReceiveBytesAsync()
        {
            while (true)
            {
              var received= await client.ReceiveAsync();
               return received.Buffer;
            }



        }


    }
}
