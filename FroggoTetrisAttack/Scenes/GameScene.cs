using System;
using System.Collections.Generic;

using WarnerEngine.Lib;

namespace FroggoTetrisAttack.Scenes
{
    public class GameScene : Scene
    {
        public override Dictionary<string, Func<string[], string>> GetLocalTerminalCommands()
        {
            return new Dictionary<string, Func<string[], string>>();
        }

        public override void OnSceneEnd() { }

        public override void OnSceneStart() { }
    }
}
