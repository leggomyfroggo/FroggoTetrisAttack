namespace FroggoTetrisAttack.Entities.State
{
    public class BlockPopState : ABlockClearState
    {
        private int _popTimer;

        public BlockPopState(int PopIndex, int TotalInMatch) : base(PopIndex, TotalInMatch) { }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            if (++_popTimer == (_popIndex + 1) * SERIAL_POP_DELAY)
            {
                return new BlockPostPopState(_popIndex, _totalInMatch);
            }
            return this;
        }

        public override Block.BlockFace GetBlockFace()
        {
            return Block.BlockFace.Dead;
        }
    }
}
