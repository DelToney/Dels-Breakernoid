using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



public class Block : GameObject
{
    public enum BlockColor
    {
        Red = 0,
        Yellow,
        Blue,
        Green,
        Purple,
        GreyHi,
        Grey
    }

    public Block(BlockColor color, Game myGame):
        base(myGame)
    {
        if (color == BlockColor.Red)
        {
            textureName = "block_red";
        }
        if (color == BlockColor.Yellow)
        {
            textureName = "block_yellow";
        }
        if (color == BlockColor.Blue)
        {
            textureName = "block_blue";
        }
        if (color == BlockColor.Green)
        {
            textureName = "block_green";
        }
        if (color == BlockColor.Purple)
        {
            textureName = "block_purple";
        }
        if (color == BlockColor.GreyHi)
        {
            textureName = "block_grey_hi";
        }
        if (color == BlockColor.Grey)
        {
            textureName = "block_grey";
        }

    }

    public override void Update(float deltaTime)
    {

    }

}

