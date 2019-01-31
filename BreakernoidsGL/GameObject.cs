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
public class GameObject
{
    protected string textureName = "";
    protected Texture2D texture;
    protected Game game;
    public Vector2 position = Vector2.Zero;

    public GameObject(GameObject myGame)
    {
        //
        // TODO: Add constructor logic here
        //
        game = myGame;
    }

    public virtual void LoadContent() {

    }



}
