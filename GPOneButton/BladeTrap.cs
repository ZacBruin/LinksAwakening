using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;


namespace GPOneButton
{
    class BladeTrap : Sprite
    {
        public Rectangle[] AttackBoxes;
        public Rectangle HitBox, AttackAxis, SmallHitBox, StationaryPosHitBox;

        public Link link;

        SoundEffect AttackSound, StopSound;

        public Vector2 StationaryPos;

        public float AttackBoxGirth, ReturnSpeed, AttackBoxOffSet;

        enum AttackDir { Up, Down, Left, Right}
        public AttackDir CurrentAttackDir
        {
            get
            {
                return currentAttackDir;
            }

            set
            {
                if (currentAttackDir != value)
                {
                    this.Dir = DetermineAttackDir();
                    currentAttackDir = value;
                }
            }
        }
        private AttackDir currentAttackDir;

        enum BladeState { Stationary, Attacking, Returning, Stopped };
        public BladeState CurrentState
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
        private BladeState currentState;

        int StoppedCounter, StoppedCounterMax;

        public BladeTrap(Game1 game, int x, int y, Link link) : base (game)
        {
            this.Pos.X = (float)(x * 40) + 20;
            this.Pos.Y = (float)(y * 40) + 20;
            this.link = link;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteTexture = content.Load<Texture2D>("EnemySprites/BladeTrap");

            AttackBoxes = new Rectangle[4];
            AttackAxis = new Rectangle(0,0,0,0);

            AttackSound = content.Load<SoundEffect>("Audio/BladeTrapAttack");
            StopSound = content.Load<SoundEffect>("Audio/BladeTrapTap");

            Speed = 250f;
            ReturnSpeed = 80f;
            AttackBoxGirth = 70f;
            AttackBoxOffSet = -15f;
            Dir = new Vector2(0, 0);
            Scale = .24f;
            StoppedCounter = 0;
            StoppedCounterMax = 30;

            StationaryPos = Pos;
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
                this.Dir = DetermineAttackDir();
                AttackSound.Play(.7f,0,0);
            }

            if (this.Dir.Length() > 0)
            {
                if(this.CurrentState == BladeState.Attacking)
                    this.Pos += ((Vector2.Normalize(Dir) * (lastUpdateTime / 1000)) * this.Speed);

                if(this.CurrentState == BladeState.Returning)
                    this.Pos += ((Vector2.Normalize(Dir) * (lastUpdateTime / 1000)) * this.ReturnSpeed);

                this.UpdateHitBoxes();
            }

            switch(this.CurrentState)
            {
                case BladeState.Attacking:
                    if (this.ReachedEdgeOfAttackBox())
                    {
                        this.CurrentState = BladeState.Stopped;
                        StopSound.Play();
                        this.Dir = new Vector2(0, 0);
                    }
                    break;

                case BladeState.Stopped:
                    StoppedCounter++;

                    if (StoppedCounter == StoppedCounterMax)
                    {
                        this.CurrentState = BladeState.Returning;
                        StoppedCounter = 0;
                    }
                    break;

                case BladeState.Returning:
                    this.Dir = GetReturnDirection();

                    if (this.SmallHitBox.Intersects(this.StationaryPosHitBox))
                        this.ReturnSpeed = 25f;

                    if (this.CheckIfReturned())
                    {
                        this.AttackAxis = new Rectangle(0, 0, 0, 0);
                        this.Pos = this.StationaryPos;
                        this.Dir = new Vector2(0, 0);
                    }
                    break;
            }

            if (this.CurrentState == BladeState.Returning && this.Pos == this.StationaryPos)
            {
                StoppedCounter++;

                if(StoppedCounter == 15)
                {
                    StoppedCounter = 0;
                    this.CurrentState = BladeState.Stationary;
                }
            }

            CheckLinkHit();
        }

        private void UpdateHitBoxes()
        {
            this.HitBox.X = (int)(this.Pos.X - ((this.spriteTexture.Width / 2) * this.Scale));
            this.HitBox.Y = (int)(this.Pos.Y - ((this.spriteTexture.Height / 2) * this.Scale));

            this.SmallHitBox.X = (int)(this.Pos.X - ((this.spriteTexture.Width / 2) * this.Scale) + 16);
            this.SmallHitBox.Y = (int)((this.Pos.Y - (this.spriteTexture.Height / 2) * this.Scale) + 16);
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
                    DetermineAttackDir(i);
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
                    if (this.Pos.Y <= AttackBoxes[0].Top) return true;
                    break;

                case BladeTrap.AttackDir.Down:
                    if (this.Pos.Y >= AttackBoxes[2].Bottom) return true;
                    break;

                case BladeTrap.AttackDir.Left:
                    if (this.Pos.X <= AttackBoxes[3].Left) return true;
                    break;

                case BladeTrap.AttackDir.Right:
                    if (this.Pos.X >= AttackBoxes[1].Right) return true;
                    break;

            }

            return false;
        }

        private Vector2 GetReturnDirection()
        {
            return this.Dir * -1;
        }

        private bool CheckIfReturned()
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

        public Vector2 DetermineAttackDir()
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

        //TODO: Implement innards of this OnStateChange function
        public BladeState OnStateChange(BladeTrap.BladeState state)
        {
            switch(state)
            {
                case BladeTrap.BladeState.Attacking:
                    break;

                case BladeTrap.BladeState.Returning:
                    break;

                case BladeTrap.BladeState.Stationary:
                    break;

                case BladeTrap.BladeState.Stopped:
                    break;
            }

            return state;
        }

        private void SetHitBoxes()
        {
            HitBox = new Rectangle((int)(this.Pos.X - ((this.spriteTexture.Width / 2) * this.Scale)),
                (int)((this.Pos.Y - (this.spriteTexture.Height / 2) * this.Scale)), (int)(this.spriteTexture.Width * this.Scale),
                (int)(this.spriteTexture.Height * this.Scale));

            SmallHitBox = new Rectangle((int)(this.Pos.X - ((this.spriteTexture.Width / 2) * this.Scale) + 16),
                (int)((this.Pos.Y - (this.spriteTexture.Height / 2) * this.Scale) + 16), 10, 10);

            StationaryPosHitBox = new Rectangle((int)(this.StationaryPos.X - 5), (int)(this.StationaryPos.Y - 5), 10, 10);
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
