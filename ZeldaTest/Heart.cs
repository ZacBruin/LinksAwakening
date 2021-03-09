using Microsoft.Xna.Framework.Graphics;

namespace GPOneButton
{
    class Heart : Sprite
    {
        public Texture2D Full, Half, Empty;
        public enum HeartState
        {
            Full,
            Half,
            Empty
        };

        public HeartState CurrentState
        {
            get
            {
                return currentState;
            }

            set 
            { 
                if(currentState != value)
                {
                    UpdateHeartSprite();
                    currentState = value;                  
                }
            }
        }
        private HeartState currentState;

        public Heart(Game1 game) : base(game)
        {
            Full = content.Load<Texture2D>("UISprites/FullHeart");
            Half = content.Load<Texture2D>("UISprites/HalfHeart");
            Empty = content.Load<Texture2D>("UISprites/EmptyHeart");

            CurrentState = HeartState.Full;
            spriteTexture = Full;
            Scale = .25f;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void UpdateHeartSprite()
        {         
            switch (CurrentState)
            {
                case HeartState.Full: 
                    spriteTexture = Full;
                    break;

                case HeartState.Half: 
                    spriteTexture = Half;
                    break;

                case HeartState.Empty: 
                    spriteTexture = Empty;
                    break;
            }
        }

        public void TakeDamage()
        {
            switch (CurrentState)
            {
                case HeartState.Full: 
                    CurrentState = HeartState.Half;
                    break;

                case HeartState.Half: 
                    CurrentState = HeartState.Empty;
                    break;

                case HeartState.Empty:
                    break;
            }
        }

        public Texture2D GetSpriteTexture()
        {
            return spriteTexture;
        }

    }
}
