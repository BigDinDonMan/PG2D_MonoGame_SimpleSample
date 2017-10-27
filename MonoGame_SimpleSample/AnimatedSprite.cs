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
    enum WalkingDirection
    {
        up = 0,
        left = 1,
        down = 2,
        right = 3,
        idle = 4 // not supported in the current sprite
    }

    class AnimatedSprite : Sprite
    {

        int numberOfAnimationRows = 4;
        int animationFramesInRow = 9;

        int whichFrame;
        double currentFrameTime = 0;
        double expectedFrameTime = 200.0f;
        WalkingDirection currentWalkingDirection = WalkingDirection.down;


        public AnimatedSprite(Texture2D texture, Vector2 startingPosition, int numberOfAnimationRows, int animationFramesInRow) : base(texture, startingPosition)
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

            updateMovement(gameTime);
            base.updateBoundingBox();

        }

        //void updateBoundingBox()
        //{
        //    boundingBox = new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3(position.X + frameWidth, position.Y + frameHeight, 0));
        //}

        //This should be a part of input manager
        void updateMovement(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var pressedKeys = keyboardState.GetPressedKeys();

            int pixelsPerSecond = 50;
            float movementSpeed = (float)(pixelsPerSecond * (gameTime.ElapsedGameTime.TotalSeconds));

            Vector2 movementVector = Vector2.Zero;
            if (pressedKeys.Length == 0)
            {
                //should be:
                //currentWalkingDirection = WalkingDirection.idle;
                
                //is right now (placeholder):
                whichFrame = 0;
            }
            else
            {
                foreach (var Key in pressedKeys)
                {
                    switch (Key)
                    {
                        case Keys.A:
                            {
                                currentWalkingDirection = WalkingDirection.left;
                                movementVector += new Vector2(-movementSpeed, 0);
                                break;
                            }

                        case Keys.D:
                            {
                                currentWalkingDirection = WalkingDirection.right;
                                movementVector += new Vector2(movementSpeed, 0);
                                break;
                            }
                        case Keys.W:
                            {
                                currentWalkingDirection = WalkingDirection.up;
                                movementVector += new Vector2(0, -movementSpeed);
                                break;
                            }
                        case Keys.S:
                            {
                                currentWalkingDirection = WalkingDirection.down;
                                movementVector += new Vector2(0, movementSpeed);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
               

            position += movementVector;
        }




        new public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Rectangle(whichFrame*base.frameWidth, base.frameHeight * (int)currentWalkingDirection, base.frameWidth, base.frameHeight), Color.White);

        }


    }
}
