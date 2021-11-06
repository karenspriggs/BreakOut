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
    public enum GameState { Playing, Lost, Won }
    public class GameEnd : DrawableGameComponent
    {
        SpriteBatch sb;
        SpriteFont font;
        public string result;
        Vector2 resultpos;
        int offset;
        public Ball b;

        public bool lost;

        public GameEnd(Game game): base(game)
        {
            offset = 100;
            lost = false;
        }

        protected override void LoadContent()
        {
            result = "";
            sb = new SpriteBatch(this.Game.GraphicsDevice);
            font = this.Game.Content.Load<SpriteFont>("Arial");
            base.LoadContent();
        }

        public override void Initialize()
        {
            resultpos = new Vector2(this.Game.GraphicsDevice.Viewport.Width / 2 - offset, this.Game.GraphicsDevice.Viewport.Height / 2);

            base.Initialize();
        }

        public void Lose()
        {
            b.Speed = 0;
            result = "You lose and suck";
            lost = true;
        }

        public void Win()
        {
            result = "You win lmfao";
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.DrawString(font, result, resultpos, Color.White);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
