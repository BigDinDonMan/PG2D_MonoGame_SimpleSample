using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame_SimpleSample
{
    class ScrollingBackground
    {

        private Texture2D texture;
        private Vector2 positionL;
        private Vector2 positionR;
        private Vector2 screenDimmensions;

        private int frameWidth;
        private int frameHeight;

        private int scrollingSpeed = -30;


        public int ScrollingSpeed
        {
            get
            {
                return this.scrollingSpeed;
            }
            set
            {
                this.scrollingSpeed = value;
            }
        }

        public ScrollingBackground(Texture2D backgroundTtexture, Vector2 screenDimmensions)
        {
            this.texture = backgroundTtexture;

            frameWidth = texture.Width;
            frameHeight = texture.Height;

            positionL = Vector2.Zero;
            positionR = new Vector2(frameWidth, 0);

            this.screenDimmensions = screenDimmensions;
        }


        public void Update (GameTime gameTime)
        {
            if((positionL.X + frameWidth) < 0)
            {
                positionL = positionR;
                positionR.X = positionL.X + frameWidth;
            }

            if(positionL.X > 0)
            {
                positionR = positionL;
                positionL.X = positionR.X - frameWidth;
            }

            positionL.X += (float)(scrollingSpeed*gameTime.ElapsedGameTime.TotalSeconds);
            positionR.X += (float)(scrollingSpeed*gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, positionL, Color.White);
            spriteBatch.Draw(texture, positionR, Color.White);
        }







    }
}
