using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformGame
{
    class Player : DynamicObject
    { 
        public enum playerAction //TODO make Private
        {
            attack1,
            moving,
            climbing,
        }
        public playerAction playeraction;

        private float gravity;
        private const float GRAVITY_VALUE = 0.4f;
        public bool[] collisions;


        private double globalCooldown;
        private double castTime;
        private Spell attack1;
        Texture2D BaseTexture;
        Texture2D Airborne;
        Texture2D AttackTexture1;
        Texture2D climbingTexture;

        public Player(Texture2D texture, Texture2D AttackText1, Texture2D AirborneTexture, Vector2 position,Texture2D climbingTexture)
            : this(texture, AttackText1, AirborneTexture,  19/*Xframes*/, 1/*Yframes*/, 5/*frames/sec*/, 19 /*TotalFrames*/, 60 /*TileWidth*/, position, 10 /*frameStart*/, 17 /*frameStop*/,climbingTexture) { }

        private Player(Texture2D texture, Texture2D AttackTexture1, Texture2D AirborneTexture, int tilesX, int tilesY, double frameRate, int frameCount, int tileWidth, Vector2 position, int framestart, int framestop, Texture2D climbingTexture)
            : base(texture, tilesX, tilesY, frameRate, frameCount, tileWidth, position,framestart,framestop)
        {
            this.playeraction = playerAction.moving;
            this.BaseTexture = texture;
            this.AttackTexture1 = AttackTexture1;
            this.climbingTexture = climbingTexture;
            gravity = GRAVITY_VALUE;
            collisions = new bool[4]; 
            recallCollisions();
            this.Airborne = AirborneTexture;
            globalCooldown = 0;
            castTime = -1;
            attack1 = new Spell(0.9f, 0);
            Flip = 1;
        }

        public override void Update(GameTime gametime)
        {
            recallCollisions();
            anyCollision();
            Gravity();
                        userInput();
            animate();
            MobOrSpellCollision();
            base.Update(gametime);
        }

        private void recallCollisions()
        {
            collisions[0] = false; //TOP of player
            collisions[1] = false; // RIGHT
            collisions[2] = false; // BOT
            collisions[3] = false; // LEFT
        }

        protected override void timers(GameTime gametime)
        {
            Spell.ManageCoolDowns(gametime);

            if(globalCooldown > 0)
                this.globalCooldown -= gametime.ElapsedGameTime.TotalSeconds;
            if (this.castTime > 0)
                this.castTime -= gametime.ElapsedGameTime.TotalSeconds;

            if (this.castTime < 0 && playeraction == playerAction.attack1)
                playeraction = playerAction.moving;
        }

        private void userInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            switch (playeraction)
            {
                case playerAction.moving:
                    {
                        if (keyboardState.IsKeyDown(Keys.Space))
                        {
                            if (collisions[2])
                                velocity.Y = -11;
                        }

                        if (keyboardState.IsKeyDown(Keys.Up))
                        {
                            foreach (GameObject rope in GameObjecten)
                            {
                                if (rope is Rope)
                                {
                                    if (intersect(this.hittBox, rope.bounds) != Rectangle.Empty)
                                    {
                                        Flip = 0;
                                        this.velocity.X = 0;
                                        this.velocity.Y = 0;
                                        playeraction = playerAction.climbing;
                                        this.position.X = rope.position.X + (rope.tileWidth / 2) - 15; // 1/2 breedte klim texture
                                        return;
                                    }
                                }
                            }
                        }
                        else if (keyboardState.IsKeyDown(Keys.C) && collisions[2] && attack1.CoolDown <= 0 && castTime <= 0 && globalCooldown <= 0)
                        {
                             castTime = attack1.CastTime;
                             playeraction = playerAction.attack1;
                             velocity.X = 0;
                             attack1.TimeCooldown = attack1.CoolDown;
                             globalCooldown = 1;
                        }

                        if ((keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left)) ||
                        (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right)))
                        {
                            velocity.X = 0;
                        }

                        else if (keyboardState.IsKeyDown(Keys.Left) && !collisions[3])
                        {
                            if (velocity.X > -4)
                                velocity.X -= 0.2f;
                            Flip = -1;
                        }

                        else if (keyboardState.IsKeyDown(Keys.Right) && !collisions[1])
                        {
                            if (velocity.X < 4)
                                velocity.X += 0.2f;
                            Flip = 1;
                        }

                        if (playeraction != playerAction.moving || playeraction == playerAction.climbing)
                            velocity.X = 0;

                        break;
                    }

                case playerAction.climbing:
                    {
                        foreach (GameObject rope in GameObjecten)
                        {
                            if (rope is Rope)
                            {
                                if (intersect(this.hittBox, rope.bounds) != Rectangle.Empty)
                                {
                                    if (keyboardState.IsKeyDown(Keys.Up))
                                        this.position.Y -= 4;
                                    else if (keyboardState.IsKeyDown(Keys.Down))
                                        this.position.Y += 4;
                                    flip = 0;
                                    if (keyboardState.IsKeyDown(Keys.Left) && !collisions[3])
                                    {
                                        this.position.X -= 30;
                                        if (velocity.X > -4)
                                            velocity.X -= 0.2f;
                                        Flip = -1;
                                        playeraction = playerAction.moving;
                                    }
                                    else if (keyboardState.IsKeyDown(Keys.Right) && !collisions[1])
                                    {
                                        if (velocity.X < 4)
                                            velocity.X += 0.2f;
                                        Flip = 1;
                                        playeraction = playerAction.moving;
                                    }
                                    
                                    return;
                                }
                            }
                        }
                        playeraction = playerAction.moving;
                        break;
                    }
            }
         }       

        private void animate()
        {
            if (this.velocity.Y != 0)
                NewAnimation(2, 2, Airborne, 4, 1, 1, 4, 71);

            else if (castTime > 0) //spell
            {
                if (playeraction == playerAction.attack1)
                    NewAnimation(1, 7, AttackTexture1, 7, 1, 8, 7, 119);
            }
            else if (this.velocity.X != 0)
                NewAnimation(3, 10, BaseTexture, 19, 1, 10, 19, 60);
            else if(playeraction == playerAction.climbing)
                NewAnimation(1, 2, climbingTexture, 2, 1, 10, 2, 45);
            else if (this.velocity.X == 0)
                NewAnimation(10, 17, BaseTexture, 19, 1, 5, 19, 60);
        }

        private void Gravity()
        {
            if(collisions[2] || playeraction == playerAction.climbing )
                return;
            if (velocity.Y < 16)
                velocity.Y += gravity;
        }
        protected override void wallCollision(Char Side,Wall colidedObject)
        {
            switch (Side)
            {
                case 'R':
                    {
                        this.position.X = colidedObject.position.X + colidedObject.tileWidth;
                      
                        if (this.velocity.X < 0)
                            this.velocity.X = 0;
                        this.collisions[3] = true;
                        break;
                    }
                case 'B':
                    {
                        position.Y = colidedObject.position.Y + bounds.Height;
                        this.collisions[0] = true;
                        if (velocity.Y < 0)
                            velocity.Y = 0;
                        break;
                    }
                case 'L':
                    {
                        this.position.X = colidedObject.position.X - hittBox.Width ;

                        if (this.velocity.X > 0)
                            this.velocity.X = 0;
                        this.collisions[1] = true;
                        break;
                    }
                case 'T':
                    {
                        this.position.Y = colidedObject.position.Y - colidedObject.tileHeight + 1;
                        this.collisions[2] = true;
                        this.velocity.Y = 0;
                        break;
                    }
            }
        }

        protected override void CheckBounds()
        {
            if (this.position.X < 0)
            {
                this.position.X = 0;
            }
            else if (this.position.X > Game1.MAP_WIDTH - tileWidth)
            {
                this.position.X = Game1.MAP_WIDTH - tileWidth;
            }
        }

        private void MobOrSpellCollision()
        {
            foreach (GameObject item in GameObject.GameObjecten)
            {
                if (item is Mobster)
                {
                    if (checkCollision(this, item))
                    {
                        var mob = item as Mobster;
                        HostileCollision(mob);
                    }
                }
            }
        }

        private void HostileCollision(Mobster mob)
        {
            switch (playeraction)
            {
                case playerAction.attack1:
                    if(this.frame == FrameStop -3*Flip )
                        mob.OffenciveHit(new Vector2(10,0), flip, 10);
                    break;
                case playerAction.moving:
                    break;
                default:
                    break;
            }
        }
    }
}
