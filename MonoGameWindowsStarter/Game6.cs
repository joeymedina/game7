using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game6 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Texture2D texture;

        Player player;
        Enemy enemy;
        List<Enemy> enemies = new List<Enemy>();
        Texture2D win;
        Texture2D lose;
        public Vector2 scoreRect;
        int score;

        public Rectangle finishRect;
        Texture2D finish;

        public bool won;
        public bool lost;
        ParticleSystem particleSystem;
        Texture2D particleTexture;
        Texture2D rain;
        ParticleSystem finishSystem;
        Texture2D finishTexture;
        ParticleSystem rainParticle;
        Random random = new Random();

        public Game6()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            player = new Player(this);
            enemy = new Enemy(this);
            enemies.Add(enemy);
            //enemies[0] = enemy;
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
            graphics.PreferredBackBufferWidth = 1042;
            graphics.PreferredBackBufferHeight = 768;

            /**Enemy Class**/
            //ran = new Random();

            graphics.ApplyChanges();
            player.Initialize();
            scoreRect = new Vector2(850, 200);
            score = 0;

            foreach (Enemy en in enemies)
            {
                en.Initialize();
            }



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



            player.LoadContent(Content);

            //foreach (Enemy en in enemies)
            //{
            //    en.LoadContent(Content);
            //}



            texture = Content.Load<Texture2D>("pixel");
            finish = Content.Load<Texture2D>("finish");

            win = Content.Load<Texture2D>("win");
            lose = Content.Load<Texture2D>("lose");


            finishRect = new Rectangle(900, 350, 100, 250);

            // TODO: use this.Content to load your game content here

            //PLAYERTAIL
            particleTexture = Content.Load<Texture2D>("diamond");
            particleSystem = new ParticleSystem(this.GraphicsDevice, 1000, particleTexture);

            finishTexture = Content.Load<Texture2D>("finishparticle");
            finishSystem = new ParticleSystem(this.GraphicsDevice, 1000, finishTexture);

            // Set the SpawnParticle method
            particleSystem.SpawnParticle = (ref Particle particle) =>
            {
                //MouseState mouse = Mouse.GetState();
                particle.Position = new Vector2(player.testmanRec.X + 10, player.testmanRec.Y + 25);
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-50, 50, (float)random.NextDouble()), // X between -50 and 50
                    MathHelper.Lerp(0, 100, (float)random.NextDouble()) // Y between 0 and 100
                    );
                particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.Purple;
                particle.Scale = 1f;
                particle.Life = 1.0f;
            };
            // Set the UpdateParticle method
            particleSystem.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };
            particleSystem.SpawnPerFrame = 6;

            // SMOKE STACK
            finishSystem.SpawnParticle = (ref Particle particle) =>
            {

                particle.Position = new Vector2(950, 350);
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-50, 50, (float)random.NextDouble()), // X between -50 and 50
                    MathHelper.Lerp(0, 100, (float)random.NextDouble()) // Y between 0 and 100
                    );
                particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.DarkCyan;
                particle.Scale = 5f;
                // particle.Life = 3.0f;
                particle.Life = 3.0f;


            };

            finishSystem.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position -= deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };
            finishSystem.SpawnPerFrame = 4;

            rain = Content.Load<Texture2D>("rain");
            rainParticle = new ParticleSystem(this.GraphicsDevice, 2000, rain);

            // THE SNOW RAIN
            rainParticle.SpawnParticle = (ref Particle particle) =>
            {
                MouseState mouse = Mouse.GetState();
                particle.Position = new Vector2(MathHelper.Lerp(0, 950, (float)random.NextDouble()), 0);
                particle.Velocity = new Vector2(
                    0, MathHelper.Lerp(0, 200, (float)random.NextDouble()) // Y between 0 and 100
            );
                particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.DarkSeaGreen;
                particle.Scale = .2f;
                particle.Life = 100f;
            };


            rainParticle.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Life -= deltaT;
            };
            rainParticle.SpawnPerFrame = 6;



            won = false;
            lost = false;
            spriteFont = Content.Load<SpriteFont>("defaultFont");


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

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
            rainParticle.Update(gameTime);
            particleSystem.Update(gameTime);
            finishSystem.Update(gameTime);
            var keyboardState = Keyboard.GetState();

            score = player.score;



            if (keyboardState.IsKeyDown(Keys.R))
            {
                won = false;
                lost = false;

                Initialize();

                enemies.RemoveRange(1, enemies.Count - 1);

            }
            foreach (Enemy en in enemies)
            {

                en.Update(gameTime);

            }
            player.Update(gameTime, enemies);





            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Orange);


            var keyboardState = Keyboard.GetState();

            //var offset = new Vector2(200, 300) - new Vector2(player.testmanRec.X, player.testmanRec.Y);
            //var t = Matrix.CreateTranslation(offset.X, offset.Y, 0);
            spriteBatch.Begin();
            rainParticle.Draw();
            particleSystem.Draw();
            finishSystem.Draw();
            spriteBatch.Draw(texture, new Rectangle(0, 0, 1942, 200), Color.Black);
            spriteBatch.Draw(texture, new Rectangle(0, 600, 1942, 395), Color.Black);
            spriteBatch.Draw(finish, finishRect, Color.Yellow);
            player.Draw(spriteBatch);

            foreach (Enemy en in enemies)
            {

                en.LoadContent(Content);

                en.Draw(spriteBatch);
            }



            spriteBatch.DrawString(spriteFont, "(0.0)/ GET TO THE FINISH -> ", new Vector2(325, 400), Color.Green);

            spriteBatch.DrawString(spriteFont, "SCORE: " + score, scoreRect, Color.DeepPink);
            if (won && !lost)
            {

                spriteBatch.DrawString(spriteFont, "YOU WIN WOO HOO!!!!!! \n press R to restart", new Vector2(325, 450), Color.Purple);
            }
            if (lost && !won)
            {

                spriteBatch.DrawString(spriteFont, "YOU LOSE BOO HOO!!!!!! \n press R to restart", new Vector2(325, 450), Color.Red);
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
