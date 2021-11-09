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
    public enum BallState {  OnPaddleStart, Playing }

    public class Ball : DrawableSprite
    {
        public BallState State { get; private set; }
        public bool autopaddle;
        public bool lost;

        GameConsole console;
        ScoreManager sc;

        int offsetX;
        int offsetY;
        int speed;
        public int addspeed;
        public int previousspeed;

        public Vector2 startinglocation;

        Vector2 LaunchDirection = new Vector2(1, -1);
        
        public Ball(Game game, ScoreManager sc)
            : base(game)
        {
            this.State = BallState.OnPaddleStart;
            this.offsetX = 80;
            this.offsetY = 200;
            this.speed = 190;
            this.addspeed = 50;
            this.previousspeed = speed;
            this.autopaddle = false;
            lost = false;

            this.sc = sc;
            this.startinglocation = new Vector2();
            //Lazy load GameConsole
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console let's add a new one
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }
#if DEBUG
            this.ShowMarkers = true;
#endif
        }

        public void SetInitialLocation()
        {
            this.Location = new Vector2(this.Game.GraphicsDevice.Viewport.Width / 2 - offsetX, this.Game.GraphicsDevice.Viewport.Height / 2+ offsetY);
            this.startinglocation = Location;
        }

        public void LaunchBall(GameTime gameTime)
        {
            this.Speed = speed; 
            this.Direction = LaunchDirection; 
            this.State = BallState.Playing;
            //this.console.GameConsoleWrite("Ball Launched " + gameTime.TotalGameTime.ToString());
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("ballSmall");
            SetInitialLocation();
            base.LoadContent();
        }

        private void resetBall(GameTime gameTime)
        {
            this.Speed = 0;
            this.State =  BallState.OnPaddleStart;
            //this.console.GameConsoleWrite("Ball Reset " + gameTime.TotalGameTime.ToString());
            sc.Lives--;

            if (sc.Lives == 0)
            {
                lost = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            switch(this.State)
            {
                case BallState.OnPaddleStart:
                    break;

                case BallState.Playing:
                    UpdateBall(gameTime);
                    break;
            }
            
            base.Update(gameTime);
        }

        private void UpdateBall(GameTime gameTime)
        {
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            //bounce off wall
            //Left and Right
            if ((this.Location.X + this.spriteTexture.Width > this.Game.GraphicsDevice.Viewport.Width)
                ||
                (this.Location.X < 0))
            {
                this.Direction.X *= -1;
            }

            //bottom Miss
            if (this.Location.Y + this.spriteTexture.Height > this.Game.GraphicsDevice.Viewport.Height)
            {
                this.resetBall(gameTime);
            }

            //Top
            if (this.Location.Y < 0)
            {
                this.Direction.Y *= -1;
            }
        }

        public void Reflect(MonogameBlock block)
        {
            this.Direction.Y *= -1; //TODO check for side collision with block
            sc.Score++;
        }
    }
}
