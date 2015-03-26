using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GPOneButton
{
    class Sprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Vector2 Pos, Dir;
        public float Speed, Rotate, Scale;
        public SpriteEffects SpriteEffects;

        public Color color;

        protected Texture2D spriteTexture;
        protected ContentManager content;
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected float lastUpdateTime;

        public Sprite(Game game) : base(game)
        {
            content = game.Content;
        }

        public override void Initialize()
        {
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            color = Color.White;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            lastUpdateTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;           
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();           
            
            spriteBatch.Draw(spriteTexture, new Vector2(Pos.X, Pos.Y),
                null, color, Rotate, new Vector2(spriteTexture.Width / 2, spriteTexture.Height / 2), Scale, SpriteEffects, 0);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
