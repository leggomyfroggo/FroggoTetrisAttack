namespace FroggoTetrisAttack.Entities.State
{
    public class BlockSwapFulfilledState : BlockState
    {
        private SwapDirection _swapDirection;

        public BlockSwapFulfilledState(SwapDirection SwapDirection)
        {
            _swapDirection = SwapDirection;
        }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            if (CandidateState is BlockSwappingState || CandidateState is BlockPreClearState)
            {
                return CandidateState;
            }
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            return new BlockReadyState();
        }

        public override SwapDirection GetSwapDirection(bool Peak = false)
        {
            return _swapDirection;
        }
    }
}
