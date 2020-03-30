using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{

    enum State
    {   Idle = 0,
        South = 4,
        East = 1,
        West = 2,
        North = 3,
        
    }



    public class Player
    {
        const int ANIMATION_FRAME_RATE = 124;

        /// <summary>
        /// How quickly the player should move
        /// </summary>
        float PLAYER_SPEED;

        /// <summary>
        /// The width of the animation frames
        /// </summary>
        const int FRAME_WIDTH = 64;

        /// <summary>
        /// The hieght of the animation frames
        /// </summary>
        const int FRAME_HEIGHT = 64;


        Game6 game;

        public SoundEffect hitSFX;

        public SoundEffect winSFX;

        Rectangle manRect;
        
        Texture2D man;
        public Rectangle testmanRec;
        //animation
        Texture2D spriteSheet;
        State state;
        TimeSpan timer;
        int frame;
        public int score;
        // Vector2 position;
        bool isAdded;

        public Player(Game6 game)
        {
            this.game = game;

            timer = new TimeSpan(0);
            // position = new Vector2(50, 400);
            state = State.Idle;
            testmanRec = new Rectangle(50, 400, 75, 75);
            
            
        }

        public void Initialize()
        {
            testmanRec = new Rectangle(50, 400, 75, 75);
            score = 0;
            PLAYER_SPEED = 250;

        }

        /// <summary>
        /// Loads content
        /// </summary>
        /// <param name="content">The ContentManager to use</param>
        public void LoadContent(ContentManager content)
        {
           spriteSheet = game.Content.Load<Texture2D>("spriteSheet");
            man = content.Load<Texture2D>("man2");
            winSFX = content.Load<SoundEffect>("winner");
            hitSFX = content.Load<SoundEffect>("hit");
        }

        /// <summary>
        /// Updates 
        /// </summary>
        /// <param name="gameTime">The game's GameTime</param>
        public void Update(GameTime gameTime, List<Enemy> enemy)
        {

            var keyboardState = Keyboard.GetState();
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;


            
            
            /*movement*/
            // move up down left right
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                state = State.North;
                // manRect.Y -= 5;
                testmanRec.Y -= (int)(delta * PLAYER_SPEED);
                
            }
            else if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                state = State.South;
                // manRect.Y += 5;
                testmanRec.Y += (int)(delta * PLAYER_SPEED);
                
            }

            else if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                state = State.West;
                // manRect.X -= 5;
                testmanRec.X -= (int)(delta * PLAYER_SPEED);
                //game.scoreRect.X -= (int)(delta * PLAYER_SPEED);

                

                //testmanRec.X = (int)position.Y;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                state = State.East;
                // manRect.X += 5;
                testmanRec.X += (int)(delta * PLAYER_SPEED);
              
                //game.scoreRect.X += (int)(delta * PLAYER_SPEED);

                //testmanRec.X = (int)position.Y;
            }
            else
            {
                state = State.Idle;
            }


            if (state != State.Idle) timer += gameTime.ElapsedGameTime;

            // Determine the frame should increase.  Using a while 
            // loop will accomodate the possiblity the animation should 
            // advance more than one frame.
            while (timer.TotalMilliseconds > ANIMATION_FRAME_RATE)
            {
                // increase by one frame
                frame++;
                // reduce the timer by one frame duration
                timer -= new TimeSpan(0, 0, 0, 0, ANIMATION_FRAME_RATE);
            }

            // Keep the frame within bounds (there are four frames)
            frame %= 1;


            /*bounds*/
            // keeps man between the two black bars
            if (testmanRec.Y < 200)
            {
                //position.Y = 345;
                testmanRec.Y = 200;
            }

            if (testmanRec.Y > 600 - testmanRec.Height)
            {
                //position.Y = 600 - testmanRec.Height;
                testmanRec.Y = 600 - testmanRec.Height;
            }

            //keeps man on screen sides
            if (testmanRec.X < 0)
            {
                //position.X = 0;
                testmanRec.X = 0;
            }

            if (testmanRec.X > 1925)
            {
                
                testmanRec.X = 1925;
            }


            foreach (Enemy n in enemy)

            {
                var badmanRect = n.enemyRect;
                if (testmanRec.Intersects(n.enemyRect))
                {
                    hitSFX.Play();
                    game.lost = true;
                    game.won = false;
                    n.enemyRect.X = 920;
                    n.enemyRect.Y = n.ran.Next(400, 535);
                    badmanRect.X -= 5;
                    //position.X = 50;
                    testmanRec.X = 50;
                }

                if (testmanRec.Intersects(game.finishRect))
                {
                    winSFX.Play();
                    //game.won = true;
           
                    score++;
                    PLAYER_SPEED += 100;
                    game.scoreRect = new Vector2(725, 200);
                    n.enemyRect.X = 920;
                    n.enemyRect.Y = n.ran.Next(400, 535);
                    //badmanRect.X = 825;
                    //badmanRect.X -= 5;
                    // position.X = 50;
                    testmanRec.X = 50;
                    if (score % 2 == 0 && score != 0)
                    {

                        isAdded = true;
                        n.enType = EnemyType.MEDIUM;

                    }
                }

                if (score == 5)
                {
                    game.won = true;
                    game.lost = false;
                    n.enemyRect.X = 920;
                    n.enemyRect.Y = n.ran.Next(400, 535);
                    testmanRec.X = 50;
                }
            }

            
            if (isAdded)
            {
                var enemyA = new Enemy(game);
                enemy.Add(enemyA);
                enemyA.Initialize();
               
                isAdded = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(man, manRect, Color.White);

            var source = new Rectangle(
                frame * FRAME_WIDTH, // X value 
                (int)state % 5 * FRAME_HEIGHT, // Y value
                FRAME_WIDTH, // Width 
                FRAME_HEIGHT // Height
                );

            // render the sprite
            spriteBatch.Draw(spriteSheet, new Vector2(testmanRec.X, testmanRec.Y), source, Color.White);
        }
    }
}
