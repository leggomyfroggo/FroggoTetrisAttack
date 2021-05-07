using System.Collections.Generic;

using Microsoft.Xna.Framework;

using WarnerEngine.Lib;
using WarnerEngine.Lib.Helpers;
using WarnerEngine.Lib.Components;
using WarnerEngine.Services;

namespace FroggoTetrisAttack.Entities
{
    public class Block
    {
        public const int BLOCK_SIZE = 16;

        public PlayField PField;

        public enum BlockType 
        { 
            Empty = -1, 
            Red = 0, 
            Blue = 1, 
            Green = 2, 
            Yellow = 3, 
            Purple = 4 
        }

        public enum BlockFace
        {
            Neutral,
            Flash1,
            Flash2,
            Dead,
            PreFall,
            Fall,
        }

        public State.BlockStateMachine StateMachine { get; private set; }

        private static Dictionary<BlockType, int> BlockTypeToColumn = new Dictionary<BlockType, int>() 
        {
            { BlockType.Green, 0 },
            { BlockType.Blue, 1 },
            { BlockType.Purple, 2 },
            { BlockType.Red, 3 },
            { BlockType.Yellow, 4 },
        };

        private static Dictionary<BlockFace, int> BlockFaceToRow = new Dictionary<BlockFace, int>()
        {
            { BlockFace.Neutral, 0 },
            { BlockFace.Flash1, 1 },
            { BlockFace.Flash2, 2 },
            { BlockFace.Dead, 3 },
            { BlockFace.PreFall, 4 },
            { BlockFace.Fall, 5 },
        };

        public BlockType BType { get; private set; }

        public Block(BlockType BType, PlayField PField)
        {
            this.BType = BType;
            StateMachine = new State.BlockStateMachine(new State.BlockReadyState(), this);
            this.PField = PField;
        }

        public void PreDraw(float DT, State.BlockContext Context) 
        {
            StateMachine.Update(DT, Context);
        }

        public void Draw(int PlayFieldX, int PlayFieldY, int IndexX, int IndexY, float RiseProgress, bool IsActive = true)
        {
            if (BType == BlockType.Empty || !StateMachine.CurrentState.IsVisible())
            {
                return;
            }

            var offset = StateMachine.CurrentState.GetSwapOffset();
            if (IsActive)
            {
                GameService.GetService<IRenderService>().DrawQuad(
                    "blocks",
                    new Rectangle(
                        PlayFieldX + IndexX * BLOCK_SIZE + (int)offset.X,
                        PlayFieldY + IndexY * BLOCK_SIZE + (int)offset.Y - (int)(RiseProgress * BLOCK_SIZE),
                        BLOCK_SIZE,
                        BLOCK_SIZE
                    ),
                    GetImageSource()
                );
            }
            else
            {
                int riseProgressNormalized = (int)(RiseProgress * BLOCK_SIZE);
                GameService.GetService<IRenderService>().DrawQuad(
                    "blocks",
                    new Rectangle(
                        PlayFieldX + IndexX * BLOCK_SIZE + (int)offset.X,
                        PlayFieldY + IndexY * BLOCK_SIZE + (int)offset.Y - (int)(RiseProgress * BLOCK_SIZE),
                        BLOCK_SIZE,
                        riseProgressNormalized
                    ),
                    GetPartialImageSource(riseProgressNormalized),
                    Tint: Color.Gray
                );
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

        public void ChangeType(BlockType NewType)
        {
            BType = NewType;
        }

        public bool IsClearable()
        {
            return StateMachine.CurrentState is State.BlockReadyState && BType != BlockType.Empty;
        }

        public bool IsEmptyAndInactive()
        {
            return StateMachine.CurrentState is State.BlockReadyState && BType == BlockType.Empty;
        }

        private Rectangle GetImageSource()
        {
            return GraphicsHelper.GetSheetCell(
                new Index2(BlockTypeToColumn[BType], BlockFaceToRow[StateMachine.CurrentState.GetBlockFace()]),
                BLOCK_SIZE,
                BLOCK_SIZE
            );
        }

        private Rectangle GetPartialImageSource(int RiseProgressNormalized)
        {
            return GraphicsHelper.GetSheetCell(
                new Index2(BlockTypeToColumn[BType], BlockFaceToRow[StateMachine.CurrentState.GetBlockFace()]),
                BLOCK_SIZE,
                RiseProgressNormalized
            );
        }
    }
}
