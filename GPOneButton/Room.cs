using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GPOneButton
{
    class Room : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public StationarySprite[,] Collidables;
        public float[,] SpriteRotations; 

        public Texture2D Floor;
        public float FloorScale;
        public Vector2 FloorPos;
        public bool IsOnScreen;

        double Pi = MathHelper.Pi;

        public List<Object> CollisionList;
        public List<BladeTrap> RoomEnemies;

        int FloorNum;

        protected ContentManager content;
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        public Room(Game game, int floorNum) : base(game)
        {
            content = game.Content;
            this.FloorNum = floorNum;  
        }

        public override void Initialize()
        {
            graphics = (GraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            CollisionList = new List<Object>();
            RoomEnemies = new List<BladeTrap>();

            IsOnScreen = false;

            Collidables = new StationarySprite[10,8];
            SpriteRotations = new float[10, 8];
            FloorScale = 0.25f;
            FloorPos = new Vector2(0, 0);

            switch (this.FloorNum)
            {
                case 1: this.Floor = content.Load<Texture2D>("EnvironmentSprites/Room1Floor");
                    #region Room1
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 0, 0, (float)(-Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 5, 0, (float)0f);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 0, 7, (float)(-Pi));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 9, 7, (float)(Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 9, 3, (float)0f);

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 2), 6, 0, (float)(Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 2), 9, 2, (float)(Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 2), 5, 3, (float)(Pi / 2));

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 5, 1, (float)0f);

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 2, 5, (float)0f);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 3, 2, (float)0f);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 4, 3, (float)0f);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 3, 3, (float)0f);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 2, 3, (float)0f);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 2, 6, (float)0f);

            for (int i = 1; i < 5; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i, 0, (float)(-Pi / 2));
                this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i, 7, (float)(Pi / 2));
                this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i + 4, 7, (float)(Pi / 2));

                if (i > 1)
                {
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i + 5, 0, (float)(-Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i + 4, 3, (float)(-Pi / 2));
                }

            }
            for (int i = 1; i < 7; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 0, i, (float)(-Pi));
                
                if(i > 3)
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 9, i, (float)0f);
            }
