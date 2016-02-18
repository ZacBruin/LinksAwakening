using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace GPOneButton
{
    class Link : Sprite
    {
        public Texture2D[] spriteArray;
        public Rectangle HitBox;

        public int HBThoriz, HBTvert;
        int AnimationCounter, AnimationHalf, AnimationMax, RecoveryCounter, RecoveryMax;

        public bool HasKey, PlayerHasBeatenGame;

        SoundEffect GetsHurt;

        public int NumberOfHearts;
        public List<Heart> Health;

        public Vector2 GameStartPos;

        public Room room;

        enum DirectionFace { Up, Right, Down, Left };
        DirectionFace value;

        enum State {Normal, Recovering, Dead, Win}
        State CurrentState;

        public Link(Game game, Room r) : base(game)
        {
            this.room = r;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            NumberOfHearts = 3;
            Health = new List<Heart>();

            spriteArray = new Texture2D[8];

            SetSpriteArray();

            GetsHurt = content.Load<SoundEffect>("Audio/LinkTakeDamage");

            spriteTexture = spriteArray[0];

            moveDirection = new Vector2(1, 0);
            GameStartPos = new Vector2(300, 220);
            Position = GameStartPos;

            color = Color.White;     
            
            HasKey = false; PlayerHasBeatenGame = false;

            Speed = 385f;
            Rotate = 0.0f;
            Scale = 0.25f;
            
            AnimationCounter = 0; AnimationHalf = 9; AnimationMax = 17;
            RecoveryCounter = 0; RecoveryMax = 70;
            value = DirectionFace.Down;

            HBThoriz = 12; HBTvert = 6;

            HitBox = new Rectangle(((int)(this.Position.X - ((this.spriteTexture.Width / 2) * this.Scale) + HBThoriz / 2)),
                    ((int)((this.Position.Y - (this.spriteTexture.Height / 2) * this.Scale) + HBTvert*2)),
                    ((int)(this.spriteTexture.Width * this.Scale) - HBThoriz), (int)(this.spriteTexture.Height * this.Scale) - HBTvert*2);
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState gamePad1State = GamePad.GetState(PlayerIndex.One);
            UpdateLink(lastUpdateTime);
            base.Update(gameTime);
        }

        public float GetSpriteTextureWidth()
        {
            return this.spriteTexture.Width * this.Scale;
        }

        public float GetSpriteTextureHeight()
        {
            return this.spriteTexture.Height * this.Scale;
        }

        private void SetSpriteArray()
        {
            spriteArray[0] = content.Load<Texture2D>("LinkSprites/LinkFront"); 
            spriteArray[1] = content.Load<Texture2D>("LinkSprites/LinkFrontRight");
            spriteArray[2] = content.Load<Texture2D>("LinkSprites/LinkSide");
            spriteArray[3] = content.Load<Texture2D>("LinkSprites/LinkSideAlt");
            spriteArray[4] = content.Load<Texture2D>("LinkSprites/LinkBack"); 
            spriteArray[5] = content.Load<Texture2D>("LinkSprites/LinkBackAlt");
            spriteArray[6] = content.Load<Texture2D>("LinkSprites/LinkGet"); 
            spriteArray[7] = content.Load<Texture2D>("LinkSprites/LinkDead");
        }

        public void UpdateHitBox()
        {
            this.HitBox.X = ((int)(this.Position.X - ((this.spriteTexture.Width / 2) * this.Scale) + HBThoriz / 2));
            this.HitBox.Y = (int)(this.Position.Y - ((this.spriteTexture.Height / 2) * this.Scale) + HBTvert * 2);
        }

        private void CycleWalkAnimation(int a, int b)
        {
            if (AnimationCounter >= 1 && AnimationCounter < AnimationHalf)
                this.spriteTexture = spriteArray[a];
            if (AnimationCounter >= 9)
                this.spriteTexture = spriteArray[b];
        }

        public void CheckWallCollision(Rectangle r)
        {
            Rectangle intersectCheck;

            if (r.Intersects(this.HitBox))
            {
                intersectCheck = Rectangle.Intersect(r, this.HitBox);

                if (intersectCheck.Height > intersectCheck.Width)
                {
                    if (intersectCheck.Left < this.HitBox.Right && this.Position.X > intersectCheck.Left)
                        this.Position.X = r.Right + ((int)this.HitBox.Width / 2);

                    if (intersectCheck.Right > this.HitBox.Left && this.Position.X < intersectCheck.Right)
                        this.Position.X = r.Left - ((int)this.HitBox.Width / 2);
                }

                else
                {
                    if (intersectCheck.Top < this.HitBox.Bottom && this.Position.Y > intersectCheck.Bottom)
                        this.Position.Y = (r.Bottom + ((int)this.HitBox.Height / 2) - 5);

                    if (intersectCheck.Bottom > this.HitBox.Top && this.Position.Y < intersectCheck.Top)
                        this.Position.Y = r.Top - (int)this.GetSpriteTextureHeight() / 2;
                }
            }
        }

        private void UpdateLink(float lastUpdateTime)
        {
            KeyboardState kbstate = Keyboard.GetState();

            if(this.Health[0].CurrentState == Heart.HeartState.Empty)
            {
                this.CurrentState = State.Dead;
                this.spriteTexture = spriteArray[7];
            }

            if(this.PlayerHasBeatenGame)
            {
                this.CurrentState = State.Win;
                this.spriteTexture = spriteArray[6];
            }

            if (this.room != null)
            {
                foreach (StationarySprite s in this.room.CollisionList)
                {
                    this.CheckWallCollision(s.Col);
                }
            }

            if (AnimationCounter >= AnimationMax)
                AnimationCounter = 0;

            if(this.CurrentState == State.Recovering)
            {
                if(RecoveryCounter == RecoveryMax)
                {
                    RecoveryCounter = 0;
                    this.CurrentState = State.Normal;
                    this.color = Color.White;
                }

                else
                {
                    RecoveryCounter++;

                    if (RecoveryCounter % 4 == 0)
                        this.color = Color.DarkBlue;

                    else if (RecoveryCounter % 9 == 0)
                        this.color = Color.Fuchsia;

                    else
                        this.color = Color.White;
                }               
            }

            Vector2 LinkDir = new Vector2(0, 0);

            #region KeyBoard
            if (this.CurrentState != State.Dead && this.CurrentState != State.Win)
            {
                if (kbstate.IsKeyUp(Keys.Left) && kbstate.IsKeyUp(Keys.Right) && kbstate.IsKeyUp(Keys.Up) && kbstate.IsKeyUp(Keys.Down))
                {
                    AnimationCounter = 0;
                    switch (value)
                    {
                        case DirectionFace.Left: this.spriteTexture = spriteArray[2];
                            break;
                        case DirectionFace.Right: this.spriteTexture = spriteArray[2];
                            this.SpriteEffects = SpriteEffects.FlipHorizontally;
                            break;
                        case DirectionFace.Up: this.spriteTexture = spriteArray[5];
                            break;
                        case DirectionFace.Down: this.spriteTexture = spriteArray[1];
                            break;
                    }
                }

                else
                    AnimationCounter++;

                if (kbstate.IsKeyDown(Keys.Left))
                {
                    LinkDir += new Vector2(-1, 0);
                    this.SpriteEffects = SpriteEffects.None;

                    value = DirectionFace.Left;

                    this.CycleWalkAnimation(3, 2);
                }

                if (kbstate.IsKeyDown(Keys.Right))
                {
                    LinkDir += new Vector2(1, 0);
                    value = DirectionFace.Right;
                    this.spriteTexture = spriteArray[2];
                    this.SpriteEffects = SpriteEffects.FlipHorizontally;

                    CycleWalkAnimation(3, 2);
                }

                if (kbstate.IsKeyDown(Keys.Up))
                {
                    LinkDir += new Vector2(0, -1);
                    value = DirectionFace.Up;
                    this.spriteTexture = spriteArray[4];
                    this.SpriteEffects = SpriteEffects.None;

                    CycleWalkAnimation(5, 4);
                }

                if (kbstate.IsKeyDown(Keys.Down))
                {
                    LinkDir += new Vector2(0, 1);
                    value = DirectionFace.Down;
                    this.spriteTexture = spriteArray[0];
                    this.SpriteEffects = SpriteEffects.None;

                    CycleWalkAnimation(1, 0);
                }

                if (LinkDir.Length() > 0)
                {
                    this.Position += ((Vector2.Normalize(LinkDir) * (lastUpdateTime / 1000)) * this.Speed);
                    this.UpdateHitBox();
                }
            }


            #endregion
        }

        public void AddHeart(Heart h)
        {
            this.Health.Add(h);         
        }

        public void TakeDamage()
        {
            bool DamageDealt = false;
            int i = 1;

            if (this.CurrentState == State.Normal)
            {
                while (!DamageDealt)
                {
                    switch (this.Health[this.NumberOfHearts - i].CurrentState)
                    {
                        case Heart.HeartState.Full:
                        case Heart.HeartState.Half: this.Health[this.NumberOfHearts - i].TakeDamage();
                            this.CurrentState = State.Recovering;
                            DamageDealt = true;
                            break;

                        case Heart.HeartState.Empty: i++;
                            break;
                    }
                }

                GetsHurt.Play();
                this.UpdateHearts();
            }
        }

        private void UpdateHearts()
        {
            foreach (Heart h in this.Health)
                h.UpdateHeartSprite();
        }

        public bool DeathCheck()
        {
            if (this.CurrentState == State.Dead)
                return true;
            else
                return false;
        }

        public void BringBackToLife()
        {
            if (this.CurrentState == State.Dead)
                this.CurrentState = State.Normal;
        }
    }
}
