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

            GetsHurt = content.Load<SoundEffect>("Audio/LA_Link_Hurt");

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

            HitBox = new Rectangle(((int)(Position.X - ((spriteTexture.Width / 2) * Scale) + HBThoriz / 2)),
                    ((int)((Position.Y - (spriteTexture.Height / 2) * Scale) + HBTvert*2)),
                    ((int)(spriteTexture.Width * Scale) - HBThoriz), (int)(spriteTexture.Height * Scale) - HBTvert*2);
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState gamePad1State = GamePad.GetState(PlayerIndex.One);
            UpdateLink(lastUpdateTime);
            base.Update(gameTime);
        }

        public float GetSpriteTextureWidth()
        {
            return spriteTexture.Width * Scale;
        }

        public float GetSpriteTextureHeight()
        {
            return spriteTexture.Height * Scale;
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
            HitBox.X = ((int)(Position.X - ((spriteTexture.Width / 2) * Scale) + HBThoriz / 2));
            HitBox.Y = (int)(Position.Y - ((spriteTexture.Height / 2) * Scale) + HBTvert * 2);
        }

        private void CycleWalkAnimation(int a, int b)
        {
            if (AnimationCounter >= 1 && AnimationCounter < AnimationHalf)
                spriteTexture = spriteArray[a];
            if (AnimationCounter >= 9)
                spriteTexture = spriteArray[b];
        }

        public void CheckWallCollision(Rectangle r)
        {
            Rectangle intersectCheck;

            if (r.Intersects(HitBox))
            {
                intersectCheck = Rectangle.Intersect(r, HitBox);

                if (intersectCheck.Height > intersectCheck.Width)
                {
                    if (intersectCheck.Left < HitBox.Right && Position.X > intersectCheck.Left)
                        Position.X = r.Right + (HitBox.Width / 2);

                    if (intersectCheck.Right > HitBox.Left && Position.X < intersectCheck.Right)
                        Position.X = r.Left - (HitBox.Width / 2);
                }

                else
                {
                    if (intersectCheck.Top < HitBox.Bottom && Position.Y > intersectCheck.Bottom)
                        Position.Y = (r.Bottom + (HitBox.Height / 2) - 5);

                    if (intersectCheck.Bottom > HitBox.Top && Position.Y < intersectCheck.Top)
                        Position.Y = r.Top - (int)GetSpriteTextureHeight() / 2;
                }
            }
        }

        private void UpdateLink(float lastUpdateTime)
        {
            KeyboardState kbstate = Keyboard.GetState();

            if(Health[0].CurrentState == Heart.HeartState.Empty)
            {
                CurrentState = State.Dead;
                spriteTexture = spriteArray[7];
            }

            if(PlayerHasBeatenGame)
            {
                CurrentState = State.Win;
                spriteTexture = spriteArray[6];
            }

            if (room != null)
            {
                foreach (StationarySprite s in room.CollisionList)
                    CheckWallCollision(s.Col);
            }

            if (AnimationCounter >= AnimationMax)
                AnimationCounter = 0;

            if(CurrentState == State.Recovering)
            {
                if(RecoveryCounter == RecoveryMax)
                {
                    RecoveryCounter = 0;
                    CurrentState = State.Normal;
                    color = Color.White;
                }

                else
                {
                    RecoveryCounter++;

                    if (RecoveryCounter % 4 == 0)
                        color = Color.DarkBlue;

                    else if (RecoveryCounter % 9 == 0)
                        color = Color.Fuchsia;

                    else
                        color = Color.White;
                }               
            }

            Vector2 LinkDir = new Vector2(0, 0);

            #region KeyBoard
            if (CurrentState != State.Dead && CurrentState != State.Win)
            {
                if (kbstate.IsKeyUp(Keys.Left) && kbstate.IsKeyUp(Keys.Right) && kbstate.IsKeyUp(Keys.Up) && kbstate.IsKeyUp(Keys.Down))
                {
                    AnimationCounter = 0;
                    switch (value)
                    {
                        case DirectionFace.Left: spriteTexture = spriteArray[2];
                            break;
                        case DirectionFace.Right: spriteTexture = spriteArray[2];
                            SpriteEffects = SpriteEffects.FlipHorizontally;
                            break;
                        case DirectionFace.Up: spriteTexture = spriteArray[5];
                            break;
                        case DirectionFace.Down: spriteTexture = spriteArray[1];
                            break;
                    }
                }

                else
                    AnimationCounter++;

                if (kbstate.IsKeyDown(Keys.Left))
                {
                    LinkDir += new Vector2(-1, 0);
                    SpriteEffects = SpriteEffects.None;

                    value = DirectionFace.Left;

                    CycleWalkAnimation(3, 2);
                }

                if (kbstate.IsKeyDown(Keys.Right))
                {
                    LinkDir += new Vector2(1, 0);
                    value = DirectionFace.Right;
                    spriteTexture = spriteArray[2];
                    SpriteEffects = SpriteEffects.FlipHorizontally;

                    CycleWalkAnimation(3, 2);
                }

                if (kbstate.IsKeyDown(Keys.Up))
                {
                    LinkDir += new Vector2(0, -1);
                    value = DirectionFace.Up;
                    spriteTexture = spriteArray[4];
                    SpriteEffects = SpriteEffects.None;

                    CycleWalkAnimation(5, 4);
                }

                if (kbstate.IsKeyDown(Keys.Down))
                {
                    LinkDir += new Vector2(0, 1);
                    value = DirectionFace.Down;
                    spriteTexture = spriteArray[0];
                    SpriteEffects = SpriteEffects.None;

                    CycleWalkAnimation(1, 0);
                }

                if (LinkDir.Length() > 0)
                {
                    Position += ((Vector2.Normalize(LinkDir) * (lastUpdateTime / 1000)) * this.Speed);
                    UpdateHitBox();
                }
            }


            #endregion
        }

        public void AddHeart(Heart h)
        {
            Health.Add(h);         
        }

        public void TakeDamage()
        {
            bool DamageDealt = false;
            int i = 1;

            if (CurrentState == State.Normal)
            {
                while (!DamageDealt)
                {
                    switch (Health[NumberOfHearts - i].CurrentState)
                    {
                        case Heart.HeartState.Full:
                        case Heart.HeartState.Half:
                            Health[NumberOfHearts - i].TakeDamage();
                            CurrentState = State.Recovering;
                            DamageDealt = true;
                            break;

                        case Heart.HeartState.Empty: i++;
                            break;
                    }
                }

                GetsHurt.Play();
                UpdateHearts();
            }
        }

        private void UpdateHearts()
        {
            foreach (Heart h in Health)
                h.UpdateHeartSprite();
        }

        public bool DeathCheck()
        {
            if (CurrentState == State.Dead)
                return true;
            else
                return false;
        }

        public void BringBackToLife()
        {
            if (CurrentState == State.Dead)
                CurrentState = State.Normal;
        }
    }
}
