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
            if (Context.Bottom?.StateMachine.CurrentState is BlockFallingState)
            {
                return new BlockFallingState();
            }
            return this;
        }

        public override Block.BlockFace GetBlockFace()
        {
            return Block.BlockFace.PreFall;
        }
    }
}
