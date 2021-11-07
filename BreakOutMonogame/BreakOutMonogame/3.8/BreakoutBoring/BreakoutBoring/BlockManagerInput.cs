using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using Microsoft.Xna.Framework.Input;

namespace BreakoutBoring
{
    public class BlockManagerInput
    {
		InputHandler input;

		public BlockManagerInput(Game game)
		{
			input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
		}
		public bool IsEnterPressed(GameTime gametime)
		{

			if (input.KeyboardState.IsKeyDown(Keys.Enter))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
