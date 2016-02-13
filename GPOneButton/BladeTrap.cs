using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace GPOneButton
{
    class BladeTrap : Sprite
    {
        public Rectangle[] AttackBoxes;
        public Rectangle HitBox, SmallHitBox, StationaryPosHitBox;

        public Link link;

        SoundEffect AttackSound, StopSound;

        Vector2 OriginPosition;

        float AttackBoxGirth, ReturnSpeed, AttackBoxOffSet;
        int StoppedCounter, StoppedCounterMax;

        enum AttackDir { Up, Down, Left, Right}
        AttackDir CurrentAttackDir
        {
            get
            {
                return currentAttackDir;
            }

            set
            {
                if (currentAttackDir != value)
                {
                    this.moveDirection = DetermineAttackDirection();
                    currentAttackDir = value;
                }
            }
        }
        AttackDir currentAttackDir;

        enum BladeState { Stationary, Attacking, Returning, Stopped };
        BladeState CurrentState
        {
            get
            {
                return currentState;
            }

            set
            {
                if(currentState != value)
                    currentState = OnStateChange(value);
            }
        }
        BladeState currentState;

        public BladeTrap(Game1 game, int x, int y, Link link) : base (game)
        {
            this.Position.X = (float)(x * 40) + 20;
            this.Position.Y = (float)(y * 40) + 20;
            this.link = link;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteTexture = content.Load<Texture2D>("EnemySprites/BladeTrap");

            AttackBoxes = new Rectangle[4];

            AttackSound = content.Load<SoundEffect>("Audio/BladeTrapAttack");
            StopSound = content.Load<SoundEffect>("Audio/BladeTrapTap");

            Speed = 250f;
            ReturnSpeed = 80f;
            AttackBoxGirth = 70f;
            AttackBoxOffSet = -15f;
            moveDirection = new Vector2(0, 0);
            Scale = .24f;
            StoppedCounter = 0;
            StoppedCounterMax = 30;

            OriginPosition = Position;
            CurrentState = BladeState.Stationary;

            SetHitBoxes();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateBladeTrap(lastUpdateTime);
            base.Update(gameTime);
        }

        private void UpdateBladeTrap(float lastUpdateTime)
        {
            if (CheckForLink())
            {
                moveDirection = DetermineAttackDirection();
                AttackSound.Play(.7f,0,0);
            }
            
            switch(CurrentState)
            {
                case BladeState.Attacking:
                    if (ReachedEdgeOfAttackBox())
                        CurrentState = BladeState.Stopped;

                    if (moveDirection.Length() > 0)
                    {
                        Position += ((Vector2.Normalize(moveDirection) * (lastUpdateTime / 1000)) * this.Speed);
                        UpdateHitBoxes();
                    }

                    break;

                case BladeState.Stopped:
                    StoppedCounter++;

                    if (StoppedCounter == StoppedCounterMax)
                        CurrentState = BladeState.Returning;
                    break;

                case BladeState.Returning:
                    if (SmallHitBox.Intersects(StationaryPosHitBox))
                        ReturnSpeed = 25f;                

                    if (CheckIfReturnedToOrigin())
                    {
                        Position = OriginPosition;
                        moveDirection = new Vector2(0, 0);
                    }

                    if(Position == OriginPosition)
                    {
                        StoppedCounter++;

                        if (StoppedCounter == 15)
                            CurrentState = BladeState.Stationary;
                    }

                    if (moveDirection.Length() > 0)
                    {
                        Position += ((Vector2.Normalize(moveDirection) * (lastUpdateTime / 1000)) * this.ReturnSpeed);
                        UpdateHitBoxes();
                    }

                    break;
            }

            CheckLinkHit();
        }

        private void UpdateHitBoxes()
        {
            HitBox.X = (int)(Position.X - ((spriteTexture.Width / 2) * Scale));
            HitBox.Y = (int)(Position.Y - ((spriteTexture.Height / 2) * Scale));

            this.SmallHitBox.X = (int)(this.Position.X - ((this.spriteTexture.Width / 2) * this.Scale) + 16);
            this.SmallHitBox.Y = (int)((this.Position.Y - (this.spriteTexture.Height / 2) * this.Scale) + 16);
        }

        //Floats taken in correspond to how many squares the BladeTrap's AttackBoxes will be able to reach
        public void SetAttackBoxes(float UP, float RIGHT, float DOWN, float LEFT)
        {
            this.AttackBoxes[0] =
                new Rectangle((int)(this.HitBox.Left + this.AttackBoxOffSet), (int)(this.HitBox.Top - (40 * UP)),
                (int)this.AttackBoxGirth, (int)(40 * UP));

            this.AttackBoxes[1] =
                new Rectangle((int)(this.HitBox.Right), (int)(this.HitBox.Top + this.AttackBoxOffSet),
                (int)(40 * RIGHT), (int)this.AttackBoxGirth);

            this.AttackBoxes[2] =
                new Rectangle((int)(this.HitBox.Left + this.AttackBoxOffSet), (int)(this.HitBox.Bottom),
                (int)this.AttackBoxGirth, (int)(40 * DOWN));

            this.AttackBoxes[3] =
                new Rectangle((int)(this.HitBox.Left - (40 * LEFT)), (int)(this.HitBox.Top + this.AttackBoxOffSet),
                    (int)(40 * LEFT), (int)this.AttackBoxGirth);
        }

        private bool CheckForLink()
        {
            int i = -1;
            foreach (Rectangle r in this.AttackBoxes)
            {
                i++;
                if (this.link.HitBox.Intersects(r) && this.CurrentState == BladeState.Stationary)
                {
                    this.CurrentState = BladeState.Attacking;
                    currentAttackDir = DetermineAttackDir(i); 
                    return true;
                }
            }
            return false;
        }

        private bool CheckLinkHit()
        {
            if (this.HitBox.Intersects(this.link.HitBox))
            {
                this.link.TakeDamage();
                return true;
            }

            else
                return false;              
        }

        private bool ReachedEdgeOfAttackBox()
        {
            switch(currentAttackDir)
            {
                case BladeTrap.AttackDir.Up:
                    if (this.Position.Y <= AttackBoxes[0].Top) return true;
                    break;

                case BladeTrap.AttackDir.Down:
                    if (this.Position.Y >= AttackBoxes[2].Bottom) return true;
                    break;

                case BladeTrap.AttackDir.Left:
                    if (this.Position.X <= AttackBoxes[3].Left) return true;
                    break;

                case BladeTrap.AttackDir.Right:
                    if (this.Position.X >= AttackBoxes[1].Right) return true;
                    break;
            }

            return false;
        }

        private Vector2 GetReturnDirection()
        {
            return moveDirection * -1;
        }

        private bool CheckIfReturnedToOrigin()
        {
            if (this.HitBox.Top == this.AttackBoxes[0].Bottom && this.HitBox.Left == this.AttackBoxes[3].Right)
            {
                this.ReturnSpeed = 80f;
                return true;
            }

            else
                return false;
        }

        public Texture2D GetSpriteTexture()
        {
            return this.spriteTexture;
        }

        public Vector2 DetermineAttackDirection()
        {
            Vector2 dirVect = Vector2.Zero;
            switch(currentAttackDir)
            {
                case BladeTrap.AttackDir.Up:
                    dirVect = new Vector2(0, -1);
                    break;

                case BladeTrap.AttackDir.Down:
                    dirVect = new Vector2(0, 1);
                    break;

                case BladeTrap.AttackDir.Left:
                    dirVect = new Vector2(-1, 0);
                    break;

                case BladeTrap.AttackDir.Right:
                    dirVect = new Vector2(1, 0);
                    break;
            }
            return dirVect;
        }

        private BladeState OnStateChange(BladeTrap.BladeState state)
        {
            switch(state)
            {
                case BladeTrap.BladeState.Returning:
                    StoppedCounter = 0;
                    moveDirection = GetReturnDirection();
                    break;

                case BladeTrap.BladeState.Stationary:
                    StoppedCounter = 0;
                    break;

                case BladeTrap.BladeState.Stopped:
                    if (state == BladeTrap.BladeState.Attacking)
                    {
                        StopSound.Play();
                        moveDirection = Vector2.Zero;
                    }
                    break;
            }

            return state;
        }

        private void SetHitBoxes()
        {
            HitBox = new Rectangle((int)(this.Position.X - ((this.spriteTexture.Width / 2) * this.Scale)),
                (int)((this.Position.Y - (this.spriteTexture.Height / 2) * this.Scale)), (int)(this.spriteTexture.Width * this.Scale),
                (int)(this.spriteTexture.Height * this.Scale));

            SmallHitBox = new Rectangle((int)(this.Position.X - ((this.spriteTexture.Width / 2) * this.Scale) + 16),
                (int)((this.Position.Y - (this.spriteTexture.Height / 2) * this.Scale) + 16), 10, 10);

            StationaryPosHitBox = new Rectangle((int)(this.OriginPosition.X - 5), (int)(this.OriginPosition.Y - 5), 10, 10);
        }

        private AttackDir DetermineAttackDir(int elementInAttackBoxes)
        {
            switch(elementInAttackBoxes)
            {
                case 0: return AttackDir.Up;
                case 1: return AttackDir.Right;
                case 2: return AttackDir.Down;
                case 3: return AttackDir.Left;

                default: return AttackDir.Up;
            }
        }

    }
}
