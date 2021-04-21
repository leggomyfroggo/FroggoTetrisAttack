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

        public State.BlockStateMachine StateMachine { get; private set; }

        private static Dictionary<BlockType, Rectangle> BlockTypeToImage = new Dictionary<BlockType, Rectangle>() 
        {
            { BlockType.Red, GraphicsHelper.GetSheetCell(new Index2(3, 0), 16, 16) },
            { BlockType.Blue, GraphicsHelper.GetSheetCell(new Index2(1, 0), 16, 16) },
            { BlockType.Green, GraphicsHelper.GetSheetCell(new Index2(0, 0), 16, 16) },
            { BlockType.Yellow, GraphicsHelper.GetSheetCell(new Index2(0, 1), 16, 16) },
            { BlockType.Purple, GraphicsHelper.GetSheetCell(new Index2(2, 0), 16, 16) },
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

        public void Draw(int PlayFieldX, int PlayFieldY, int IndexX, int IndexY)
        {
            if (BType == BlockType.Empty)
            {
                return;
            }

            GameService.GetService<IRenderService>().DrawQuad(
                "blocks",
                new Rectangle(PlayFieldX + IndexX * BLOCK_SIZE, PlayFieldY + IndexY * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE),
                BlockTypeToImage[BType]
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
    }
}
