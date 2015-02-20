using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformGame
{
    class Spell
    {
        private static List<Spell> Spells = new List<Spell>(); 
        public float CastTime { get; private set; }
        public float CoolDown { get; private set; }
        public double TimeCooldown;

        public Spell(float CastTime, float CoolDown)
        {
            this.CastTime = CastTime;
            this.CoolDown = CoolDown;
            this.TimeCooldown = 0;
            Spells.Add(this);
        }

        public static void ManageCoolDowns(GameTime gametime)
        {
            foreach (Spell spell in Spells)
            {
                if (spell.CoolDown > 0)
                    spell.TimeCooldown -= gametime.ElapsedGameTime.TotalSeconds;
            }
        
        }
    }
}
