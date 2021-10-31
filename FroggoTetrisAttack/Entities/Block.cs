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

        public void Draw(int PlayFieldX, int PlayFieldY, int IndexX, int IndexY, Color? Tint = null, int VisibleLines = BLOCK_SIZE)
        {
            if (BType == BlockType.Empty || !StateMachine.CurrentState.IsVisible())
            {
                return;
            }

            var offset = StateMachine.CurrentState.GetSwapOffset();
            GameService.GetService<IRenderService>().DrawQuad(
                "blocks",
                new Rectangle(
                    PlayFieldX + IndexX * BLOCK_SIZE + (int)offset.X, 
                    PlayFieldY + IndexY * BLOCK_SIZE + (int)offset.Y, 
                    BLOCK_SIZE, 
                    VisibleLines
                ),
                GetImageSource(VisibleLines),
                Tint: Tint
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

        private Rectangle GetImageSource(int VisibleLines)
        {
            return GraphicsHelper.GetSheetCell(
                new Index2(BlockTypeToColumn[BType], BlockFaceToRow[StateMachine.CurrentState.GetBlockFace()]),
                16,
                VisibleLines
            );
        }
    }
}
