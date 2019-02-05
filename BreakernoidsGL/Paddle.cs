using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Paddle : GameObject
{
    private float speed = 1000;
    public Paddle(Game myGame):    
        base(myGame)
    {
            textureName = "Paddle";
    }

    public override void Update(float deltaTime)
    {
        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.Left))
        {
            position.X -= speed * deltaTime;
        }
        if (keyState.IsKeyDown(Keys.Right))
        {
            position.X += speed * deltaTime;
        }
        base.Update(deltaTime);
    }
}
