namespace FroggoTetrisAttack.Entities.State
{
    public class BlockPreFallState : BlockState
    {
        private const int PAUSE_FRAMES = 10;
        private int _frameCounter;

        public BlockPreFallState()
        {
            _frameCounter = 0;
        }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            // TODO: Handle case where block below is in prefall state
            if (++_frameCounter == PAUSE_FRAMES)
            {
                return new BlockFallingState();
            }
            return this;
        }
    }
}
