using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;


namespace BreakoutBoring
{
    public class ScoreManager : DrawableGameComponent
    {
        SpriteFont font;
        public int Lives;    
        public int Level;
        public int Score;
        int offsetX;
        int offsetY;

        Texture2D paddle;   //Texture for drawing lives left scoremanager is also the GUI/HUD

        SpriteBatch sb;
        Vector2 scoreLoc, livesLoc, levelLoc, rulesLoc; //Locations to draw GUI elements

        public GameEnd ge;

        public ScoreManager(Game game, GameEnd ge)
            : base(game)
        {
            SetupNewGame();
            this.ge = ge;
            offsetX = 100;
            offsetY = 80;
        }

        public void SetupNewGame()  //Generally mixing static and non static methods is messy be careful
        {
            Lives = 3;
            Level = 1;
            Score = 0;
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(this.Game.GraphicsDevice);
            font = this.Game.Content.Load<SpriteFont>("Arial");
            paddle = this.Game.Content.Load<Texture2D>("paddleSmall");
            livesLoc = new Vector2(this.Game.GraphicsDevice.Viewport.Width / 25, this.Game.GraphicsDevice.Viewport.Height / 25); //Hard coded locations TODO fix for locations relative to window size
            levelLoc = new Vector2(this.Game.GraphicsDevice.Viewport.Height / 2+50, this.Game.GraphicsDevice.Viewport.Height / 25);
            scoreLoc = new Vector2(this.Game.GraphicsDevice.Viewport.Height / 2 + 400, this.Game.GraphicsDevice.Viewport.Height / 25);
            rulesLoc = new Vector2(this.Game.GraphicsDevice.Viewport.Width / 2 - offsetX, this.Game.GraphicsDevice.Viewport.Height / 2 - offsetY);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            for (int i = 0; i < Lives; i++)
            {
                sb.Draw(paddle, new Rectangle((65 * i) + 100, 15, paddle.Width / 2, paddle.Height / 2), Color.White);
            }
            sb.DrawString(font, "Lives: " + Lives, livesLoc, Color.White);
            sb.DrawString(font, "Score: " + Score, scoreLoc, Color.White);
            sb.DrawString(font, "Level: " + Level, levelLoc, Color.White);
            sb.DrawString(font, "Press space to shoot ball\nPress x to turn on easy mode\nIf you kill a green block you lose", rulesLoc, Color.White);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
