using Dookie.Core.Network;

var server = new ServerNetworkManager();
server.Start();

while (!Console.KeyAvailable)
{
    server.Update();
    Thread.Sleep(15);
}

server.Stop();
