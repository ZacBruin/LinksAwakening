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

        enum States { Stationary, Attacking, Returning, Stopped };
        States CurrentState;

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
            CurrentState = States.Stationary;

            HitBox = new Rectangle((int)(this.Pos.X - ((this.spriteTexture.Width / 2) * this.Scale)),
                 (int)((this.Pos.Y - (this.spriteTexture.Height / 2) * this.Scale)), (int)(this.spriteTexture.Width * this.Scale),
                 (int)(this.spriteTexture.Height * this.Scale));

            SmallHitBox = new Rectangle((int)(this.Pos.X - ((this.spriteTexture.Width / 2) * this.Scale) + 16),
                    (int)((this.Pos.Y - (this.spriteTexture.Height / 2) * this.Scale) + 16), 10, 10);

            StationaryPosHitBox = new Rectangle((int)(this.StationaryPos.X - 5 ), (int)(this.StationaryPos.Y - 5), 10, 10);
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
                this.Attack();
                AttackSound.Play(.7f,0,0);
            }

            if (this.Dir.Length() > 0)
            {
                if(this.CurrentState == States.Attacking)
                    this.Pos += ((Vector2.Normalize(Dir) * (lastUpdateTime / 1000)) * this.Speed);
                if(this.CurrentState == States.Returning)
                    this.Pos += ((Vector2.Normalize(Dir) * (lastUpdateTime / 1000)) * this.ReturnSpeed);
                this.UpdateHitBoxes();
            }

            switch(this.CurrentState)
            {
                case States.Attacking:
                    if (this.CheckForAttackBoxEdge())
                    {
                        this.CurrentState = States.Stopped;
                        StopSound.Play();
                        this.Dir = new Vector2(0, 0);
                    }
                    break;

                case States.Stopped:
                    StoppedCounter++;

                    if (StoppedCounter == StoppedCounterMax)
                    {
                        this.CurrentState = States.Returning;
                        StoppedCounter = 0;
                    }
                    break;

                case States.Returning:
                    this.ReturnToStationaryPos();

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

            if (this.CurrentState == States.Returning && this.Pos == this.StationaryPos)
            {
                StoppedCounter++;

                if(StoppedCounter == 15)
                {
                    StoppedCounter = 0;
                    this.CurrentState = States.Stationary;
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
            foreach (Rectangle r in this.AttackBoxes)
            {
                if (this.link.HitBox.Intersects(r) && this.CurrentState == States.Stationary)
                {
                    this.CurrentState = States.Attacking;
                    this.AttackAxis = r;
                    return true;
                }
            }
            return false;
        }

        private void Attack()
        {
            Rectangle NoAttack = new Rectangle (0,0,0,0);

            if (this.AttackAxis == NoAttack)
                return;

            if (this.AttackAxis == this.AttackBoxes[0])
            {
                this.Dir = new Vector2(0, -1);
                return;
            }

            if (this.AttackAxis == this.AttackBoxes[1])
            {
                this.Dir = new Vector2(1, 0);
                return;
            }

            if (this.AttackAxis == this.AttackBoxes[2])
            {
                this.Dir = new Vector2(0, 1);
                return;
            }

            if (this.AttackAxis == this.AttackBoxes[3])
            {
                this.Dir = new Vector2(-1, 0);
                return;
            }
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

        private bool CheckForAttackBoxEdge()
        {
            if (this.AttackAxis == this.AttackBoxes[0])
            {
                if (this.Pos.Y <= this.AttackAxis.Top)
                    return true;
            }

            if (this.AttackAxis == this.AttackBoxes[1])
            {
                if (this.Pos.X >= this.AttackAxis.Right)
                    return true;
            }

            if (this.AttackAxis == this.AttackBoxes[2])
            {
                if (this.Pos.Y >= this.AttackAxis.Bottom)
                    return true;
            }

            if (this.AttackAxis == this.AttackBoxes[3])
            {
                if (this.Pos.X <= this.AttackAxis.Left)
                    return true;
            }

            return false;
        }

        private void ReturnToStationaryPos()
        {
            if (this.AttackAxis == this.AttackBoxes[0])
            {
                this.Dir = new Vector2(0, 1);
                return;
            }

            if (this.AttackAxis == this.AttackBoxes[1])
            {
                this.Dir = new Vector2(-1, 0);
                return;
            }

            if (this.AttackAxis == this.AttackBoxes[2])
            {
                this.Dir = new Vector2(0, -1);
                return;
            }

            if (this.AttackAxis == this.AttackBoxes[3])
            {
                this.Dir = new Vector2(1, 0);
                return;
            }
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
    }
}
