using System.Collections.Generic;

using Microsoft.Xna.Framework;

using WarnerEngine.Lib.Components;
using WarnerEngine.Lib.Entities;
using WarnerEngine.Services;

namespace FroggoTetrisAttack.Entities
{
    public class Block : IPreDraw
    {
        public const int BLOCK_SIZE = 16;

        private int _xIndex;
        private int _yIndex;

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

        private static Dictionary<BlockType, Color> BlockTypeToColor = new Dictionary<BlockType, Color>() 
        {
            { BlockType.Red, Color.Red },
            { BlockType.Blue, Color.Blue },
            { BlockType.Green, Color.LimeGreen },
            { BlockType.Yellow, Color.Yellow },
            { BlockType.Purple, Color.MediumPurple },
        };

        public BlockType BType { get; private set; }

        public Block(BlockType BType, int XIndex, int YIndex)
        {
            this.BType = BType;
            StateMachine = new State.BlockStateMachine(new State.BlockReadyState(), this);
            _xIndex = XIndex;
            _yIndex = YIndex;
        }

        public void PreDraw(float DT) 
        {
            StateMachine.Update(this, DT);
        }

        public void Draw(int PlayFieldX, int PlayFieldY)
        {
            if (BType == BlockType.Empty)
            {
                return;
            }

            GameService.GetService<IRenderService>().DrawQuad(
                GameService.GetService<IContentService>().GetWhiteTileTexture(),
                new Rectangle(PlayFieldX + _xIndex * BLOCK_SIZE, PlayFieldY + _yIndex * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE),
                new Rectangle(0, 0, 8, 8),
                Tint: BlockTypeToColor[BType]
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
