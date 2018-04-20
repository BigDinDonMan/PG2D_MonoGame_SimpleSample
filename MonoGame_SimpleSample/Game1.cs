using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGame_SimpleSample
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
	enum GameState 
	{
		playing,
		paused
	}
	
	
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch effectsSpriteBatch;


        Texture2D playerTexture;
        PlayerSprite playerSprite;

        Texture2D groundTexture;
        Sprite groundSprite;
		GameState currentGameState = GameState.playing;

        Texture2D backgroundTexture;
        //ScrollingBackground scrollingBackground;

        Texture2D noise;
        Texture2D perlinNoise;
        Texture2D lightMap;

        bool isPauseKeyHeld = false;

        string collisionText = "";
        SpriteFont HUDFont;
        Effect defaultShader;
        Effect lightingShader;
        Effect portalRippleShader;


        List<AnimatedSprite> Level;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 1000;
            

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Level = new List<AnimatedSprite>();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effectsSpriteBatch = new SpriteBatch(GraphicsDevice);
            playerTexture = Content.Load<Texture2D>("professor_walk_cycle_no_hat");

            var lines = System.IO.File.ReadAllLines(@"Content/Level1.txt");
            foreach(var line in lines)
            {
                var data = line.Split(';');

                Texture2D tempTexture = Content.Load<Texture2D>(data[0]);
                Vector2 tempPos = new Vector2(int.Parse(data[1]), int.Parse(data[2]));
                int animationRows = int.Parse(data[3]);
                int animationFramesInRow = int.Parse(data[4]);

                Level.Add(new AnimatedSprite(tempTexture, tempPos,animationRows, animationFramesInRow, GraphicsDevice));

            }
            groundTexture = Content.Load<Texture2D>("ground");
            groundSprite = new Sprite(groundTexture, new Vector2(0, graphics.GraphicsDevice.Viewport.Height - groundTexture.Height), GraphicsDevice);

            backgroundTexture = Content.Load<Texture2D>("wall_background");
            //scrollingBackground = new ScrollingBackground(backgroundTexture, new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

            playerSprite = new PlayerSprite(playerTexture, Vector2.Zero , 4, 9, GraphicsDevice);
            playerSprite.Position = new Vector2(0, graphics.PreferredBackBufferHeight - (groundTexture.Height + (playerSprite.BoundingBox.Max.Y - playerSprite.BoundingBox.Min.Y) + 30));
            HUDFont = Content.Load<SpriteFont>("HUDFont");

            //Effect Textures

            noise = Content.Load<Texture2D>("noise");
            perlinNoise = Content.Load<Texture2D>("perlin_noise");
            lightMap = Content.Load<Texture2D>("light_map_inv");

            //Effects
            defaultShader = Content.Load<Effect>("DefaultShader");
            lightingShader = Content.Load<Effect>("TorchesLightingShader");
            portalRippleShader = Content.Load<Effect>("Portal_ripple");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardState = Keyboard.GetState();


            if( keyboardState.IsKeyDown(Keys.P) && !isPauseKeyHeld)
            {

                if (currentGameState == GameState.playing)
                        currentGameState = GameState.paused;
                else currentGameState = GameState.playing;
            }


            //This should be in the Input Manager - differentiate between pressed and held
            isPauseKeyHeld = keyboardState.IsKeyUp(Keys.P) ? false : true;



            // TODO: Add your update logic here
            switch (currentGameState)
			{
				case GameState.playing:
				{
                    //Update ground:
                    groundSprite.Update(gameTime);
                    //scrollingBackground.Update(gameTime);

                    //Update Level
                    foreach(var sprite in Level)
                    {
                        sprite.Update(gameTime);
                    }

                    playerSprite.Update(gameTime);
                   
                    //check collisions
                    playerSprite.IsCollidingWith(groundSprite);


                    //Effects
                    //Lighting from torches on the wall
                    Vector2 screenMiddle = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);


                    //portalRippleShader.Parameters["tint"].SetValue(0.0f);

                    portalRippleShader.Parameters["Timer"].SetValue((float)gameTime.TotalGameTime.TotalSeconds*10);
                    portalRippleShader.Parameters["Refracton"].SetValue(50.0f);
                    portalRippleShader.Parameters["VerticalTroughWidth"].SetValue(23.0f);
                    portalRippleShader.Parameters["Wobble2"].SetValue(23.0f);

                    lightingShader.Parameters["lightPos_1"].SetValue(Level[0].Middle);
                    lightingShader.Parameters["lightPos_2"].SetValue(Level[1].Middle);
                    lightingShader.Parameters["lightPos_3"].SetValue(Level[2].Middle);
                    lightingShader.Parameters["lightPos_4"].SetValue(Level[3].Middle);
                    lightingShader.Parameters["lightMapSize"].SetValue(new Vector2(lightMap.Width, lightMap.Height));
                    lightingShader.Parameters["textureSize"].SetValue(new Vector2(backgroundTexture.Width, backgroundTexture.Height));
                    lightingShader.Parameters["LightMapTexture"].SetValue(lightMap);




                    }
                    break;
				
				case GameState.paused:
				{
					
				}
				
				break;
				
			}

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // TODO: Add your drawing code here

            effectsSpriteBatch.Begin(effect: lightingShader);
            spriteBatch.Begin();

            //spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            effectsSpriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            effectsSpriteBatch.End();
            //draw the ground
            //groundSprite.Draw(GraphicsDevice, spriteBatch);
            groundSprite.Draw(GraphicsDevice, spriteBatch);

            //Draw only torches
            for (int i =0; i< 4; i++)
            {
                Level[i].Draw(GraphicsDevice, spriteBatch);
            }


            //Draw only door + portal
            effectsSpriteBatch.Begin(effect: portalRippleShader);
            Level[4].Draw(GraphicsDevice, effectsSpriteBatch);
            effectsSpriteBatch.End();

            effectsSpriteBatch.Begin();
            Level[5].Draw(GraphicsDevice, effectsSpriteBatch);
            effectsSpriteBatch.End();

            playerSprite.Draw(GraphicsDevice, spriteBatch);
            spriteBatch.DrawString(HUDFont, collisionText, new Vector2(700, 0), Color.Red);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
