using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_SimpleSample
{
    class Sprite
    {

        protected Texture2D texture;
        protected Vector2 position;
        public Vector2 Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }


        protected BoundingBox boundingBox;
        public BoundingBox BoundingBox
        {
            get
            {
                return this.boundingBox;
            }

        }

        protected int frameWidth;
        protected int frameHeight;

        public Sprite(Texture2D texture, Vector2 startingPosition)
        {
            position = startingPosition;
            this.texture = texture;
            frameHeight = texture.Height;
            frameWidth = texture.Width;
            boundingBox = new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3(position.X + frameWidth, position.Y + frameHeight, 0));

        }


        public void Update(GameTime gameTime)
        {
            updateBoundingBox();
        }

        protected void updateBoundingBox()
        {
            boundingBox = new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3(position.X + frameWidth, position.Y + frameHeight, 0));

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);

        }

        public bool IsCollidingWith(Sprite otherSprite)
        {
            return this.boundingBox.Intersects(otherSprite.BoundingBox) ? true : false;
        }

    }
}
