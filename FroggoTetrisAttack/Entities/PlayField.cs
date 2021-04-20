using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using WarnerEngine.Lib;
using WarnerEngine.Lib.Components;
using WarnerEngine.Lib.Entities;
using WarnerEngine.Lib.Helpers;
using WarnerEngine.Services;

namespace FroggoTetrisAttack.Entities
{
    public class PlayField : ISceneEntity, IDraw, IPreDraw
    {
        private const int WIDTH = 6;
        private const int HEIGHT = 12;

        private Block[,] _blocks;

        private State.PlayFieldStateMachine _stateMachine;

        private int _swapperXIndex;
        private int _swapperYIndex;

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

            _stateMachine = new State.PlayFieldStateMachine(new State.PlayFieldActiveState(), this);
            _swapperXIndex = 2;
            _swapperYIndex = 6;
        }

        public void OnAdd(Scene ParentScene) { }

        public void OnRemove(Scene ParentScene) { }

        public void PreDraw(float DT) 
        { 
            foreach (Block block in _blocks)
            {
                block.PreDraw(DT);
            }

            var inputService = GameService.GetService<IInputService>();
            if (inputService.WasKeyPressed(Keys.Left))
            {
                _swapperXIndex = Math.Clamp(_swapperXIndex - 1, 0, WIDTH - 2);
            }
            else if (inputService.WasKeyPressed(Keys.Right))
            {
                _swapperXIndex = Math.Clamp(_swapperXIndex + 1, 0, WIDTH - 2);
            }
            if (inputService.WasKeyPressed(Keys.Up))
            {
                _swapperYIndex = Math.Clamp(_swapperYIndex - 1, 0, HEIGHT - 1);
            }
            else if (inputService.WasKeyPressed(Keys.Down))
            {
                _swapperYIndex = Math.Clamp(_swapperYIndex + 1, 0, HEIGHT - 1);
            }
            if (inputService.WasKeyPressed(Keys.Space))
            {
                var leftBlock = _blocks[_swapperXIndex, _swapperYIndex];
                var rightBlock = _blocks[_swapperXIndex + 1, _swapperYIndex];
                if (
                    leftBlock.StateMachine.CurrentState is State.BlockReadyState &&
                    rightBlock.StateMachine.CurrentState is State.BlockReadyState
                )
                {
                    leftBlock.StateMachine.ConsiderStateChange(new State.BlockSwappingState(1, rightBlock.BType), leftBlock);
                    rightBlock.StateMachine.ConsiderStateChange(new State.BlockSwappingState(-1, leftBlock.BType), rightBlock);
                }
            }
        }

        public void Draw()
        {
            foreach (Block block in _blocks)
            {
                block.Draw(0, 0);
            }
            DrawSwapper();
        }

        private void DrawSwapper()
        {
            GraphicsHelper.DrawSquare(
                new Rectangle(_swapperXIndex * Block.BLOCK_SIZE, _swapperYIndex * Block.BLOCK_SIZE, Block.BLOCK_SIZE, Block.BLOCK_SIZE),
                Color.White
            );
            GraphicsHelper.DrawSquare(
                new Rectangle((_swapperXIndex + 1) * Block.BLOCK_SIZE, _swapperYIndex * Block.BLOCK_SIZE, Block.BLOCK_SIZE, Block.BLOCK_SIZE),
                Color.White
            );
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
