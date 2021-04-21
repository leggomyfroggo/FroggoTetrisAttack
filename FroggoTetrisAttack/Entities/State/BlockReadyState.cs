namespace FroggoTetrisAttack.Entities.State
{
    public class BlockReadyState : BlockState
    {
        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            if (CandidateState is BlockSwappingState state) 
            {
                return state;
            }
            return this;
        }

        public override void Enter(Block Target, BlockState PreviousState) { }

        public override void Exit(Block Target, BlockState IncomingState) { }

        public override BlockStateType GetStateType()
        {
            return BlockStateType.Ready;
        }

        public override BlockState Update(Block Target, float DT)
        {
            var bottomNeighbor = Target.GetBottomNeighbor();
            if (bottomNeighbor != null)
            {
                if (bottomNeighborbottomNeighbor.StateMachine.CurrentState is )
            }
            return this;
        }
    }
}
