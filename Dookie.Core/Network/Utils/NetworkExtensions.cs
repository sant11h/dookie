using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace Dookie.Core.Network;

public static class NetworkExtensions
{
    public static void Put(this NetDataWriter writer, Vector2 vector)
    {
        writer.Put(vector.X);
        writer.Put(vector.Y);
    }

    public static Vector2 GetVector2(this NetDataReader reader)
    { 
        var x = reader.GetFloat(); 
        var y = reader.GetFloat();
        
        return new Vector2(float.IsNaN(x) ? 0 : x, float.IsNaN(y) ? 0 : y);
    }

    public static T GetRandomElement<T>(this T[] array)
    {
        return array[Random.Shared.Next(0, array.Length)];
    }
}