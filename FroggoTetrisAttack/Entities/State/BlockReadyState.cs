namespace FroggoTetrisAttack.Entities.State
{
    public class BlockReadyState : BlockState
    {
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
            if (Target.BType != Block.BlockType.Empty)
            {
                if (Context.Bottom?.IsEmptyAndInactive() == true)
                {
                    return new BlockPreFallLeaderState();
                }
                else if (
                    Context.Bottom?.StateMachine.CurrentState is BlockPreFallLeaderState || 
                    Context.Bottom?.StateMachine.CurrentState is BlockPreFallFollowerState
                )
                {
                    return new BlockPreFallFollowerState();
                }
            }
            return this;
        }
    }
}
