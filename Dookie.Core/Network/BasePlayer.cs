﻿using Microsoft.Xna.Framework;

namespace Dookie.Core.Network;

public abstract class BasePlayer
{
    public readonly string Name;

    private GameTimer _shootTimer = new(0.2f);
    private BasePlayerManager _playerManager;

    protected Vector2 position = Vector2.Zero;
    protected float rotation;

    public const float Radius = 0.5f;
    public Vector2 Position => position;
    public readonly byte Id;
    public int Ping;

    protected BasePlayer(BasePlayerManager playerManager, string name, byte id)
    {
        Id = id;
        Name = name;
        _playerManager = playerManager;
    }

    public virtual void Spawn(Vector2 position)
    {
        this.position = position;
        rotation = 0;
    }

    // private void Shoot()
    // {
    //     const float MaxLength = 20f;
    //     Vector2 dir = new Vector2(Math.Cos(_rotation), Mathf.Sin(_rotation));
    //     var player = _playerManager.CastToPlayer(_position, dir, MaxLength, this);
    //     Vector2 target = _position + dir * (player != null ? Vector2.Distance(_position, player._position) : MaxLength);
    //     _playerManager.OnShoot(this, target, player);
    // }

    public virtual void ApplyInput(PlayerInputPacket command, float delta)
    {
        var direction = Vector2.Zero;
        var speed = 300f;

        if ((command.Keys & MovementKeys.Left) != 0)
            direction.X = -1f;
        if ((command.Keys & MovementKeys.Right) != 0)
            direction.X = 1f;
        if ((command.Keys & MovementKeys.SpeedUp) != 0)
            speed *= 2;

        if (direction != Vector2.Zero)
        {
            direction.Normalize();
        }
        
        var velocity = direction * speed * delta;
        position += velocity;

        // if ((command.Keys & MovementKeys.Fire) != 0)
        // {
        //     if (_shootTimer.IsTimeElapsed)
        //     {
        //         _shootTimer.Reset();
        //         Shoot();
        //     }
        // }
    }

    public virtual void Update(float delta)
    {
        _shootTimer.UpdateAsCooldown(delta);
    }
}