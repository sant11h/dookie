using System.Collections;
using Microsoft.Xna.Framework;

namespace Dookie.Core.Network;

public abstract class BasePlayerManager: IEnumerable<BasePlayer>
{
    public abstract IEnumerator<BasePlayer> GetEnumerator();
    public abstract int Count { get; }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public BasePlayer CastToPlayer(Vector2 from, Vector2 dir, float length, BasePlayer exclude)
    {
        BasePlayer result = null;
        Vector2 target = from + dir * length;
        foreach(var p in this)
        {
            if(p == exclude)
                continue;
            if (NetworkUtils.CheckIntersection(from.X, from.Y, target.X, target.Y, p))
            {
                //TODO: check near
                if(result == null)
                    result = p;
            }
        }
            
        return result;
    }

    public abstract void LogicUpdate();
}