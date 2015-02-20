using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformGame
{
    class Mobster : DynamicObject
    {
        private const double DAZEDTIME = 0.4;
        private double dazedTimer;
        private int normalSpeed = 2;

        public Mobster(Texture2D texture, Vector2 position)
            : this(texture,  7/*Xframes*/, 1/*Yframes*/, 9/*frames/sec*/, 7 /*TotalFrames*/, 100 /*TileWidth*/, position, 1 /*frameStart*/, 7 /*frameStop*/) { }

        private Mobster(Texture2D texture, int tilesX, int tilesY, double frameRate, int frameCount, int tileWidth, Vector2 position, int framestart, int framestop)
            : base(texture, tilesX, tilesY, frameRate, frameCount, tileWidth, position, framestart, framestop)
        {
            dazedTimer = 0;
        }

        private void CalculatePath()
        {
            this.NewAnimation(1, 7, normalTexture, 7, 1, 9, 7, 100);
            int Side = random.Next(0, 100);

            if (Side <= 50)
                Flip = 1;
            else
                Flip = -1;

            velocity.X = normalSpeed * Flip;
            dazedTimer = -1;
        }

        public override void Update(GameTime gametime)
        {
            if(dazedTimer <= 0 && dazedTimer > -1)
                CalculatePath();

            anyCollision();
            base.Update(gametime);
        }

        protected override void timers(GameTime gametime)
        {
            if (this.dazedTimer > 0)
                this.dazedTimer -= gametime.ElapsedGameTime.TotalSeconds;
        } 

        protected override void wallCollision(Char Side,Wall colidedObject)
        {
            switch (Side)
            {

                case 'T':
                    {
                        this.position.Y = colidedObject.position.Y - colidedObject.tileHeight + 1;
                        this.velocity.Y = 0;
                        break;
                    }
                case 'R':
                    {
                        this.position.X = colidedObject.position.X + tileWidth;
                        if (this.velocity.X < 0)
                            this.velocity.X *= -1;
                        Flip = 1;
                        WallStun();
                        break;
                    }
                case 'B':
                    {
                        position.Y = colidedObject.position.Y + tileHeight;
                        if (velocity.Y < 0)
                            velocity.Y = 0;
                        break;
                    }
                case 'L':
                    {
                        this.position.X = colidedObject.position.X - tileWidth;
                        if (this.velocity.X > 0)
                            this.velocity.X *= -1;
                        Flip = -1;
                        WallStun();
                        break;
                    }
           }
        }

        protected override void CheckBounds()
        {
            if (this.position.X < 0)
            {
                this.position.X = 10;
                velocity.X *= -1;
                Flip = 1;
                WallStun();
            }
            else if (this.position.X > Game1.MAP_WIDTH - tileWidth)
            {
                this.position.X = Game1.MAP_WIDTH - tileWidth;
                velocity.X *= -1;
                Flip = -1;
                WallStun();
            }
        }

        private void WallStun ()
        {
            if (this.dazedTimer > 0)
            {
                dazedTimer *= 2;
                this.velocity.X = 0;
            }
        }

        public void OffenciveHit(Vector2 knockback,int side,int hpDamage)
        {
            //this.position += knockback * side;
            this.velocity = knockback *side;
            this.NewAnimation(7, 7, normalTexture , 7, 1, 0, 7, 100);
            this.Flip = side *-1;
            this.dazedTimer = DAZEDTIME;
        }
    }
}
