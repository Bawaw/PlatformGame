using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformGame
{
    class Camera
    {
        protected float zoom; 
        public Matrix transform; 
        public Vector2 pos; 
        protected float rotation;


        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; } 
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            pos += amount;
        }

        public Vector2 Pos
        {
            get { return pos; }
            set 
            {
                pos = value; 
                if (pos.Y + Game1.SCREEN_HEIGHT / 2 > Game1.MAP_HEIGHT)
                    pos.Y = Game1.MAP_HEIGHT - Game1.SCREEN_HEIGHT / 2;
                if (pos.X - Game1.SCREEN_WIDTH / 2 < 0)
                    pos.X = Game1.SCREEN_WIDTH / 2;
                if (pos.X + Game1.SCREEN_WIDTH / 2 > Game1.MAP_WIDTH)
                    pos.X = Game1.MAP_WIDTH - Game1.SCREEN_WIDTH / 2;   
            }
        }
 
        public Camera()
        {
            zoom = 1f;
            rotation = 0.0f;
            pos = Vector2.Zero;
        }

        public Matrix transformation(GraphicsDevice graphicsDevice)
        {
            transform =       
              Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(Game1.SCREEN_WIDTH * 0.5f, Game1.SCREEN_HEIGHT * 0.5f, 0));
            return transform;
        }
    }
}
