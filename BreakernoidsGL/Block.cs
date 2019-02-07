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

    public Block(Game myGame):
        base(myGame)
    {
        textureName = "block_red";
    }

    public override void Update(float deltaTime)
    {

    }

}

