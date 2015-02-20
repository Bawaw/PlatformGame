using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PlatformGame
{
    class GameObject
    {
        public Texture2D CurrentTexture;
        public Texture2D normalTexture;

        private int TilesX;
        private int TilesY;
        public int tileWidth { get; private set; }
        public int tileHeight { get; private set; }
        private int prevFameWidth;
        protected int flip;
        public int prevFlip; //TODO make private
        private  int FrameStart;
        private int origFrameStart;
        protected int FrameStop;
        private int origFrameStop;

        private bool animate;

        public Vector2 Origin 
        { 
            get 
            { 
                if (! (tileHeight >= tileWidth - 10)) // haal textures eruit die in breedte werken
                {
                    if (flip == -1) /*links*/ return new Vector2(prevFameWidth, tileHeight);
                    else /*rechts*/           return new Vector2(0, tileHeight);
                } 
                return new Vector2(0, tileHeight); 
            } 
        }

        public int Flip
        {
            get {return flip ;}
            set
            {
                flip = value;

                    if (flip == -1)
                    {
                        this.FrameStart = frameCount - origFrameStart;
                        this.FrameStop = frameCount - origFrameStop;
                        if (flip != prevFlip)
                        {
                            CurrentTexture = flipTexture(normalTexture);
                            frame = FrameStart;
                        }
                    }

                    else
                    {
                        FrameStart = origFrameStart -1;
                        FrameStop = origFrameStop;
                        if (flip != prevFlip)
                        {
                            CurrentTexture = normalTexture;
                            frame = FrameStart;
                        }
                    }
                    prevFlip = flip;
            }
        }

        public int frame;

        private int frameCount;
        private double frameInterval;
        private double frameTimeRemaining;
        public Vector2 position;

        public static List<GameObject> GameObjecten = new List<GameObject>();

        public Rectangle frameBounds // frame bounds
        {
            get
            {
                int x = (frame % TilesX * tileWidth);
                int y = 0;

                if(FrameStart != FrameStop)
                    y = (frame / TilesX * tileHeight);

                return new Rectangle(x, y, tileWidth, tileHeight);
            }
        }

        public Rectangle bounds //position bounds
        {
            get
            {
                
                int Width = tileWidth;
                if(tileWidth != normalTexture.Width) Width = tileWidth - 10;
                return new Rectangle((int)position.X - (int)Origin.X,
                                     (int)position.Y - (int)Origin.Y,
                                     Width, tileHeight);
            }
        }

        public GameObject(Texture2D texture,Vector2 position)
            :this (texture,1,1,1,1,texture.Width,position,1,1) {}

        public GameObject(Texture2D texture, int tilesX, int tilesY, double frameRate, int frameCount, int tileWidth, Vector2 position,int framestart, int framestop)
        {
            this.CurrentTexture = texture;
            normalTexture = texture;
            this.TilesX = tilesX;
            this.TilesY = tilesY;
            this.tileWidth = tileWidth;
            this.tileHeight = this.CurrentTexture.Height / tilesY;
            this.frameCount = frameCount;
            this.frameInterval = 1 / frameRate;
            this.frameTimeRemaining = frameInterval;
            this.position = position;
            GameObjecten.Add(this);
            prevFlip = Flip*-1;
            this.FrameStart = framestart;
            this.FrameStop = framestop;
            this.origFrameStart = framestart;
            this.origFrameStop = framestop;
            frame = framestart;
            animate = true;
            if (framestart == framestop)
                animate = false;
        }

        public virtual void Update(GameTime gametime)
        {
            if(animate)
                this.frameTimeRemaining -= gametime.ElapsedGameTime.TotalSeconds;

            if (this.frameTimeRemaining <= 0)
            {
                if (flip == 1)
                {
                    frame++;
                    if (this.frame == FrameStop -1)
                        this.frame = FrameStart;
                }
                if(flip == 0)
                {
                    frame++;
                    if (frame == FrameStop)
                        this.frame = FrameStart;
                }
                if (flip == -1)
                {
                    frame--;
                    if (this.frame == FrameStop)
                        frame = FrameStart;
                }
                if (this.frame < 0)
                    this.frame = FrameStart;
                if (this.frame > frameCount)
                    this.frame = FrameStop;

                this.frameTimeRemaining = this.frameInterval;
            }
        }
        public void NewAnimation(int FirstFrame, int LastFrame, Texture2D texture, int tilesX, int tilesY, double frameRate, int frameCount, int tileWidth)
        {
            if(origFrameStart == FirstFrame && origFrameStop == LastFrame /*zelfde frame range*/
                || this.FrameStart == frameCount - FirstFrame && this.FrameStop == frameCount - LastFrame /*zelfde frame range flipped*/)
                return;

                animate = true;

                origFrameStart = FirstFrame;
                origFrameStop = LastFrame;

                if (texture != normalTexture)
                {
                    this.frameCount = frameCount;
                    prevFameWidth = this.tileWidth;
                    CurrentTexture = texture;
                    normalTexture = CurrentTexture;

                    this.TilesX = tilesX;
                    this.TilesY = tilesY;
                    this.tileWidth = tileWidth;
                    this.tileHeight = this.CurrentTexture.Height / tilesY;
                    this.frameCount = frameCount;
                    prevFlip = flip * -1; //new texture
                }

                Flip = flip; // roep flip property op voor framestart en framestop

                if (FirstFrame == LastFrame) //static frame
                {
                    animate = false;
                    if (flip == 1)
                        this.FrameStart = FirstFrame - 1;
                }

                this.frame = FrameStart;
                this.frameInterval = 1 / frameRate;
                this.frameTimeRemaining = this.frameInterval;
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(CurrentTexture, position, this.frameBounds, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0.0f);
        }

        public Rectangle NormilizeBounds(Rectangle rect)
        {
                Rectangle normilizedBounds = new Rectangle(rect.X - (int)this.position.X + this.frameBounds.X - (int)Origin.X, 
                         rect.Y - (int)this.position.Y + this.frameBounds.Y + (int)Origin.Y,
                         rect.Width, rect.Height);
                if (normilizedBounds.X < 0) normilizedBounds.X = 0;
                return normilizedBounds;
        }

        private static Texture2D flipTexture(Texture2D texture)
        {
            Texture2D flipped = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
            Color[] data = new Color[texture.Width * texture.Height];
            Color[] flippedData = new Color[data.Length];

            texture.GetData<Color>(data);

            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                {
                    int idx = texture.Width - 1 - x + y * texture.Width;
                    flippedData[x + y * texture.Width] = data[idx];
                }

            flipped.SetData<Color>(flippedData);

            return flipped;
        }
    }
}
