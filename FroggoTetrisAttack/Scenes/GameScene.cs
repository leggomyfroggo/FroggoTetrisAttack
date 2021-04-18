using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using WarnerEngine.Lib;
using WarnerEngine.Services;
using WarnerEngine.Services.Implementations;

using FroggoTetrisAttack.Entities;

namespace FroggoTetrisAttack.Scenes
{
    public class GameScene : Scene
    {
        public override Dictionary<string, Func<string[], string>> GetLocalTerminalCommands()
        {
            return new Dictionary<string, Func<string[], string>>();
        }

        public GameScene()
        {
            AddEntity(new PlayField());
            Camera = new Camera(new Vector2(200, 120));
        }

        public override void OnSceneEnd() { }

        public override void OnSceneStart() { }

        public override void Draw()
        {
            GameService.GetService<IRenderService>()
                .SetRenderTarget(SceneService.RenderTargets.CompositeTertiary, Color.Black)
                .Start(Camera.GetCenterPoint())
                .Render(base.Draw)
                .End()
                .Cleanup();
        }
    }
}
