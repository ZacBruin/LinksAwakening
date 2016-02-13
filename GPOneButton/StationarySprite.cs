using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GPOneButton
{
     class StationarySprite : Sprite
    {
        public Rectangle Col;
        public Texture2D[] spriteArray;
        int SpriteNum;

        public StationarySprite(Game game, float rotate, SpriteEffects spriteEffect, int spriteNum) : base(game)
        {
            this.Rotate = rotate;
            this.SpriteEffects = spriteEffect;
            this.SpriteNum = spriteNum;

            spriteArray = new Texture2D[8];

            //HACK: This is a STUPID way to do this.
            switch (this.SpriteNum)
            {
                case 0: spriteArray[0] = content.Load<Texture2D>("EnvironmentSprites/WallFace1");
                    break;
                case 1: spriteArray[1] = content.Load<Texture2D>("EnvironmentSprites/WallInCorner");
                    break;
                case 2: spriteArray[2] = content.Load<Texture2D>("EnvironmentSprites/WallOutCorner");
                    break;
                case 3: spriteArray[3] = content.Load<Texture2D>("EnvironmentSprites/TopOfWall");
                    break;
                case 4: spriteArray[4] = content.Load<Texture2D>("EnvironmentSprites/Rock");
                    break;
                case 5:
                case 6: spriteArray[5] = content.Load<Texture2D>("EnvironmentSprites/TorchBlock1");
                    spriteArray[6] = content.Load<Texture2D>("EnvironmentSprites/TorchBlock2");
                    break;
                case 7: spriteArray[7] = content.Load<Texture2D>("EnvironmentSprites/Door");
                    break;
            }

            spriteTexture = spriteArray[SpriteNum];

            Scale = 0.25f;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.UpdateCol();
        } 

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

/////////////////////////////////////////////////////////////////////////////////////////////////

         public Texture2D GetSpriteTexture()
        {
            return this.spriteTexture;
        }

         public float GetSpriteTextureWidth()
         {
             return this.spriteTexture.Width;
         }

         public float GetSpriteTextureHeight()
         {
             return this.spriteTexture.Height;
         }

         public Vector2 SetPos(Vector2 v)
         {
             this.Position.X = v.X;
             this.Position.Y = v.Y;

             return this.Position;
         }

         public void UpdateCol()
        {
             this.Col = new Rectangle((int)(this.Position.X - ((this.spriteTexture.Width / 2) * this.Scale)),
                 (int)((this.Position.Y - (this.spriteTexture.Height /2) * this.Scale)), (int)(this.spriteTexture.Width * this.Scale),
                 (int)(this.spriteTexture.Height * this.Scale));
        }

         public void TorchAlternate()
        {
            if (this.spriteTexture == spriteArray[5])
                this.spriteTexture = spriteArray[6];

            else
             this.spriteTexture = spriteArray[5];
        }

    }
}
