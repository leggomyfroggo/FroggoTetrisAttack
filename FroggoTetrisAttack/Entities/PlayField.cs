using System;

using WarnerEngine.Lib;
using WarnerEngine.Lib.Components;
using WarnerEngine.Lib.Entities;

namespace FroggoTetrisAttack.Entities
{
    public class PlayField : ISceneEntity, IDraw, IPreDraw
    {
        private const int WIDTH = 6;
        private const int HEIGHT = 12;

        private Block[,] _blocks;

        public PlayField()
        {
            Random rand = new Random();
            _blocks = new Block[WIDTH, HEIGHT];
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    if (y < 6)
                    {
                        _blocks[x, y] = new Block(Block.BlockType.Empty, x, y);
                    }
                    else
                    {
                        Block.BlockType suggestedType;
                        do
                        {
                            suggestedType = (Block.BlockType)rand.Next(0, 5);
                        }
                        while ((x > 0 && _blocks[x - 1, y].BType == suggestedType) || (y > 0 && _blocks[x, y - 1].BType == suggestedType));
                        _blocks[x, y] = new Block(suggestedType, x, y);
                    }
                }
            }
        }

        public void OnAdd(Scene ParentScene) { }

        public void OnRemove(Scene ParentScene) { }

        public void PreDraw(float DT) { }

        public void Draw()
        {
            foreach (Block block in _blocks)
            {
                block.Draw(0, 0);
            }
        }

        public BackingBox GetBackingBox()
        {
            return BackingBox.Dummy;
        }

        public bool IsVisible()
        {
            return true;
        }
    }
}
