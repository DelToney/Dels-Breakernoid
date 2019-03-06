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

        switch (color)
        {
            case BlockColor.Red:
                textureName = "block_red";
                break;
            case BlockColor.Yellow:
                textureName = "block_yellow";
                break;
            case BlockColor.Blue:
                textureName = "block_blue";
                break;
            case BlockColor.Green:
                textureName = "block_green";
                break;
            case BlockColor.Purple:
                textureName = "block_purple";
                break;
            case BlockColor.GreyHi:
                textureName = "block_grey_hi";
                break;
            case BlockColor.Grey:
                textureName = "block_grey";
                break;
            case (BlockColor)9:
                textureName = "";
                break;
        }
            }

    public override void Update(float deltaTime)
    {

    }

}

