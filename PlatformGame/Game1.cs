using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace PlatformGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont mainFont;
        Texture2D fillText;
        Player player;
        Camera cam;
        Texture2D backgroundTexture;
        public const int SCREEN_WIDTH = 1400;
        public const int SCREEN_HEIGHT = 800;
        public const int MAP_WIDTH = 1600;
        public const int MAP_HEIGHT = 1600;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            //graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            cam = new Camera();
            backgroundTexture = Content.Load<Texture2D>(@"Textures\Background1");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainFont = Content.Load<SpriteFont>(@"Textures\spritefont");
            fillText = Content.Load<Texture2D>(@"Textures\fillText");

            player = new Player(Content.Load<Texture2D>(@"Textures\CharMove"), Content.Load<Texture2D>(@"Textures\CharATTACK 1"), Content.Load<Texture2D>(@"Textures\Airborne"), new Vector2(900, 780), Content.Load<Texture2D>(@"Textures\ClimbAni"));
            //new GameObject(Content.Load<Texture2D>(@"Textures\charmove"), 19, 1, 10, 1, 60, new Vector2(100, 300), 9, 11);
            
            new Mobster(Content.Load<Texture2D>(@"Textures\Mob1"), new Vector2(500, 1580));
            new Mobster(Content.Load<Texture2D>(@"Textures\Mob1"), new Vector2(900, 1580));

            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1482));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1441));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1400));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1359));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1418));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1382));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1342));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1302));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1362));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1322));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1282));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1242));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1202));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1162));
            new Rope(Content.Load<Texture2D>(@"Textures\Rope"), new Vector2(400, 1122));



            //new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(360, 1321));
            //new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(400, 1321));
            //new Wall(Content.Load<Texture2D>(@"Textures\platform"), new Vector2(440, 1321));

            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(400, 570));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(480, 570));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(560, 570));
            new Wall(Content.Load<Texture2D>(@"Textures\platform"), new Vector2(200, 700));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(640, 570));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(720, 570));


            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(0, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(80, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(160, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(240, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(320, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(400, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(480, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(560, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(640, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(720, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(880, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(960, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(1040, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(1120, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(1200, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(1280, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(1360, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(1420, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(1500, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(1580, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\Platform"), new Vector2(1600, 1600));

            new Wall(Content.Load<Texture2D>(@"Textures\GroundText"), new Vector2(800, 1600));
            new Wall(Content.Load<Texture2D>(@"Textures\GroundText"), new Vector2(800, 1520));

            
            new Wall(Content.Load<Texture2D>(@"Textures\GroundText"), new Vector2(800, 720));
            new Wall(Content.Load<Texture2D>(@"Textures\GroundText"), new Vector2(800, 640));
            new Wall(Content.Load<Texture2D>(@"Textures\GroundText"), new Vector2(1020, 570));
            new Wall(Content.Load<Texture2D>(@"Textures\GroundText"), new Vector2(880, 570));
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            //MouseState ms = Mouse.GetState();
            //GameObject.GameObjecten[1].position.X = ms.X;
            //GameObject.GameObjecten[1].position.Y = ms.Y;
            foreach (GameObject gameObject in GameObject.GameObjecten)
                gameObject.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            cam.Pos = player.position;

            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend, null, null, null, null,
                        cam.transformation(GraphicsDevice));

            //spriteBatch.Draw(backgroundTexture, new Vector2(0, 700), null ,Color.White,0f,Vector2.Zero,1,SpriteEffects.None,-1);
            spriteBatch.Draw(backgroundTexture, new Vector2(0, 700), null, Color.White, 0.0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1.0f);
           
            foreach (GameObject gameObject in GameObject.GameObjecten)
            {
                gameObject.Draw(spriteBatch);
            }
            //spriteBatch.DrawString(mainFont, player.playeraction.ToString(), new Vector2(player.position.X, player.position.Y - 150), Color.Red);
            //spriteBatch.DrawString(mainFont, player.Flip.ToString(), new Vector2(player.position.X, player.position.Y - 175), Color.Red);
            
            //spriteBatch.DrawString(mainFont, player.frame.ToString(), new Vector2(player.position.X, player.position.Y - 200), Color.Red);
           
            spriteBatch.End();

            base.Draw(gameTime);
        }  
    }
}