#endregion 
                    break;
                case 2: this.Floor = content.Load<Texture2D>("EnvironmentSprites/Room2Floor");
                    #region Room2

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 9, 0, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 0, 7, (float)(-Pi));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 9, 7, (float)(Pi / 2));

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 2), 0, 2, (float)(-Pi / 2));

                    for (int i = 0; i < 9; i++)
                    {
                      this.InsertStationarySprite(new StationarySprite (this.Game, 0.0f, SpriteEffects.None, 0), i, 0, (float)(-Pi / 2));
                     if (i > 0)
                      {
                         this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i, 7, (float)(Pi / 2));
                      }
                    }       

                  for (int i = 1; i < 3 ; i++)
                  {
                      this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), i*2, 1, (float)0);
                      if(i%2 != 0)
                       {
                           this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), i, 6, (float)0);
                           this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), i+5, 6, (float)0);
                       }
                   }

                  this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 8, 6, (float)0);
                  this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 5, 1, (float)0);
                  this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 7, 1, (float)0);
                  this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 3, 6, (float)0);
                  this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 4, 6, (float)0);
                  this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 5, 6, (float)0);


                 for (int i = 3; i < 7; i++)
                 {
                  this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 0, i, (float)(-Pi));
                        if (i > 3)
                           this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 9, i, (float)0);
                     if (i > 4)
                         this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 9, i-4, (float)0);
                 }

            #endregion
                    break;
                case 3: this.Floor = content.Load<Texture2D>("EnvironmentSprites/Room3Floor");
                    #region Room3

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 5), 6, 1, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 5), 6, 6, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 5), 8, 3, (float)0);

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 6), 8, 1, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 6), 6, 3, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 6), 8, 6, (float)0);

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 1, 2, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 3, 6, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 7), 2, 7, (float)Pi);

                    for (int i = 0; i < 2; i++)
                    {
                        if (i > 0)
                            i += 4;

                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), i, 0, (float)(-Pi / 2));
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), i, 7, (float)(-Pi));
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), i+4, 0, (float)0);
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), i + 4, 7, (float)(Pi / 2));
                    }
            

                    for (int i = 1; i < 7; i++)
                    {
                        this.InsertStationarySprite(new StationarySprite (this.Game, 0.0f, SpriteEffects.None, 0), 4, i, (float)0);
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 9, i, (float)0);
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 5, i, (float)(Pi));
                    }

                    for (int i = 1; i < 4; i++)
                    {
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i, 0, (float)(-Pi / 2));
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i+5, 0, (float)(-Pi / 2));
                    }

                    for (int i = 1; i < 7; i++)
                    {
                        if(i != 3)
                        this.InsertStationarySprite(new StationarySprite (this.Game, 0.0f, SpriteEffects.None, 0), 0, i, (float)(-Pi));
                    }

                    for (int i = 1; i < 3; i++)
                    {
                        if (i > 1)
                            i += 1;
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i, 7, (float)(Pi / 2));
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i+5, 7, (float)(Pi / 2));
                    }

                    #endregion
                    break;
                case 4: this.Floor = content.Load<Texture2D>("EnvironmentSprites/Room4Floor");
                    #region Room4
                    for (int i = 1; i < 7; i++)
                    {
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 0, i, (float)Pi);
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 9, i, (float)0f);
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i, 7, (float)(Pi/2));
                    }

                    for (int i = 3; i < 7; i++)
                    {
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i, 0, (float)(-Pi/2));
                    }

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 0, 0, (float)(-Pi/2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 0, 7, (float)Pi);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 9, 0, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 9, 7, (float)(Pi/2));

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 1, 0, (float)(-Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 8, 0, (float)(-Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 8, 0, (float)(-Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 7, 7, (float)(Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 8, 7, (float)(Pi / 2));

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 6, 1, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 6, 2, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 6, 4, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 1, 3, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 3, 3, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 1, 3, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 1, 5, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 8, 1, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 8, 2, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 7, 5, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 4), 1, 1, (float)0);

            #endregion
                    break;
                case 5: this.Floor = content.Load<Texture2D>("EnvironmentSprites/Room5Floor");
                    #region Room5
                    for (int i = 1; i < 7; i++)
                    {
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 0, i, (float)Pi);
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 9, i, (float)0f);
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i, 7, (float)(Pi / 2));
                    }

                    for (int i = 1; i < 3; i++)
                    {
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i, 0, (float)(-Pi/2));
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i+6, 0, (float)(-Pi / 2));
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 3, i, (float)0);
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), 6, i, (float)(-Pi));
                        this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 0), i+6, 7, (float)(Pi/2));
                    }
            

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 0, 0, (float)(-Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 0, 7, (float)Pi);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 9, 0, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 9, 7, (float)(Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 3, 0, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 1), 6, 0, (float)(-Pi / 2));

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 2), 4, 0, (float)(Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 2), 5, 0, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 2), 3, 3, (float)(Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 2), 6, 3, (float)0);

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 5), 2, 2, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 5), 7, 5, (float)0);

                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 6), 2, 5, (float)0);
                    this.InsertStationarySprite(new StationarySprite(this.Game, 0.0f, SpriteEffects.None, 6), 7, 2, (float)0);

            #endregion
                    break;
            }

            this.FillCollisionList();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (IsOnScreen)
            {
                spriteBatch.Draw(this.Floor, FloorPos,
                    null, Color.White, 0.0f, new Vector2(0, 0), FloorScale, SpriteEffects.None, 0);

                foreach (BladeTrap b in this.RoomEnemies)
                {
                    spriteBatch.Draw(b.GetSpriteTexture(), b.Position,
                        null, Color.White, 0.0f, new Vector2(0, 0), b.Scale, SpriteEffects.None, 0);
                }

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (Collidables[j, i] != null)
                        {
                            spriteBatch.Draw(Collidables[j, i].GetSpriteTexture(), Collidables[j, i].SetPos(new Vector2((40 * j) + 20, ((40 * i) + 20))),
                            null, Color.White, SpriteRotations[j, i],
                            new Vector2(Collidables[j, i].GetSpriteTextureWidth() / 2,
                            Collidables[j, i].GetSpriteTextureHeight() / 2),
                            Collidables[j, i].Scale, Collidables[j, i].SpriteEffects, 0);

                            this.UpdateCol(Collidables[j, i]);
                        }
                    }
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void InsertStationarySprite(StationarySprite s, int x, int y, float rot)
        {
            this.Collidables[x, y] = s;
            this.SpriteRotations[x, y] = rot;
        }

        public void FillCollisionList()
        {
            foreach (StationarySprite s in this.Collidables)
            {
                if (s == null)
                {}

                else 
                {
                    this.CollisionList.Add(s);
                }
            }
        }

        private void UpdateCol(StationarySprite s)
        {
            s.Col = new Rectangle((int)(s.Position.X - ((s.GetSpriteTextureWidth() / 2) * s.Scale)),
                (int)((s.Position.Y - (s.GetSpriteTextureHeight() / 2) * s.Scale)), (int)(s.GetSpriteTextureWidth() * s.Scale),
                (int)(s.GetSpriteTextureHeight() * s.Scale));
        }

        public void SetOnScreen(bool b)
        {
            this.IsOnScreen = b;
        }
    }
}
