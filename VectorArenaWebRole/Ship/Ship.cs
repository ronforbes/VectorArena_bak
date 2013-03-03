using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using VectorArenaWebRole;

namespace VectorArenaWebRole
{
    public class Ship : GameObject
    {
        public bool Alive;
        public float Health;
        public ShipWeapons Weapons;
        public ConcurrentDictionary<Action, bool> Actions;
        public Player Player;

        public enum Action
        {
            TurnLeft,
            TurnRight,
            ThrustForward,
            ThrustBackward,
            Fire
        }

        const float maxHealth = 1000;
        
        static int idCounter = 0;        
        

        public Ship(Vector2 position, BulletManager bulletManager) : base()
        {
            Id = Interlocked.Increment(ref idCounter);
            Movement = new ShipMovement(this, position);
            Weapons = new ShipWeapons(this, bulletManager);
            Radius = 15.0f;
            Alive = true;
            Health = maxHealth;
            
            Actions = new ConcurrentDictionary<Action, bool>();
            Actions.TryAdd(Action.TurnLeft, false);
            Actions.TryAdd(Action.TurnRight, false);
            Actions.TryAdd(Action.ThrustForward, false);
            Actions.TryAdd(Action.ThrustBackward, false);
            Actions.TryAdd(Action.Fire, false);
        }

        public void Damage(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Health = 0;
                Alive = false;
            }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            Movement.Update(elapsedTime);
            Weapons.Update();

            base.Update(elapsedTime);
        }
    }
}