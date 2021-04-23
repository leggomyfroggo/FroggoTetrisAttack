namespace FroggoTetrisAttack.Entities.State
{
    public class BlockEmptyAwaitingFallState : BlockState
    {
        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            if (Context.Top?.IsEmptyAndInactive() == true)
            {
                return new BlockReadyState();
            }
            return this;
        }
    }
}
