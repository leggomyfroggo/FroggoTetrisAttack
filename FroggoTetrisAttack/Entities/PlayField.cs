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
        private const int PLAYFIELD_X = 152;
        private const int PLAYFIELD_Y = 24;
        private const int WIDTH = 6;
        private const int HEIGHT = 12;

        private Block[,] _blocks;
        private Block[,] _blockBuffer;

        private Block[] _newLine;
        private float _newLineProgress;

        private State.PlayFieldStateMachine _stateMachine;

        private int _swapperXIndex;
        private int _swapperYIndex;

        public PlayField()
        {
            Random rand = new Random();
            _blocks = new Block[WIDTH, HEIGHT];
            _blockBuffer = new Block[WIDTH, HEIGHT];
            int remainingInitialBlocks = 36;
            for (int x = 0; x < WIDTH; x++)
            {
                int columnMin = remainingInitialBlocks - (WIDTH - x - 1) * 7;
                int columnBlockCount = rand.Next(columnMin, 7 + 1);
                remainingInitialBlocks -= columnBlockCount;
                for (int y = 0; y < HEIGHT; y++)
                {
                    if (y < HEIGHT - columnBlockCount)
                    {
                        _blocks[x, y] = new Block(Block.BlockType.Empty, this);
                    }
                    else
                    {
                        Block.BlockType suggestedType;
                        do
                        {
                            suggestedType = (Block.BlockType)rand.Next(0, 5);
                        }
                        while ((x > 0 && _blocks[x - 1, y].BType == suggestedType) || (y > 0 && _blocks[x, y - 1].BType == suggestedType));
                        _blocks[x, y] = new Block(suggestedType, this);
                    }
                }
            }

            // Create the new line
            _newLine = new Block[WIDTH];
            for (int x = 0; x < _newLine.Length; x++)
            {
                Block.BlockType suggestedType;
                do
                {
                    suggestedType = (Block.BlockType)rand.Next(0, 5);
                }
                while (x > 0 && _newLine[x - 1].BType == suggestedType);
                _newLine[x] = new Block(suggestedType, this);
            }

            _stateMachine = new State.PlayFieldStateMachine(new State.PlayFieldActiveState(), this);
            _swapperXIndex = 2;
            _swapperYIndex = 6;
        }

        public void OnAdd(Scene ParentScene) { }

        public void OnRemove(Scene ParentScene) { }

        public void PreDraw(float DT) 
        {
            // Primary update loop for the field, going from bottom to top to account for gravity
            bool isInputLocked = false;
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = HEIGHT - 1; y >= 0; y--)
                {
                    var block = GetBlockAt(x, y);
                    block.PreDraw(
                        DT,
                        new State.BlockContext(GetBlockAt(x, y - 1), GetBlockAt(x + 1, y), GetBlockAt(x, y + 1), GetBlockAt(x - 1, y))
                    );
                    var currentBlockState = block.StateMachine.CurrentState;
                    // Lock input if we see a block that's swapping
                    if (!isInputLocked && currentBlockState is State.BlockSwappingState)
                    {
                        isInputLocked = true;
                    }
                    SwapBlockToBuffer(x, y, currentBlockState.GetSwapDirection());
                }
            }

            // Merge buffer with regular field
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    _blocks[x, y] = _blockBuffer[x, y] ?? _blocks[x, y];
                    UpdateBufferAt(x, y, null);
                }
            }

            int totalMatchCount = 0;
            bool[,] matchMap = new bool[WIDTH, HEIGHT];

            // Horizontal matches
            for (int y = 0; y < HEIGHT; y++)
            {
                int numContiguous = 0;
                for (int x = 0; x <= WIDTH; x++)
                {
                    Block currentBlock = GetBlockAt(x, y);
                    Block prevBlock = GetBlockAt(x - 1, y);
                    if (prevBlock?.BType != currentBlock?.BType || prevBlock?.IsClearable() == false || x == WIDTH)
                    {
                        if (numContiguous >= 3)
                        {
                            for (int r = 1; r <= numContiguous; r++)
                            {
                                totalMatchCount++;
                                matchMap[x - r, y] = true;
                            }
                        }
                        numContiguous = 0;
                    }
                    if (currentBlock?.IsClearable() == true)
                    {
                        numContiguous++;
                    }
                }
            }

            // Vertical matches
            for (int x = 0; x <= WIDTH; x++)
            {
                int numContiguous = 0;
                for (int y = 0; y <= HEIGHT; y++)
                {
                    Block currentBlock = GetBlockAt(x, y);
                    Block prevBlock = GetBlockAt(x, y - 1);
                    if (prevBlock?.BType != currentBlock?.BType || prevBlock?.IsClearable() == false || y == HEIGHT)
                    {
                        if (numContiguous >= 3)
                        {
                            for (int r = 1; r <= numContiguous; r++)
                            {
                                totalMatchCount++;
                                matchMap[x, y - r] = true;
                            }
                        }
                        numContiguous = 0;
                    }
                    if (currentBlock?.IsClearable() == true)
                    {
                        numContiguous++;
                    }
                }
            }

            int popIndex = 0;
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    if (matchMap[x, y])
                    {
                        GetBlockAt(x, y).StateMachine.ConsiderStateChange(new State.BlockPreClearState(popIndex++, totalMatchCount));
                    }
                }
            }

            // Nope out of here if input is locked
            if (isInputLocked)
            {
                return;
            }

            var inputService = GameService.GetService<IInputService>();
            if (inputService.WasActionPressed(InputAction.Left))
            {
                _swapperXIndex = Math.Clamp(_swapperXIndex - 1, 0, WIDTH - 2);
            }
            else if (inputService.WasActionPressed(InputAction.Right))
            {
                _swapperXIndex = Math.Clamp(_swapperXIndex + 1, 0, WIDTH - 2);
            }
            if (inputService.WasActionPressed(InputAction.Up))
            {
                _swapperYIndex = Math.Clamp(_swapperYIndex - 1, 0, HEIGHT - 1);
            }
            else if (inputService.WasActionPressed(InputAction.Down))
            {
                _swapperYIndex = Math.Clamp(_swapperYIndex + 1, 0, HEIGHT - 1);
            }
            if (inputService.WasActionPressed(InputAction.Interact))
            {
                var leftBlock = _blocks[_swapperXIndex, _swapperYIndex];
                var rightBlock = _blocks[_swapperXIndex + 1, _swapperYIndex];
                if (
                    leftBlock.StateMachine.CurrentState is State.BlockReadyState &&
                    rightBlock.StateMachine.CurrentState is State.BlockReadyState
                )
                {
                    leftBlock.StateMachine.ConsiderStateChange(new State.BlockSwappingState(State.BlockState.SwapDirection.Right, rightBlock.BType));
                    rightBlock.StateMachine.ConsiderStateChange(new State.BlockSwappingState(State.BlockState.SwapDirection.Left, leftBlock.BType));
                }
            }
        }

        public void Draw()
        {
            int newLineProgress = (int)_newLineProgress;
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    GetBlockAt(x, y).Draw(PLAYFIELD_X, PLAYFIELD_Y - newLineProgress, x, y);
                }
                _newLine[x].Draw(PLAYFIELD_X, PLAYFIELD_Y - newLineProgress, x, HEIGHT, Tint: Color.Gray, VisibleLines: (int)_newLineProgress);
            }
            DrawSwapper(newLineProgress);
        }

        private void DrawSwapper(int YOffset)
        {
            GraphicsHelper.DrawSquare(
                new Rectangle(
                    PLAYFIELD_X + _swapperXIndex * Block.BLOCK_SIZE, 
                    PLAYFIELD_Y + _swapperYIndex * Block.BLOCK_SIZE - YOffset, 
                    Block.BLOCK_SIZE, 
                    Block.BLOCK_SIZE
                ),
                Color.White
            );
            GraphicsHelper.DrawSquare(
                new Rectangle(
                    PLAYFIELD_X + (_swapperXIndex + 1) * Block.BLOCK_SIZE, 
                    PLAYFIELD_Y + _swapperYIndex * Block.BLOCK_SIZE - YOffset, 
                    Block.BLOCK_SIZE, 
                    Block.BLOCK_SIZE
                ),
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

        public Block GetBlockAt1D(int Index)
        {
            return _blocks[Index % WIDTH, Index / WIDTH];
        }

        public Block GetBlockAt(int X, int Y, bool IncludeBuffer = false)
        {
            if (X < 0 || X >= WIDTH || Y < 0 || Y >= HEIGHT)
            {
                return null;
            }
            if (IncludeBuffer)
            {
                return _blockBuffer[X, Y] ?? _blocks[X, Y];
            }
            return _blocks[X, Y];
        }

        public void SwapBlockToBuffer(int X, int Y, State.BlockState.SwapDirection SwapDirection)
        {
            Block block = GetBlockAt(X, Y);
            Block replacedBlock = null;
            switch (SwapDirection)
            {
                case State.BlockState.SwapDirection.Up:
                    replacedBlock = GetBlockAt(X, Y - 1, true);
                    UpdateBufferAt(X, Y - 1, block);
                    break;
                case State.BlockState.SwapDirection.Right:
                    replacedBlock = GetBlockAt(X + 1, Y, true);
                    UpdateBufferAt(X + 1, Y, block);
                    break;
                case State.BlockState.SwapDirection.Down:
                    replacedBlock = GetBlockAt(X, Y + 1, true);
                    UpdateBufferAt(X, Y + 1, block);
                    break;
                case State.BlockState.SwapDirection.Left:
                    replacedBlock = GetBlockAt(X - 1, Y, true);
                    UpdateBufferAt(X - 1, Y, block);
                    break;
            }
            if (_blockBuffer[X, Y] == null)
            {
                UpdateBufferAt(X, Y, replacedBlock);
            }
        }

        public void UpdateBufferAt(int X, int Y, Block B)
        {
            if (X < 0 || X >= WIDTH || Y < 0 || Y >= HEIGHT)
            {
                return;
            }
            _blockBuffer[X, Y] = B;
        }
    }
}
