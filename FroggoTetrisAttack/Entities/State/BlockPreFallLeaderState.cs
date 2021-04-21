namespace FroggoTetrisAttack.Entities.State
{
    public class BlockPreFallLeaderState : BlockState
    {
        private const int PAUSE_FRAMES = 10;
        private int _frameCounter;

        public BlockPreFallLeaderState()
        {
            _frameCounter = 0;
        }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            if (Context.Bottom?.BType == Block.BlockType.Empty)
            {
                if (++_frameCounter == PAUSE_FRAMES)
                {
                    return new BlockFallingState();
                }
            }
            return this;
        }
    }
}
