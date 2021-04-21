namespace FroggoTetrisAttack.Entities.State
{
    public class BlockSwappingState : BlockState
    {
        private const int SWAP_SPEED = 6;
        private SwapDirection _swapDirection;
        private int _offset;
        private bool _latch;

        public BlockSwappingState(SwapDirection SwapDirection, Block.BlockType NewType)
        {
            _swapDirection = SwapDirection;
            _offset = 0;
            _latch = false;
        }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            if (!_latch)
            {
                _offset += SWAP_SPEED;
                if (_offset >= Block.BLOCK_SIZE)
                {
                    _latch = true;
                    _offset = 0;
                }
                return this;
            }
            return new BlockReadyState();
        }

        public override SwapDirection GetSwapDirection(bool Peak = false)
        {
            if (Peak || _latch)
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
