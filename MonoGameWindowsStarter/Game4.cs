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
    public class Game4 : Game
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



        public Game4()
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
            scoreRect = new Vector2(725, 200);
            score = 0;
            
            foreach(Enemy en in enemies)
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
            won = false;
            lost = false;
            spriteFont = Content.Load<SpriteFont>("defaultFont");

            player.LoadContent(Content);

            //foreach (Enemy en in enemies)
            //{
            //    en.LoadContent(Content);
            //}

            

            texture = Content.Load<Texture2D>("pixel");
            finish = Content.Load<Texture2D>("finish");

            win = Content.Load<Texture2D>("win");
            lose = Content.Load<Texture2D>("lose");


            finishRect = new Rectangle(1825, 350, 100, 250);




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
            GraphicsDevice.Clear(Color.AliceBlue);
            var keyboardState = Keyboard.GetState();

            var offset = new Vector2(200, 300) - new Vector2(player.testmanRec.X, player.testmanRec.Y);
            var t = Matrix.CreateTranslation(offset.X, offset.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);


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
