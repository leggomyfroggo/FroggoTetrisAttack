namespace FroggoTetrisAttack.Entities.State
{
    public class BlockSwappingState : BlockState
    {
        private int _swapDirection;
        private Block.BlockType _newType;

        public BlockSwappingState(int SwapDirection, Block.BlockType NewType)
        {
            _swapDirection = SwapDirection;
            _newType = NewType;
        }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override void Enter(Block Target, BlockState PreviousState) { }

        public override void Exit(Block Target, BlockState IncomingState) { }

        public override BlockStateType GetStateType()
        {
            return BlockStateType.Swapping;
        }

        public override BlockState Update(Block Target, float DT)
        {
            Target.ChangeType(_newType);
            return new BlockReadyState();
        }
    }
}
