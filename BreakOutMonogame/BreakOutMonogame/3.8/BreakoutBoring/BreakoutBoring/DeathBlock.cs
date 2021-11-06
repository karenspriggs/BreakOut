using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace BreakoutBoring
{
    class DeathBlock: MonogameBlock
    {
        GameEnd ge;

        public DeathBlock(Game game, GameEnd ge): base(game)
        {
            this.ge = ge;
        }

        protected override void LoadContent()
        {
            NormalTextureName = "block_greenWaveSmall";
            HitTextureName = "block_red";
            base.LoadContent();
        }

        protected override void updateBlockTexture()
        {
            switch (block.BlockState)
            {
                case BlockState.Normal:
                    this.Visible = true;
                    this.spriteTexture = NormalTexture;
                    break;
                case BlockState.Hit:
                    this.spriteTexture = HitTexture;
                    break;
                case BlockState.Broken:
                    this.spriteTexture = NormalTexture;
                    //this.enabled = false;
                    this.Visible = false; //don't show block
                    ge.Lose();
                    break;
            }
        }
    }
}
