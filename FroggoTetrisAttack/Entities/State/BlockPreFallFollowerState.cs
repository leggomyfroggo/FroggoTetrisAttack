namespace FroggoTetrisAttack.Entities.State
{
    public class BlockPreFallFollowerState : BlockState
    {
        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            // TODO: Should be swappable from this state
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            if (Context.Bottom?.BType == Block.BlockType.Empty)
            {
                return new BlockFallingState();
            }
            return this; ;
        }
    }
}
