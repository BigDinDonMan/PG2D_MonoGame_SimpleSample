using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MonoGame_SimpleSample
{

    class AnimatedSprite : Sprite
    {

        int numberOfAnimationRows = 4;
        int animationFramesInRow = 9;

        int whichFrame;
        double currentFrameTime = 0;
        double expectedFrameTime = 100.0f;
        public Vector2 Middle { get
            {
                return position + new Vector2(frameWidth/2, frameHeight/2);

            }
        }


        public AnimatedSprite(Texture2D texture, Vector2 startingPosition, int numberOfAnimationRows, int animationFramesInRow, GraphicsDevice graphicsDevice) : base(texture, startingPosition, graphicsDevice)
        {

            base.frameHeight = texture.Height / numberOfAnimationRows;
            base.frameWidth = texture.Width / animationFramesInRow;

            this.numberOfAnimationRows = numberOfAnimationRows;
            this.animationFramesInRow = animationFramesInRow;

            boundingBox = new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3(position.X + frameWidth, position.Y + frameHeight, 0));

        }

        new public void Update(GameTime gameTime)
        {

            currentFrameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (currentFrameTime >= expectedFrameTime)
            {
                whichFrame = (whichFrame < animationFramesInRow-1) ? whichFrame + 1 : 0;
                currentFrameTime = 0;
            }

            base.updateBoundingBoxes();

        }


        new public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Rectangle(whichFrame*base.frameWidth, base.frameHeight*0, base.frameWidth, base.frameHeight), Color.White);
            //Debug_DrawBounds(graphicsDevice, spriteBatch);
        }


        //new public bool IsCollidingWith(Sprite otherSprite)
        //{
        //    //collsion left/right -> stop the left/right momentum
        //    if (this.leftBoundingBox.Intersects(otherSprite.RightBoundingBox))
        //    {
        //        isTouchingLeft = true;
        //    }
        //    else isTouchingLeft = false;
        //    if (this.rightBoundingBox.Intersects(otherSprite.LeftBoundingBox))
        //    {
        //        isTouchingRight = true;
        //    }
        //    else isTouchingRight = false;


        //    return this.boundingBox.Intersects(otherSprite.BoundingBox) ? true : false;

        //}


    }
}
