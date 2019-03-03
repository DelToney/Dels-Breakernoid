using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



public class Ball : GameObject
{

    public Vector2 direction = new Vector2(.707f, -.707f);
    public float speed = 300;
    public bool caught = false;
    public bool destroy = false;
    public int colTimer = 0;
    public Vector2 tempBallPaddleRatio;

    public Ball(Game myGame):
        base(myGame)
    {
        textureName = "Ball";
    }

    public override void Update(float deltaTime)
    {
        if (!caught)
        {
            position += direction * speed * deltaTime;
        }

        base.Update(deltaTime);
    }
}

