using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformGame
{
    class DynamicObject: GameObject
    {
        public Vector2 velocity;
        private const int ALPHA_TRESHHOLD = 38;
        public static string testcollision = "no Collision"; 
        protected static Random random = new Random();

        public Rectangle hittBox
        {
            get
            {
                return new Rectangle((int)position.X,
                                     (int)position.Y - 80,
                                     50, 80);
            }
        }

        public DynamicObject(Texture2D texture, int tilesX, int tilesY, double frameRate, int frameCount, int tileWidth, Vector2 position, int framestart, int framestop)
            : base(texture, tilesX, tilesY, frameRate, frameCount, tileWidth,position,framestart,framestop)
        {
            velocity = new Vector2(0, 0);
        }

        public override void Update(GameTime gametime)
        {
            move();
            CheckBounds();
            timers(gametime);
            base.Update(gametime);
        }

        private void move()
        {
            position += velocity;
        }

        #region methods
        protected virtual void wallCollision(char side,Wall collidedObject) {}

        protected void anyCollision()
        {
            foreach (GameObject item in GameObjecten)
            {
                if (item is Wall)
                {
                   //if (checkCollision(this,item))
                    if(intersect(this.hittBox,item.bounds) != Rectangle.Empty)
                    {
                        var wall = item as Wall;
                        wallCollision(wall.WallCollision(this), wall);
                    }
                }
            }
        }


        #endregion

        #region collision

        protected virtual void timers(GameTime gameTime) { }


        public static Rectangle intersect(Rectangle boundsA, Rectangle boundsB)
        {
            int x1 = Math.Max(boundsA.Left, boundsB.Left);
            int y1 = Math.Max(boundsA.Top,boundsB.Top);
            int x2 = Math.Min(boundsA.Right, boundsB.Right);
            int y2 = Math.Min(boundsA.Bottom, boundsB.Bottom);

            int width = x2 - x1;
            int height = y2 - y1;

            if (width > 0 && height > 0)
                return new Rectangle(x1, y1, width, height);
            else
                return Rectangle.Empty;          
        }

        public static bool checkCollision(GameObject objectA, GameObject objectB)
        {
            Rectangle collisionRect = intersect(objectA.bounds, objectB.bounds);

            if (collisionRect == Rectangle.Empty)
            {
                testcollision = "no Collision";
                return false;
            }

            int pixelcount = collisionRect.Width * collisionRect.Height;
            Color[] pixelsA = new Color[pixelcount];
            Color[] pixelsB = new Color[pixelcount];
            objectA.CurrentTexture.GetData<Color>(0, objectA.NormilizeBounds(collisionRect), pixelsA, 0, pixelcount);
            objectB.CurrentTexture.GetData<Color>(0, objectB.NormilizeBounds(collisionRect), pixelsB, 0, pixelcount);

          
            for (int i = 0; i < pixelcount; i++)
            {
                if (pixelsA[i].A > ALPHA_TRESHHOLD && pixelsB[i].A > ALPHA_TRESHHOLD)
                {
                    testcollision = "collision!";
                    return true;
                }        
            }
             testcollision = "no Collision";
            return false;
        }

        protected virtual void CheckBounds()
        {
            if (this.position.X < 0 || this.position.X > Game1.SCREEN_WIDTH - tileWidth)
                GameObjecten.Remove(this);
        }

        #endregion
    }
}
