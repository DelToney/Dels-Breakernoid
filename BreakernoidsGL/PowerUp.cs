using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakernoidsGL
{
    class PowerUp : GameObject
    {
        public int speed = 100;
        public enum PowerUpType
        {
            powerup_c = 0, 
            powerup_b,
            powerup_p
        }
        public PowerUpType powerUpType;
        public bool readyToDestroy = false;

        public PowerUp(PowerUpType type, Game myGame):
            base(myGame)
        {
            switch (type)
            {
                case PowerUpType.powerup_c:
                    textureName = "powerup_c";
                    powerUpType = 0;
                    break;
                case PowerUpType.powerup_b:
                    textureName = "powerup_b";
                    powerUpType = (PowerUpType)1;
                    break;
                case PowerUpType.powerup_p:
                    textureName = "powerup_p";
                    powerUpType = (PowerUpType)2;
                    break;
            }
        }

        public override void Update(float deltaTime)
        {
            position.Y += speed * deltaTime;
            base.Update(deltaTime);
        }
    }
}
