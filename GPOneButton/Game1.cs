using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GPOneButton
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D CollisionOutline, UI, Key, Chest, Treasure, Credit1, Credit2, Credit3;

        SoundEffect KeyGet, ItemGet, Open, Stairs;
        Song DungeonMusic, Credits;

        bool CreditsHaveStarted, DrawCredit1, DrawCredit2, DrawCredit3;

        int TorchAnimationCounter, Wait;

        Link lonk;
        BladeTrap B, B21, B22, B24, B25, B26, B31, B32, B41, B42, B43, B44;
        Room room1, room2, room3, room4, room5;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 360;

            room1 = new Room(this, 1); room2 = new Room(this, 2); room3 = new Room(this, 3); 
            room4 = new Room(this, 4); room5 = new Room(this, 5);

            lonk = new Link(this, room1);

            CreditsHaveStarted = false; DrawCredit1 = false; DrawCredit2 = false; DrawCredit3 = false;

            B = new BladeTrap(this, 1, 6, lonk);

            this.Components.Add(room1); this.Components.Add(room2); this.Components.Add(room3); 
            this.Components.Add(room4); this.Components.Add(room5);

            this.Components.Add(lonk);

            this.Components.Add(B);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            CollisionOutline = (Content.Load<Texture2D>("1Pixel"));
            UI = (Content.Load<Texture2D>("UISprites/LinkUI"));
            Key = (Content.Load<Texture2D>("EnvironmentSprites/Key"));
            Chest = Content.Load<Texture2D>("EnvironmentSprites/Chest");
            Treasure = Content.Load<Texture2D>("EnvironmentSprites/Pizza");

            Credit1 = Content.Load<Texture2D>("UISprites/Credit1");
            Credit2 = Content.Load<Texture2D>("UISprites/Credit2");
            Credit3 = Content.Load<Texture2D>("UISprites/Credit3");

            DungeonMusic = Content.Load<Song>("Audio/Level2");
            Credits = Content.Load<Song>("Audio/Credits");
            KeyGet = Content.Load<SoundEffect>("Audio/KeyGet");
            ItemGet = Content.Load<SoundEffect>("Audio/ItemGet");
            Open = Content.Load<SoundEffect>("Audio/DoorOpen");
            Stairs = Content.Load<SoundEffect>("Audio/Stairs");

            MediaPlayer.Play(DungeonMusic);
            MediaPlayer.IsRepeating = true;

            B.SetAttackBoxes(5f, 0,0,0);

            TorchAnimationCounter = 0;

            for (int i = 0; i < lonk.NumberOfHearts; i++)
            {
                lonk.AddHeart(new Heart(this));
            }            

            room1.SetOnScreen(true);
        }

        protected override void UnloadContent()
        {
            if (lonk.room == room1)
            {
                this.Components.Remove(B21); this.Components.Remove(B22);
                this.Components.Remove(B24); this.Components.Remove(B25); this.Components.Remove(B26);

                this.Components.Remove(B31); this.Components.Remove(B32);

                this.Components.Remove(B41); this.Components.Remove(B42);
                this.Components.Remove(B43); this.Components.Remove(B44);

                B = new BladeTrap(this, 1, 6, lonk);
                this.Components.Add(B);
                B.SetAttackBoxes(5f, 0, 0, 0);
            }

            if(lonk.room == room2)
            {
                this.Components.Remove(B); this.Components.Remove(B31); this.Components.Remove(B32);

                B21 = new BladeTrap(this, 2, 6, lonk); B22 = new BladeTrap(this, 3, 1, lonk); 
                B24 = new BladeTrap(this, 6, 1, lonk); B25 = new BladeTrap(this, 7, 6, lonk); B26 = new BladeTrap(this, 8, 1, lonk);

                this.Components.Add(B21); this.Components.Add(B22); 
                this.Components.Add(B24); this.Components.Add(B25); this.Components.Add(B26);

                B21.SetAttackBoxes(4f, 0, 0, 0); B22.SetAttackBoxes(0, 0, 4f, 0); 
                B24.SetAttackBoxes(0, 0, 4f, 0); B25.SetAttackBoxes(4f, 0, 0, 0); B26.SetAttackBoxes(0, 0, 4f, 0);
            }

            if(lonk.room == room3)
            {
                this.Components.Remove(B21); this.Components.Remove(B22); 
                this.Components.Remove(B24); this.Components.Remove(B25); this.Components.Remove(B26);

                this.Components.Remove(B41); this.Components.Remove(B42);
                this.Components.Remove(B43); this.Components.Remove(B44);

                B31 = new BladeTrap(this, 1, 1, lonk); B32 = new BladeTrap(this, 3, 5, lonk);
                this.Components.Add(B31); this.Components.Add(B32);
                B31.SetAttackBoxes(0, 2f, 0, 0); B32.SetAttackBoxes(4f, 0, 0, 2f);
            }

            if(lonk.room == room4)
            {
                this.Components.Remove(B31); this.Components.Remove(B32);

                B41 = new BladeTrap(this, 5, 2, lonk); B42 = new BladeTrap(this, 6, 3, lonk);
                B43 = new BladeTrap(this, 1, 4, lonk); B44 = new BladeTrap(this, 1, 6, lonk);

                this.Components.Add(B41); this.Components.Add(B42);
                this.Components.Add(B43); this.Components.Add(B44);

                B41.SetAttackBoxes(1f, 0, 4f, 4f); B42.SetAttackBoxes(0, 2f, 0, 2f);
                B43.SetAttackBoxes(0, 4f, 0, 0); B44.SetAttackBoxes(0, 7f, 0, 0);
            }

            if(lonk.room == room5)
            {
                this.Components.Remove(B31); this.Components.Remove(B32);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (lonk.CheckForDeath())
            {
                MediaPlayer.Stop();

                Wait++;

                if (Wait >= 200)
                {
                    this.Restart();
                    Wait = 0;
                }
            }

            if(lonk.PlayerHasBeatenGame && !CreditsHaveStarted)
            {
                Wait++;
                if(Wait >= 400)
                {
                    this.CueTitleSequence();
                    Wait = 0;
                }
            }

            if(CreditsHaveStarted)
            {
                Wait++;
                if(Wait >= 200)
                {
                    DrawCredit1 = true;
                }

                if(Wait >= 400)
                {
                    DrawCredit1 = false;
                    DrawCredit2 = true;
                }

                if(Wait >= 600)
                {
                    DrawCredit2 = false;
                    DrawCredit3 = true;
                }
            }

//////////////////////////////////////////////////////////////////////////////////
//  These are all very hacky ways to accomplish small, mostly one-time things   //
//////////////////////////////////////////////////////////////////////////////////

            //Moving slowly up stairs
            if (lonk.room == room1 && lonk.Position.X > 200 && lonk.Position.X < 240 && lonk.Position.Y > 80 && lonk.Position.Y < 120)
                lonk.Speed = 75f;
            else
                lonk.Speed = 150f;

            //Grabbing the Key
            if(lonk.room == room3 && lonk.Position.X > 115 && lonk.Position.X < 160 && lonk.Position.Y < 90)
            {
                lonk.HasKey = true;
                if (!Key.IsDisposed)
                {
                    KeyGet.Play();
                }
                Key.Dispose();
            }

            //Opening the door
            if (lonk.HasKey)
            {
                if (lonk.room == room3 && lonk.HitBox.Bottom >= lonk.room.Collidables[2, 7].Col.Top)
                {
                    lonk.room.Collidables[2, 7] = null;
                    lonk.room.CollisionList.RemoveAt(11);
                    Open.Play();
                    lonk.HasKey = false;
                }
            }

            //Link opening the chest
            if (lonk.room == room5 && lonk.Position.X > 160 && lonk.Position.X < 240 && lonk.Position.Y < 120)
            {
                if (!lonk.PlayerHasBeatenGame)
                {
                    Open.Play();
                    ItemGet.Play();
                    MediaPlayer.Stop();
                }
                Chest = Content.Load<Texture2D>("EnvironmentSprites/ChestOpen");
                lonk.PlayerHasBeatenGame = true;
            }

            //Animating the torches
            if (!CreditsHaveStarted)
            {
                if (TorchAnimationCounter == 7)
                {
                    room3.Collidables[6, 1].TorchAlternate(); room3.Collidables[8, 1].TorchAlternate();
                    room3.Collidables[6, 3].TorchAlternate(); room3.Collidables[8, 3].TorchAlternate();
                    room3.Collidables[6, 6].TorchAlternate(); room3.Collidables[8, 6].TorchAlternate();

                    room5.Collidables[2, 2].TorchAlternate(); room5.Collidables[2, 5].TorchAlternate();
                    room5.Collidables[7, 2].TorchAlternate(); room5.Collidables[7, 5].TorchAlternate();
                }

                if (TorchAnimationCounter >= 14)
                    TorchAnimationCounter = 0;

                if (lonk.room == room3 || lonk.room == room5)
                    TorchAnimationCounter++;

                //////////////////////////////////////////////////////////////////////////////////
                //  These handle Link going between rooms   //
                //////////////////////////////////////////////////////////////////////////////////

                if (lonk.room == room1 && lonk.HitBox.Right >= this.GraphicsDevice.Viewport.Bounds.Right)
                    this.ChangeRooms(room1, room2, 15, lonk.Position.Y);

                if (lonk.room == room2 && lonk.HitBox.Left <= this.GraphicsDevice.Viewport.Bounds.Left)
                    this.ChangeRooms(room2, room1, 385, lonk.Position.Y);

                if (lonk.room == room2 && lonk.HitBox.Right >= this.GraphicsDevice.Viewport.Bounds.Right)
                    this.ChangeRooms(room2, room3, 15, lonk.Position.Y);

                if (lonk.room == room3 && lonk.HitBox.Left <= this.GraphicsDevice.Viewport.Bounds.Left)
                    this.ChangeRooms(room3, room2, 385, lonk.Position.Y);

                if (lonk.room == room3 && lonk.HitBox.Bottom >= this.GraphicsDevice.Viewport.Bounds.Bottom - 20)
                {
                    this.ChangeRooms(room3, room4, lonk.Position.X, 20);

                    TorchAnimationCounter = 0;
                }

                if (lonk.room == room4 && lonk.HitBox.Top <= this.GraphicsDevice.Viewport.Bounds.Top)
                    this.ChangeRooms(room4, room3, lonk.Position.X, 300);


                if (lonk.room == room3 && lonk.Position.X > 280 && lonk.Position.Y < 120)
                {
                    this.ChangeRooms(room3, room5, 180, 260);

                    Stairs.Play();

                    TorchAnimationCounter = 0;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

            int i = 0;

            spriteBatch.Begin();

            if (!CreditsHaveStarted)
            {
                spriteBatch.Draw(UI, new Vector2(0, 320), null, Color.White, 0.0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);           

            if (lonk.room == room3 && !Key.IsDisposed)
                spriteBatch.Draw(Key, new Vector2(120, 40), null, Color.White, 0.0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);

            if (lonk.room == room5)
                spriteBatch.Draw(Chest, new Vector2(180, 60), null, Color.White, 0.0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);

            if (lonk.PlayerHasBeatenGame)
                spriteBatch.Draw(Treasure, new Vector2(lonk.Position.X - 20, lonk.Position.Y - 60), null, Color.White, 0.0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);

                foreach (Heart h in lonk.Health)
                {
                    spriteBatch.Draw(h.GetSpriteTexture(), new Vector2(270 + (i * 20), 325), null, Color.White, 0.0f, new Vector2(0, 0), h.Scale,
                        SpriteEffects.None, 0);

                    i++;
                }
            }
            

            if (CreditsHaveStarted)
            {
                if (DrawCredit1)
                    spriteBatch.Draw(Credit1, new Vector2(0, 0), null, Color.White, 0.0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

                if (DrawCredit2)
                    spriteBatch.Draw(Credit2, new Vector2(0, 0), null, Color.White, 0.0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

                if (DrawCredit3)
                    spriteBatch.Draw(Credit3, new Vector2(0, 0), null, Color.White, 0.0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
            }

                spriteBatch.End();
            }
        

        public void DrawHitBox(Rectangle r, Color c)
        {
            for (int i = r.Left; i < r.Right; i++)
            {
                spriteBatch.Draw(CollisionOutline, new Vector2((float)i, (float)r.Top), c);
                spriteBatch.Draw(CollisionOutline, new Vector2((float)i, (float)r.Bottom), c);
            }

            for (int i = r.Top; i < r.Bottom; i++)
            {
                spriteBatch.Draw(CollisionOutline, new Vector2((float)r.Right, (float)i), c);
                spriteBatch.Draw(CollisionOutline, new Vector2((float)r.Left, (float)i), c);
            }
        }

        private void ChangeRooms(Room ComingFrom, Room GoingTo, float X, float Y)
        {
            ComingFrom.SetOnScreen(false);
            lonk.room = GoingTo;
            GoingTo.SetOnScreen(true);

            lonk.Position.X = X;
            lonk.Position.Y = Y;

            lonk.UpdateHitBox();
            this.UnloadContent();
        }

        public void Restart()
        {
            lonk.Health.Clear();

            if (lonk.room != room1)
                ChangeRooms(lonk.room, room1, lonk.GameStartPos.X, lonk.GameStartPos.Y);

            else
            {
                lonk.Position = lonk.GameStartPos;
                lonk.UpdateHitBox();
                this.Components.Remove(B);

                B = new BladeTrap(this, 1, 6, lonk);
                this.Components.Add(B);
                B.SetAttackBoxes(5f, 0, 0, 0);
            }

            for (int i = 0; i < lonk.NumberOfHearts; i++)
            {
                lonk.AddHeart(new Heart(this));
            }            

            lonk.BringBackToLife();

            MediaPlayer.Play(DungeonMusic);
        }

        public void CueTitleSequence()
        {
            CreditsHaveStarted = true;
            this.Components.Remove(lonk);
            this.Components.Remove(room5);
            MediaPlayer.Play(Credits);
        }
    }
}
