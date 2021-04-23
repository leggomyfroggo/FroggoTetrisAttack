namespace FroggoTetrisAttack.Entities.State
{
    public class BlockPopState : ABlockClearState
    {
        private int _popTimer;

        public BlockPopState(int PopIndex, int TotalInMatch) : base(PopIndex, TotalInMatch) { }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            if (CandidateState is BlockPostPopState)
            {
                return CandidateState;
            }
            return this;
        }

        public override void OnEnter(Block Target)
        {
            if (_popIndex == 0)
            {
                Target.StateMachine.ConsiderStateChange(new BlockPostPopState(_popIndex, _totalInMatch));
            }
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            if (++_popTimer == _popIndex * SERIAL_POP_DELAY)
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
