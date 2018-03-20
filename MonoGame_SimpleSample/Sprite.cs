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

        //Debug textures - just for drawing bounding boxes
        Texture2D rect;

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

        protected Rectangle boundingRectangle;
        public Rectangle BoundingRectangle
        {
            get
            {
                return this.boundingRectangle;
            }
        }


        protected BoundingBox bottomBoundingBox;
        public BoundingBox BottomBoundingBox
        {
            get
            {
                return this.bottomBoundingBox;
            }

        }
        protected BoundingBox topBoundingBox;
        public BoundingBox TopBoundingBox
        {
            get
            {
                return this.topBoundingBox;
            }

        }

        protected BoundingBox leftBoundingBox;
        public BoundingBox LeftBoundingBox
        {
            get
            {
                return this.leftBoundingBox;
            }

        }
        protected BoundingBox rightBoundingBox;
        public BoundingBox RightBoundingBox
        {
            get
            {
                return this.rightBoundingBox;
            }

        }

        protected int frameWidth;
        protected int frameHeight;

        public Sprite(Texture2D texture, Vector2 startingPosition, GraphicsDevice graphicsDevice)
        {
            rect = new Texture2D(graphicsDevice, 1, 1);
            position = startingPosition;
            this.texture = texture;
            frameHeight = texture.Height;
            frameWidth = texture.Width;
            updateBoundingBoxes();
        }


        public void Update(GameTime gameTime)
        {
            updateBoundingBoxes();
            updateRectengle();
        }

        private void updateRectengle()
        {
            boundingRectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
        }

        protected void updateBoundingBoxes()
        {
            boundingBox = new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3(position.X + frameWidth, position.Y + frameHeight, 0));
            bottomBoundingBox = new BoundingBox(new Vector3(position.X + 4, position.Y + frameHeight - 4, 0), new Vector3(position.X + frameWidth - 4, position.Y + frameHeight, 0));
            topBoundingBox = new BoundingBox(new Vector3(position.X + 4, position.Y, 0), new Vector3(position.X + frameWidth - 4, position.Y + 4, 0));
            leftBoundingBox = new BoundingBox(new Vector3(position.X, position.Y + 4, 0), new Vector3(position.X + 4, position.Y + frameHeight - 4, 0));
            rightBoundingBox = new BoundingBox(new Vector3(position.X + frameWidth - 4, position.Y + 4, 0), new Vector3(position.X + frameWidth, position.Y + frameHeight - 4, 0));
        }


        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            //Debug_DrawBounds(graphicsDevice, spriteBatch);

        }

        #region Collisions - BoundingBox
        public bool IsCollidingWith(Sprite otherSprite)
        {
            return this.boundingBox.Intersects(otherSprite.BoundingBox) ? true : false;
        }


        public void Debug_DrawBounds(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            DrawRectangle(graphicsDevice, spriteBatch, BottomBoundingBox, Color.Red);
            DrawRectangle(graphicsDevice, spriteBatch, TopBoundingBox, Color.Green);
            DrawRectangle(graphicsDevice, spriteBatch, LeftBoundingBox, Color.Blue);
            DrawRectangle(graphicsDevice, spriteBatch, RightBoundingBox, Color.Violet);
        }

        private void DrawRectangle(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, BoundingBox boundingBox, Color color)
        {

            //rect = new Texture2D(graphicsDevice, 1, 1); - TODO: fix memory leak caused by this line
            rect.SetData(new[] { Color.White });
            int rectWidth = (int)(boundingBox.Max.X - boundingBox.Min.X);
            int rectHeight = (int)(boundingBox.Max.Y - boundingBox.Min.Y);

            Rectangle coords = new Rectangle((int)boundingBox.Min.X, (int)boundingBox.Min.Y, rectWidth, rectHeight);

            spriteBatch.Draw(rect, coords, color);
        }

        #endregion
        #region Collisions - Rectangles

        public bool isCollidingLeft(Sprite otherSprite)
        {
            return this.boundingRectangle.Right + 5 > otherSprite.boundingRectangle.Left &&
                   this.boundingRectangle.Left < otherSprite.boundingRectangle.Left &&
                   this.boundingRectangle.Bottom > otherSprite.boundingRectangle.Top &&
                   this.boundingRectangle.Top < otherSprite.boundingRectangle.Bottom;
        }

        public bool isCollidingRight(Sprite otherSprite)
        {
            return this.boundingRectangle.Left - 5 < otherSprite.boundingRectangle.Right &&
                   this.boundingRectangle.Right > otherSprite.boundingRectangle.Right &&
                   this.boundingRectangle.Bottom > otherSprite.boundingRectangle.Top &&
                   this.boundingRectangle.Top < otherSprite.boundingRectangle.Bottom;
        }

        public bool isCollidingTop(Sprite otherSprite)
        {
            return this.boundingRectangle.Bottom  + 5 > otherSprite.boundingRectangle.Top &&
                   this.boundingRectangle.Top < otherSprite.boundingRectangle.Top &&
                   this.boundingRectangle.Right > otherSprite.boundingRectangle.Left &&
                   this.boundingRectangle.Left < otherSprite.boundingRectangle.Right;
        }

        public bool isCollidingBottom(Sprite otherSprite)
        {
            return this.boundingRectangle.Top  - 5 < otherSprite.boundingRectangle.Bottom &&
                   this.boundingRectangle.Bottom > otherSprite.boundingRectangle.Bottom &&
                   this.boundingRectangle.Right > otherSprite.boundingRectangle.Left &&
                   this.boundingRectangle.Left < otherSprite.boundingRectangle.Right;
        }

        #endregion
    }
}
