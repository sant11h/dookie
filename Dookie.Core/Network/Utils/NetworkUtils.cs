namespace Dookie.Core.Network;

public class NetworkUtils
{
    public static bool CheckIntersection(float x1, float y1, float x2, float y2, BasePlayer player)
    {
        var cx = player.Position.X;
        var cy = player.Position.Y;
        var distX = x2-x1;
        var distY = y2-y1;
        var lineLenSqr = distX * distX + distY * distY;
        var dot = ( (cx-x1)*distX + (cy-y1)*distY ) / lineLenSqr;
        var closestX = x1 + dot * distX;
        var closestY = y1 + dot * distY;

        var dcx1 = closestX - x1;
        var dcy1 = closestY - y1;
        var dcx2 = closestX - x2;
        var dcy2 = closestY - y2;
        var distToLineSqr1 = dcx1 * dcx1 + dcy1 * dcy1;
        var distToLineSqr2 = dcx2 * dcx2 + dcy2 * dcy2;
        if (distToLineSqr1 > lineLenSqr || distToLineSqr2 > lineLenSqr)
            return false;
            
        distX = closestX - cx;
        distY = closestY - cy;
        return distX*distX + distY*distY <= BasePlayer.Radius * BasePlayer.Radius;
    }
    
    public static int SeqDiff(int a, int b)
    {
        return Diff(a, b, NetworkConstants.HalfMaxGameSequence);
    }
    
    public static int Diff(int a, int b, int halfMax)
    {
        return (a - b + halfMax*3) % (halfMax*2) - halfMax;
    }
}