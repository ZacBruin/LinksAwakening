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

        SoundEffectInstance SFI;
        SoundEffect KeyGet, ItemGet, Open, Stairs;
        Song DungeonMusic, Credits;

        bool CreditsHaveStarted, DrawCredit1, DrawCredit2, DrawCredit3;

        int TorchAnimationCounter, Wait;

        Link link;
        BladeTrap B, B21, B22, B24, B25, B26, B31, B32, B41, B42, B43, B44;
        Room room1, room2, room3, room4, room5;

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 360;

            room1 = new Room(this, 1);
            room2 = new Room(this, 2);
            room3 = new Room(this, 3);
            room4 = new Room(this, 4);
            room5 = new Room(this, 5);

            link = new Link(this, room1);

            CreditsHaveStarted = false; DrawCredit1 = false; DrawCredit2 = false; DrawCredit3 = false;

            B = new BladeTrap(this, 1, 6, link);

            Components.Add(room1);
            Components.Add(room2);
            Components.Add(room3);
            Components.Add(room4);
            Components.Add(room5);

            Components.Add(link);

            Components.Add(B);
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
            KeyGet = Content.Load<SoundEffect>("Audio/LA_Get_Item2");
            ItemGet = Content.Load<SoundEffect>("Audio/LA_Fanfare_Item_Extended");
            Open = Content.Load<SoundEffect>("Audio/LA_Chest_Open");
            Stairs = Content.Load<SoundEffect>("Audio/LA_Stairs");

            MediaPlayer.Play(DungeonMusic);
            MediaPlayer.IsRepeating = true;

            B.SetAttackBoxes(5f, 0, 0, 0);

            TorchAnimationCounter = 0;

            for (int i = 0; i < link.NumberOfHearts; i++)
                link.AddHeart(new Heart(this));

            room1.SetOnScreen(true);
        }

        protected override void UnloadContent()
        {
            if (link.room == room1)
            {
                Components.Remove(B21);
                Components.Remove(B22);
                Components.Remove(B24);
                Components.Remove(B25);
                Components.Remove(B26);

                Components.Remove(B31);
                Components.Remove(B32);

                Components.Remove(B41);
                Components.Remove(B42);
                Components.Remove(B43);
                Components.Remove(B44);

                B = new BladeTrap(this, 1, 6, link);
                Components.Add(B);
                B.SetAttackBoxes(5f, 0, 0, 0);
            }

            if (link.room == room2)
            {
                Components.Remove(B);
                Components.Remove(B31);
                Components.Remove(B32);

                B21 = new BladeTrap(this, 2, 6, link);
                B22 = new BladeTrap(this, 3, 1, link);
                B24 = new BladeTrap(this, 6, 1, link);
                B25 = new BladeTrap(this, 7, 6, link);
                B26 = new BladeTrap(this, 8, 1, link);

                Components.Add(B21);
                Components.Add(B22);
                Components.Add(B24);
                Components.Add(B25);
                Components.Add(B26);

                B21.SetAttackBoxes(4f, 0, 0, 0);
                B22.SetAttackBoxes(0, 0, 4f, 0);
                B24.SetAttackBoxes(0, 0, 4f, 0);
                B25.SetAttackBoxes(4f, 0, 0, 0);
                B26.SetAttackBoxes(0, 0, 4f, 0);
            }

            if (link.room == room3)
            {
                Components.Remove(B21);
                Components.Remove(B22);
                Components.Remove(B24);
                Components.Remove(B25);
                Components.Remove(B26);

                Components.Remove(B41);
                Components.Remove(B42);
                Components.Remove(B43);
                Components.Remove(B44);

                B31 = new BladeTrap(this, 1, 1, link); B32 = new BladeTrap(this, 3, 5, link);
                Components.Add(B31); Components.Add(B32);
                B31.SetAttackBoxes(0, 2f, 0, 0); B32.SetAttackBoxes(4f, 0, 0, 2f);
            }

            if (link.room == room4)
            {
                Components.Remove(B31); Components.Remove(B32);

                B41 = new BladeTrap(this, 5, 2, link);
                B42 = new BladeTrap(this, 6, 3, link);
                B43 = new BladeTrap(this, 1, 4, link);
                B44 = new BladeTrap(this, 1, 6, link);

                Components.Add(B41);
                Components.Add(B42);
                Components.Add(B43);
                Components.Add(B44);

                B41.SetAttackBoxes(1f, 0, 4f, 4f);
                B42.SetAttackBoxes(0, 2f, 0, 2f);
                B43.SetAttackBoxes(0, 4f, 0, 0);
                B44.SetAttackBoxes(0, 7f, 0, 0);
            }

            if (link.room == room5)
            {
                Components.Remove(B31);
                Components.Remove(B32);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (link.DeathCheck())
            {
                MediaPlayer.Stop();

                Wait++;

                if (Wait >= 200)
                {
                    Restart();
                    Wait = 0;
                }
            }

            if (link.PlayerHasBeatenGame && !CreditsHaveStarted)
            {
                Wait++;
                if (Wait >= 400)
                {
                    CueTitleSequence();
                    Wait = 0;
                }
            }

            if (CreditsHaveStarted)
            {
                Wait++;
                if (Wait >= 200)
                {
                    DrawCredit1 = true;
                }

                if (Wait >= 400)
                {
                    DrawCredit1 = false;
                    DrawCredit2 = true;
                }

                if (Wait >= 600)
                {
                    DrawCredit2 = false;
                    DrawCredit3 = true;
                }
            }

            //////////////////////////////////////////////////////////////////////////////////
            //  These are all very hacky ways to accomplish small, mostly one-time things   //
            //////////////////////////////////////////////////////////////////////////////////

            //Moving slowly up stairs
            if (link.room == room1 && link.Position.X > 200 && link.Position.X < 240 && link.Position.Y > 80 && link.Position.Y < 120)
                link.Speed = 75f;
            else
                link.Speed = 150f;

            //Grabbing the Key
            if (link.room == room3 && link.Position.X > 115 && link.Position.X < 160 && link.Position.Y < 90)
            {
                link.HasKey = true;
                if (!Key.IsDisposed)
                {
                    KeyGet.Play();
                }
                Key.Dispose();
            }

            //Opening the door
            if (link.HasKey)
            {
                if (link.room == room3 && link.HitBox.Bottom >= link.room.Collidables[2, 7].Col.Top)
                {
                    link.room.Collidables[2, 7] = null;
                    link.room.CollisionList.RemoveAt(11);
                    Open.Play();
                    link.HasKey = false;
                }
            }

            //Link opening the chest
            if (link.room == room5 && link.Position.X > 160 && link.Position.X < 240 && link.Position.Y < 120)
            {
                if (!link.PlayerHasBeatenGame)
                {
                    Open.Play();
                    ItemGet.Play();
                    MediaPlayer.Stop();
                }
                Chest = Content.Load<Texture2D>("EnvironmentSprites/ChestOpen");
                link.PlayerHasBeatenGame = true;
            }

            //Animating the torches
            if (!CreditsHaveStarted)
            {
                if (TorchAnimationCounter == 7)
                {
                    room3.Collidables[6, 1].AnimateTorch();
                    room3.Collidables[8, 1].AnimateTorch();
                    room3.Collidables[6, 3].AnimateTorch();
                    room3.Collidables[8, 3].AnimateTorch();
                    room3.Collidables[6, 6].AnimateTorch();
                    room3.Collidables[8, 6].AnimateTorch();

                    room5.Collidables[2, 2].AnimateTorch();
                    room5.Collidables[2, 5].AnimateTorch();
                    room5.Collidables[7, 2].AnimateTorch();
                    room5.Collidables[7, 5].AnimateTorch();
                }

                if (TorchAnimationCounter >= 14)
                    TorchAnimationCounter = 0;

                if (link.room == room3 || link.room == room5)
                    TorchAnimationCounter++;

                //////////////////////////////////////////////////////////////////////////////////
                //  These handle Link going between rooms   //
                //////////////////////////////////////////////////////////////////////////////////

                if (link.room == room1 && link.HitBox.Right >= GraphicsDevice.Viewport.Bounds.Right)
                    ChangeRooms(room1, room2, 15, link.Position.Y);

                if (link.room == room2 && link.HitBox.Left <= GraphicsDevice.Viewport.Bounds.Left)
                    ChangeRooms(room2, room1, 385, link.Position.Y);

                if (link.room == room2 && link.HitBox.Right >= GraphicsDevice.Viewport.Bounds.Right)
                    ChangeRooms(room2, room3, 15, link.Position.Y);

                if (link.room == room3 && link.HitBox.Left <= GraphicsDevice.Viewport.Bounds.Left)
                    ChangeRooms(room3, room2, 385, link.Position.Y);

                if (link.room == room3 && link.HitBox.Bottom >= GraphicsDevice.Viewport.Bounds.Bottom - 20)
                {
                    ChangeRooms(room3, room4, link.Position.X, 20);

                    TorchAnimationCounter = 0;
                }

                if (link.room == room4 && link.HitBox.Top <= GraphicsDevice.Viewport.Bounds.Top)
                    ChangeRooms(room4, room3, link.Position.X, 300);


                if (link.room == room3 && link.Position.X > 280 && link.Position.Y < 120)
                {
                    ChangeRooms(room3, room5, 180, 260);
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

                if (link.room == room3 && !Key.IsDisposed)
                    spriteBatch.Draw(Key, new Vector2(120, 40), null, Color.White, 0.0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);

                if (link.room == room5)
                    spriteBatch.Draw(Chest, new Vector2(180, 60), null, Color.White, 0.0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);

                if (link.PlayerHasBeatenGame)
                    spriteBatch.Draw(Treasure, new Vector2(link.Position.X - 20, link.Position.Y - 60), null, Color.White, 0.0f, new Vector2(0, 0), .25f, SpriteEffects.None, 0);

                foreach (Heart h in link.Health)
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
                spriteBatch.Draw(CollisionOutline, new Vector2(i, r.Top), c);
                spriteBatch.Draw(CollisionOutline, new Vector2(i, r.Bottom), c);
            }

            for (int i = r.Top; i < r.Bottom; i++)
            {
                spriteBatch.Draw(CollisionOutline, new Vector2(r.Right, i), c);
                spriteBatch.Draw(CollisionOutline, new Vector2(r.Left, i), c);
            }
        }

        private void ChangeRooms(Room ComingFrom, Room GoingTo, float X, float Y)
        {
            ComingFrom.SetOnScreen(false);
            link.room = GoingTo;
            GoingTo.SetOnScreen(true);

            link.Position.X = X;
            link.Position.Y = Y;

            link.UpdateHitBox();
            UnloadContent();
        }

        public void Restart()
        {
            link.Health.Clear();

            if (link.room != room1)
                ChangeRooms(link.room, room1, link.GameStartPos.X, link.GameStartPos.Y);

            else
            {
                link.Position = link.GameStartPos;
                link.UpdateHitBox();
                Components.Remove(B);

                B = new BladeTrap(this, 1, 6, link);
                Components.Add(B);
                B.SetAttackBoxes(5f, 0, 0, 0);
            }

            for (int i = 0; i < link.NumberOfHearts; i++)
                link.AddHeart(new Heart(this));

            link.BringBackToLife();

            MediaPlayer.Play(DungeonMusic);
        }

        public void CueTitleSequence()
        {
            CreditsHaveStarted = true;
            Components.Remove(link);
            Components.Remove(room5);
            MediaPlayer.Play(Credits);
        }
    }
}
