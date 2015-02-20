using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformGame
{
    class Wall : GameObject
    {
        public Vector2[] corners;
        public float[] cornerMarkers;
        private float angle;
        Vector2 midlePoint;

        public Wall(Texture2D texture, Vector2 position)
            : base(texture, position) 
        {
            corners = new Vector2[4];
            corners[0] = new Vector2(this.position.X , this.position.Y - this.Origin.Y);
            corners[1] = new Vector2(this.position.X + tileWidth, this.position.Y - this.Origin.Y);
            corners[2] = new Vector2(this.position.X + tileWidth, this.position.Y);
            corners[3] = new Vector2(this.position.X, this.position.Y);
            midlePoint = new Vector2(this.position.X + tileWidth / 2, this.position.Y - tileHeight / 2);

            cornerMarkers = new float[4];

            for (int i = 0; i < cornerMarkers.Length; i++)
            {
                Vector2 direction = Vector2.Normalize(corners[i] - midlePoint);
                this.cornerMarkers[i] = (float)Math.Atan2(direction.Y, direction.X) + MathHelper.Pi;
            }
        }

        public char WallCollision(DynamicObject collidingObject)
        {
            Vector2 pos1 = new Vector2(collidingObject.hittBox.X + collidingObject.hittBox.Width / 2, collidingObject.hittBox.Y + collidingObject.hittBox.Height - 10);
            Vector2 pos2 = new Vector2(this.position.X + this.tileWidth / 2, this.position.Y - this.tileHeight / 2);
 
            Vector2 direction = Vector2.Normalize(pos1 - pos2);
            angle = (float)Math.Atan2(direction.Y, direction.X) + MathHelper.Pi;
            char Side;

            if (angle >= this.cornerMarkers[0] &&  angle < this.cornerMarkers[1])        
                Side = 'T';
            else if (angle >= this.cornerMarkers[1] && angle < this.cornerMarkers[2])        
                Side = 'R';
            else if (angle >= this.cornerMarkers[2] && angle < this.cornerMarkers[3])     
                Side = 'B';
            else
                Side = 'L';
            return Side;
        }
    }
}
