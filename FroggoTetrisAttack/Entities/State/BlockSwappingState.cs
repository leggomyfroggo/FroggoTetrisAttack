namespace FroggoTetrisAttack.Entities.State
{
    public class BlockSwappingState : BlockState
    {
        private SwapDirection _swapDirection;
        private bool _latch;

        public BlockSwappingState(SwapDirection SwapDirection, Block.BlockType NewType)
        {
            _swapDirection = SwapDirection;
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
                _latch = true;
                return this;
            }
            return new BlockReadyState();
        }

        public override SwapDirection GetSwapDirection()
        {
            return _swapDirection;
        }
    }
}
