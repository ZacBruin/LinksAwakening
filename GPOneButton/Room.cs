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
        protected ContentManager content;
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        public StationarySprite[,] Collidables;

        public List<Object> CollisionList;
        public List<BladeTrap> RoomEnemies;

        public Vector2 FloorPos;
        public Texture2D Floor;

        public float[,] SpriteRotations;
        public float FloorScale;
        
        public bool IsOnScreen;

        float Pi = MathHelper.Pi;

        int FloorNum;

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
                    GenFirstRoom();
                    break;

                case 2: this.Floor = content.Load<Texture2D>("EnvironmentSprites/Room2Floor");
                    GenSecondRoom();
                    break;

                case 3: this.Floor = content.Load<Texture2D>("EnvironmentSprites/Room3Floor");
                    GenThirdRoom();
                    break;

                case 4: this.Floor = content.Load<Texture2D>("EnvironmentSprites/Room4Floor");
                    GenFourthRoom();
                    break;

                case 5: this.Floor = content.Load<Texture2D>("EnvironmentSprites/Room5Floor");
                    GenFifthRoom();
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

        public void InsertStationarySprite(StationarySprite s, int x, int y, RotationAmt rot)
        {
            this.Collidables[x, y] = s;

            //Determine rotation amount based on rot
            switch(rot)
            {
                case RotationAmt.rot90:
                    this.SpriteRotations[x, y] = Pi / 2;
                    break;

                case RotationAmt.rot180:
                    this.SpriteRotations[x, y] = Pi;
                    break;

                case RotationAmt.rot270:
                    this.SpriteRotations[x, y] = -Pi / 2;
                    break;
            }
        }

        public void InsertStationarySprite(StationarySprite s, int x, int y)
        {
            this.Collidables[x, y] = s;
        }

        public void FillCollisionList()
        {
            foreach (StationarySprite s in this.Collidables)
            {
                if (s != null)
                    this.CollisionList.Add(s);
            }
        }

        private void UpdateCol(StationarySprite s)
        {
            s.Col = new Rectangle((int)(s.Position.X - ((s.GetSpriteTextureWidth() / 2) * s.Scale)),
                (int)((s.Position.Y - (s.GetSpriteTextureHeight() / 2) * s.Scale)), (int)(s.GetSpriteTextureWidth() * s.Scale),
                (int)(s.GetSpriteTextureHeight() * s.Scale));
        }

        private void GenFirstRoom()
        {
            int[,] RockPositions = new int[6, 2] { {2,3}, {2,5}, {2,6}, {3,2}, {3,3}, {4,3} };
            int[,] WallInPositions = new int[5, 3] { {0,0,3}, {0,7,2}, {5,0,0}, {9,7,1}, {9,3,0} }; //also contains rotations
            int[,] WallOutPositions = new int[3, 2] { {5,3}, {6,0}, {9,2} };

            for (int i = 0; i < WallInPositions.GetLength(0); i++)
            {
                if (WallInPositions[i, 2] == 0)
                    this.InsertStationarySprite
                        (new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), WallInPositions[i, 0], WallInPositions[i, 1]);
                else
                    this.InsertStationarySprite
                        (new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), WallInPositions[i, 0], WallInPositions[i, 1], (RotationAmt)WallInPositions[i, 2]);
            }

            for (int i = 0; i < RockPositions.GetLength(0); i++)
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Rock), RockPositions[i, 0], RockPositions[i, 1]);

            for (int i = 0; i < WallOutPositions.GetLength(0); i++)
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallOutCorner), WallOutPositions[i, 0], WallOutPositions[i, 1], (Pi / 2));

            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 5, 1);

            for (int i = 1; i < 7; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 0, i, (-Pi));
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 7, (Pi / 2));

                if (i < 5)
                {
                    this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 0, (-Pi / 2));
                    this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i + 4, 7, (Pi / 2));
                }
            }

            for (int i = 6; i < 9; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i + 1, 0, (-Pi / 2));
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 3, (-Pi / 2));
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 9, i - 2);
            }

        }
        private void GenSecondRoom()
        {
            int[,] RockPositions = new int[10, 2] 
                { {3,6}, {4,6}, {5,1}, {5,6}, {7,1}, {8,6}, {2,1}, {4,1}, {1,6}, {6,6} };

            for (int i = 0; i < RockPositions.GetLength(0); i++)
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Rock), RockPositions[i, 0], RockPositions[i, 1]);

            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallOutCorner), 0, 2, (-Pi / 2));
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), 0, 7, (-Pi));
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), 9, 0);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), 9, 7, (Pi / 2));

            for (int i = 0; i < 9; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 0, (-Pi / 2));
                if (i > 0) this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 7, (Pi / 2));
            }

            for (int i = 3; i < 7; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 0, i, (-Pi));
                if (i > 3) this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 9, i);
                if (i > 4) this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 9, i - 4);
            }
        }
        private void GenThirdRoom()
        {
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Torch1), 6, 1);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Torch1), 6, 6);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Torch1), 8, 3);

            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Torch2), 8, 1);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Torch2), 6, 3);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Torch2), 8, 6);

            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Rock), 1, 2);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Rock), 3, 6);

            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Door), 2, 7, Pi);

            for (int i = 0; i < 2; i++)
            {
                if (i > 0)
                    i += 4;

                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), i, 0, (-Pi / 2));
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), i, 7, (-Pi));
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), i + 4, 0);
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), i + 4, 7, (Pi / 2));
            }


            for (int i = 1; i < 7; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 4, i);
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 9, i);
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 5, i, (Pi));
            }

            for (int i = 1; i < 4; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 0, (-Pi / 2));
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i + 5, 0, (-Pi / 2));
            }

            for (int i = 1; i < 7; i++)
            {
                if (i != 3)
                    this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 0, i, (-Pi));
            }

            for (int i = 1; i < 3; i++)
            {
                if (i > 1)
                    i += 1;
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 7, (Pi / 2));
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i + 5, 7, (Pi / 2));
            }
        }
        private void GenFourthRoom()
        {
            int[,] RockPositions = new int[10, 2] 
                { {1,1}, {1,3}, {1,5}, {3,3}, {6,1}, {6,2}, {6,4}, {7,5}, {8,1}, {8,2} };

            int [,] WallInPositions = new int[4, 3] { {0,0,3}, {0,7,2}, {9,0,0}, {9,7,1} };

            int[,] WallOutPositions = new int[4, 3] { {1,0,0}, {0,3,1}, {8,0,1}, {0,6,0} };

            for (int i = 0; i < RockPositions.GetLength(0); i++)
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Rock), RockPositions[i, 0], RockPositions[i, 1]);

            for (int i = 0; i < WallOutPositions.GetLength(0); i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallOutCorner),
                    WallOutPositions[i, 0], WallOutPositions[i, 1], (RotationAmt)WallOutPositions[i, 2]);
            }

            for (int i = 0; i < WallInPositions.GetLength(0); i++)
            {
                if (WallInPositions[i, 2] == 0)
                    this.InsertStationarySprite
                        (new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), WallInPositions[i, 0], WallInPositions[i, 1]);
                else
                    this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), 
                        WallInPositions[i, 0], WallInPositions[i, 1], (RotationAmt)WallInPositions[i, 2]);
            }

            for (int i = 1; i < 7; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 0, i, Pi);
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 9, i);
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 7, (Pi / 2));

                if (i > 2) this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 0, (-Pi / 2));
            }
        }
        private void GenFifthRoom()
        {
            for (int i = 1; i < 7; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 0, i, Pi);
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 9, i);
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 7, (Pi / 2));
            }

            for (int i = 1; i < 3; i++)
            {
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i, 0, (-Pi / 2));
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i + 6, 0, (-Pi / 2));
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 3, i);
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), 6, i, (-Pi));
                this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallFace), i + 6, 7, (Pi / 2));
            }

            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), 0, 0, (-Pi / 2));
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), 0, 7, Pi);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), 3, 0);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), 6, 0, (-Pi / 2));
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), 9, 0);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallInCorner), 9, 7, (Pi / 2));

            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallOutCorner), 3, 3, (Pi / 2));
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallOutCorner), 4, 0, (Pi / 2));
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallOutCorner), 5, 0);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.WallOutCorner), 6, 3);

            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Torch1), 2, 2);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Torch1), 7, 5);

            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Torch2), 2, 5);
            this.InsertStationarySprite(new StationarySprite(this.Game, EnvirSpriteType.Torch2), 7, 2);
        }

        public void SetOnScreen(bool b)
        {
            this.IsOnScreen = b;
        }
    }
}
