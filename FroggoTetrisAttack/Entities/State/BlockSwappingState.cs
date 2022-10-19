namespace FroggoTetrisAttack.Entities.State
{
    public class BlockSwappingState : BlockState
    {
        private const int SWAP_SPEED = 6;
        private SwapDirection _swapDirection;
        private int _offset;

        public BlockSwappingState(SwapDirection SwapDirection, Block.BlockType NewType)
        {
            _swapDirection = SwapDirection;
            _offset = 0;
        }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            _offset += SWAP_SPEED;
            if (_offset >= Block.BLOCK_SIZE)
            {
                return new BlockSwapFulfilledState(_swapDirection);
            }
            return this;
        }

        public override SwapDirection GetSwapDirection(bool Peak = false)
        {
            if (Peak)
            {
                return _swapDirection;
            }
            return SwapDirection.None;
        }

        public override int GetSwapOffsetMagnitude()
        {
            return _offset;
        }
    }
}
