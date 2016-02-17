using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GPOneButton
{
    public enum EnvirSpriteType 
    { 
        WallFace = 0, 
        WallInCorner, 
        WallOutCorner, 
        WallTop, 
        Rock, 
        Torch1, 
        Torch2, 
        Door 
    };

    public enum RotationAmt { rot90=1, rot180, rot270 };

    class StationarySprite : Sprite
    {        
        public Rectangle Col;
        Texture2D altSpriteTorch;

        public StationarySprite(Game game, EnvirSpriteType spriteType) : base(game)
        {
            spriteTexture = DetermineTexture(spriteType);
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

        public Texture2D GetSpriteTexture()
        {
            return this.spriteTexture;
        }

        private Texture2D DetermineTexture(EnvirSpriteType type)
        {
            Texture2D Tex = null;

            switch ((int)type)
            {
                case 0: 
                    Tex = content.Load<Texture2D>("EnvironmentSprites/WallFace1");
                    break;

                case 1: 
                    Tex = content.Load<Texture2D>("EnvironmentSprites/WallInCorner");
                    break;

                case 2: 
                    Tex = content.Load<Texture2D>("EnvironmentSprites/WallOutCorner");
                    break;

                case 3: 
                    Tex = content.Load<Texture2D>("EnvironmentSprites/TopOfWall");
                    break;

                case 4: 
                    Tex = content.Load<Texture2D>("EnvironmentSprites/Rock");
                    break;

                case 5: Tex = content.Load<Texture2D>("EnvironmentSprites/TorchBlock1");
                    altSpriteTorch = content.Load<Texture2D>("EnvironmentSprites/TorchBlock2");
                    break;

                case 6: Tex = content.Load<Texture2D>("EnvironmentSprites/TorchBlock2");
                    altSpriteTorch = content.Load<Texture2D>("EnvironmentSprites/TorchBlock1");
                    break;

                case 7: Tex = content.Load<Texture2D>("EnvironmentSprites/Door");
                    break;
            }

            return Tex;
        }

        public float GetSpriteTextureWidth()
        {
            return this.spriteTexture.Width;
        }

        public float GetSpriteTextureHeight()
        {
            return this.spriteTexture.Height;
        }

        public Vector2 SetPos(Vector2 posVec)
        {
            this.Position.X = posVec.X;
            this.Position.Y = posVec.Y;

            return this.Position;
        }

        public void UpdateCol()
        {
            this.Col = new Rectangle((int)(this.Position.X - ((this.spriteTexture.Width / 2) * this.Scale)),
                (int)((this.Position.Y - (this.spriteTexture.Height / 2) * this.Scale)), (int)(this.spriteTexture.Width * this.Scale),
                (int)(this.spriteTexture.Height * this.Scale));
        }

        public void AnimateTorch()
        {
            Texture2D tempTex = spriteTexture;
            spriteTexture = altSpriteTorch;
            altSpriteTorch = tempTex;
        }

    }
}
