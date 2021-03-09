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

    public enum RotationAmt
    {
        rot90 = 1,
        rot180,
        rot270
    };

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
            UpdateCol();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public Texture2D GetSpriteTexture()
        {
            return spriteTexture;
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
            return spriteTexture.Width;
        }

        public float GetSpriteTextureHeight()
        {
            return spriteTexture.Height;
        }

        public Vector2 SetPos(Vector2 posVec)
        {
            Position.X = posVec.X;
            Position.Y = posVec.Y;

            return Position;
        }

        public void UpdateCol()
        {
            Col = new Rectangle((int)(Position.X - ((spriteTexture.Width / 2) * Scale)),
                (int)((Position.Y - (spriteTexture.Height / 2) * Scale)), (int)(spriteTexture.Width * Scale),
                (int)(spriteTexture.Height * Scale));
        }

        public void AnimateTorch()
        {
            Texture2D tempTex = spriteTexture;
            spriteTexture = altSpriteTorch;
            altSpriteTorch = tempTex;
        }

    }
}
