namespace FroggoTetrisAttack.Entities.State
{
    public class BlockPopState : BlockState
    {
        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            return new BlockPostPopState();
        }
    }
}
