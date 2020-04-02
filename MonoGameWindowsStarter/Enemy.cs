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



    public enum EnemyType
    {
        EASY,
        MEDIUM,
        HARD

    }
    public class Enemy: Enemy_Model
    {
        Game7 game;
        public EnemyType enType;
        public Random ran = new Random();
        public Enemy(Game7 game)
        {
            this.game = game;
        }
 
        public void Initialize()
        {
            enemyRect = new Rectangle();
            if (!game.won)
            {
                enemyRect.X = 900;
                enemyRect.Y = ran.Next(175, 535);
                enemyRect.Width = 55;
                enemyRect.Height = 55;
            }

            enType = EnemyType.EASY;

        }

        /// <summary>
        /// Loads content
        /// </summary>
        /// <param name="content">The ContentManager to use</param>
        public void LoadContent(ContentManager content)
        {
            badman = content.Load<Texture2D>("badman");
            badman2 = content.Load<Texture2D>("badman2");
            bounceSFX = content.Load<SoundEffect>("bounce");
        }

        /// <summary>
        /// Updates 
        /// </summary>
        /// <param name="gameTime">The game's GameTime</param>
        public void Update(GameTime gameTime)
        {

            var keyboardState = Keyboard.GetState();
            if (!game.won && !game.lost && enType == EnemyType.EASY) enemyRect.X -= 5;
            if (!game.won && !game.lost && enType == EnemyType.MEDIUM) enemyRect.X -= 7;

            if (enemyRect.X < 0)
            {
                bounceSFX.Play();
                enemyRect.X = 900;

                enemyRect.Y = ran.Next(175, 535);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (!game.won && !game.lost)
            //{}
            if(enemyRect.X < 900 && enType == EnemyType.EASY)
            {

             spriteBatch.Draw(badman, enemyRect, Color.White);
            }

            if (enemyRect.X < 900 && enType == EnemyType.MEDIUM)
            {

                spriteBatch.Draw(badman2, enemyRect, Color.White);
            }


        }
    }

}

