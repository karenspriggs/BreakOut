﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;

namespace BreakoutBoring
{
    class BlockManager : DrawableGameComponent
    {
        public List<MonogameBlock> Blocks { get; private set; } //List of Blocks the are managed by Block Manager\

        ScoreManager sc;
        Random rand;
        GameEnd ge;
        BlockManagerInput input;
        
        bool continuees;
        int brokencount;
        int blockcount;

        int deathblockamount;
        int deathblockcount;

        //Dependancy on Ball
        Ball ball;

        List<MonogameBlock> blocksToRemove; //list of block to remove probably because they were hit

        /// <summary>
        /// BlockManager hold a list of blocks and handles updating, drawing a block collision
        /// </summary>
        /// <param name="game">Reference to Game</param>
        /// <param name="ball">Refernce to Ball for collision</param>
        public BlockManager(Game game, Ball b, ScoreManager sb, GameEnd ge)
            : base(game)
        {
            this.Blocks = new List<MonogameBlock>();
            this.blocksToRemove = new List<MonogameBlock>();
            this.sc = sb;
            this.ge = ge;
            this.ball = b;
            ge.b = b;
            continuees = true;

            input = new BlockManagerInput(game);
            rand = new Random();
            deathblockamount = 0;
            deathblockcount = 5;
        }

        public override void Initialize()
        {
            LoadLevel();
            base.Initialize();
        }

        /// <summary>
        /// Replacable Method to Load a Level by filling the Blocks List with Blocks
        /// </summary>
        protected virtual void LoadLevel()
        {
            CreateBlockArrayByWidthAndHeight(24, 2, 1);
        }

        /// <summary>
        /// Simple Level lays out multiple levels of blocks
        /// </summary>
        /// <param name="width">Number of blocks wide</param>
        /// <param name="height">Number of blocks high</param>
        /// <param name="margin">space between blocks</param>
        private void CreateBlockArrayByWidthAndHeight(int width, int height, int margin)
        {
            //Create Block Array based on with and hieght
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    MakeBlock(w, h, margin);
                }
            }
            blockcount = Blocks.Count - deathblockcount;
        }
        
        bool reflected; //the ball should only reflect once even if it hits two bricks

        public override void Update(GameTime gameTime)
        {
            this.reflected = false; //only reflect once per update
            UpdateCheckBlocksForCollision(gameTime);
            UpdateBlocks(gameTime);
            UpdateRemoveDisabledBlocks();

            // This is in the blockmanager class since it respawns the level when the player loses, also the block manager class has a ball 
            if (ge.lost)
            {
                if (input.IsEnterPressed(gameTime))
                {
                    TrashWholeLevelLMFAO();
                    
                    MakeNewLevel();
                    ge.lost = false;
                }
            }

            if (ball.lost)
            {
                ball.lost = false;
                ge.Lose();
            }

            if (!continuees)
            {
                MakeNewLevel();
                continuees = true;
            }

            base.Update(gameTime);
        }

        private void UpdateBlocks(GameTime gameTime)
        {
            foreach (var block in Blocks)
            {
                block.Update(gameTime);
            }
        }

        /// <summary>
        /// Removes disabled blocks from list
        /// </summary>
        private void UpdateRemoveDisabledBlocks()
        {
            //remove disabled blocks
            foreach (var block in blocksToRemove)
            {
                Blocks.Remove(block);
                
                brokencount++;
            }

            //check to respawn all blocks
            if (brokencount == blockcount)
            {
                CreateBlockArrayByWidthAndHeight(24, 2, 1);
                brokencount = 0;
                sc.Level++;
                deathblockcount++;
                ball.Speed += ball.addspeed;
            }

            blocksToRemove.Clear();
        }

        private void UpdateCheckBlocksForCollision(GameTime gameTime)
        {
            foreach (MonogameBlock b in Blocks)
            {
                if (b.Enabled) //Only chack active blocks
                {
                    b.Update(gameTime); //Update Block
                    //Ball Collision
                    if (b.Intersects(ball)) //chek rectagle collision between ball and current block 
                    {
                        //hit
                        b.HitByBall(ball);
                        if(b.BlockState == BlockState.Broken)
                            blocksToRemove.Add(b);  //Ball is hit add it to remove list
                        if (!reflected) //only reflect once
                        {
                            ball.Reflect(b);
                            this.reflected = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Block Manager Draws blocks they don't draw themselves
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var block in this.Blocks)
            {
                if(block.Visible)   //respect block visible property
                    block.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        public void MakeBlock(int w, int h, int margin)
        {
            MonogameBlock b;

            if (DetermineRandom() && deathblockamount < deathblockcount)
            {
                b = new DeathBlock(this.Game, sc.ge);
                b.Initialize();
                b.Location = new Vector2(5 + (w * b.SpriteTexture.Width + (w * margin)), 50 + (h * b.SpriteTexture.Height + (h * margin)));
                Blocks.Add(b);
                deathblockamount++;
            } else
            {
                b = new MonogameBlock(this.Game);
                b.Initialize();
                b.Location = new Vector2(5 + (w * b.SpriteTexture.Width + (w * margin)), 50 + (h * b.SpriteTexture.Height + (h * margin)));
                Blocks.Add(b);
            }
        }

        public void TrashWholeLevelLMFAO()
        {
            Blocks.Clear();
            continuees = false;
        }

        public void MakeNewLevel()
        {
            deathblockamount = 0;
            CreateBlockArrayByWidthAndHeight(24, 2, 1);
            brokencount = 0;
            ball.Speed = ball.previousspeed;
            ge.result = "";
            sc.Lives = 4;
            sc.Score = 0;
            sc.Level = 1;
            ball.Speed = ball.previousspeed;
            ball.Location = ball.startinglocation;
            sc.SetupNewGame();
        }

        public bool DetermineRandom()
        {
            int numb = rand.Next(0, 5);

            if (numb == 1)
            {
                return true;
            }

            return false;
        }
    }
}
