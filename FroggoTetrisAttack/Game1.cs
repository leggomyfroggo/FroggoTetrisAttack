using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using WarnerEngine.Services;

namespace FroggoTetrisAttack
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        public Game1()
        {
            GameService.Initialize();

            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            GameService.GetService<IRenderService>()
                .SetGraphicsDevice(GraphicsDevice)
                .SetInternalResolution(400, 240)
                .SetSamplerState(SamplerState.PointWrap);

            GameService.GetService<IContentService>()
                .Bootstrap(Content, GraphicsDevice)
                .LoadAllContent();

            GameService.GetService<ISceneService>()
                .RegisterScene("game_scene", new Scenes.GameScene())
                .SetScene("game_scene");

            GameService.GetService<IInputService>()
                .RegisterInput(InputAction.Left, Key: Keys.Left)
                .RegisterInput(InputAction.Right, Key: Keys.Right)
                .RegisterInput(InputAction.Up, Key: Keys.Up)
                .RegisterInput(InputAction.Down, Key: Keys.Down)
                .RegisterInput(InputAction.Interact, Key: Keys.Space);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float DT = Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f, 0.033f);

            GameService.GetService<IStateService>().SetGlobalGameTime((float)gameTime.TotalGameTime.TotalMilliseconds);
            GameService.GetService<IStateService>().IncrementGlobalFrameCount();

            GameService.PreDraw(DT);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GameService.Draw();
            GameService.PostDraw();

            base.Draw(gameTime);
        }
    }
}
