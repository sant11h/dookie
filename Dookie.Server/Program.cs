using LiteNetLib;
using LiteNetLib.Utils;

var listener = new EventBasedNetListener();
var server = new NetManager(listener);
server.Start(3074);

listener.ConnectionRequestEvent += OnListenerOnConnectionRequestEvent;
listener.PeerConnectedEvent += OnListenerOnPeerConnectedEvent;
listener.NetworkLatencyUpdateEvent += ListenerOnNetworkLatencyUpdateEvent;

while (!Console.KeyAvailable)
{
    server.PollEvents();
    Thread.Sleep(15);
}

server.Stop();
return;

void OnListenerOnConnectionRequestEvent(ConnectionRequest request)
{
    if (server.ConnectedPeersCount < 10 /* max connections */)
        request.AcceptIfKey("SomeConnectionKey");
    else
        request.Reject();
}

void OnListenerOnPeerConnectedEvent(NetPeer peer)
{
    Console.WriteLine("We got connection: {0}", peer);
    var writer = new NetDataWriter();
    writer.Put("Hello client!");

    peer.Send(writer, DeliveryMethod.ReliableOrdered);
}

void ListenerOnNetworkLatencyUpdateEvent(NetPeer peer, int latency)
{
    var writer = new NetDataWriter();
    writer.Put($"Latency: {latency}");
    peer.Send(writer, DeliveryMethod.ReliableOrdered);
}