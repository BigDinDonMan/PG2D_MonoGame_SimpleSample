﻿using Microsoft.Xna.Framework;
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
        SpriteBatch backgroudSpriteBatch;


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
            backgroudSpriteBatch = new SpriteBatch(GraphicsDevice);
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
            lightMap = Content.Load<Texture2D>("light_map");

            //Effects
            defaultShader = Content.Load<Effect>("DefaultShader");
            lightingShader = Content.Load<Effect>("TorchesLightingShader");


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

                        //lightingShader.Parameters["SpriteTexture"].SetValue(backgroundTexture);
                        lightingShader.Parameters["LightMapTexture"].SetValue(lightMap);

                        lightingShader.Parameters["lightPos_1"].SetValue(Level[0].Position);
                    //lightingShader.Parameters["lightPos_2"].SetValue(Level[0].Position);
                    //lightingShader.Parameters["lightPos_3"].SetValue(Level[0].Position);
                    //lightingShader.Parameters["lightPos_4"].SetValue(Level[0].Position);
                    lightingShader.Parameters["lightMapSize"].SetValue(new Vector2(lightMap.Width, lightMap.Height));
                    lightingShader.Parameters["textureSize"].SetValue(new Vector2(backgroundTexture.Width, backgroundTexture.Height));
                        //lightingShader.Parameters["LightMapTexture"].SetValue(lightMap);
                        //lightingShader.Parameters["LightMapTexture"].SetValue(lightMap);
                        //lightingShader.Parameters["LightMapTexture"].SetValue(lightMap);
                        //lightingShader.Parameters["LightMapTexture"].SetValue(lightMap);
                        //normalMapShader.Parameters["NormalTexture"].SetValue(bgNormalTexture);
                        //Vector2 dir = screenMiddle - (catPosition + new Vector2(catTexture.Width / 2, catTexture.Height / 2)); // / gameScreenResolution;
                        //dir.Normalize();
                        //Vector3 lightDir = new Vector3(dir.X, -dir.Y, 0.05f);
                        //normalMapShader.Parameters["LightDirection"].SetValue(lightDir);


                        //backgroundNormalMapperSpriteBatch.Begin(effect: normalMapShader);
                        //backgroundNormalMapperSpriteBatch.Draw(bgTexture, Vector2.Zero, Color.White);
                        //backgroundNormalMapperSpriteBatch.End();


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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            backgroudSpriteBatch.Begin(effect: lightingShader);
            spriteBatch.Begin();

            //spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            backgroudSpriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);

            //draw the ground
            //groundSprite.Draw(GraphicsDevice, spriteBatch);
            groundSprite.Draw(GraphicsDevice, spriteBatch);

            foreach (var sprite in Level)
            {
                sprite.Draw(GraphicsDevice, spriteBatch);
            }



            playerSprite.Draw(GraphicsDevice, spriteBatch);
            spriteBatch.DrawString(HUDFont, collisionText, new Vector2(700, 0), Color.Red);

            backgroudSpriteBatch.End();
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
