namespace FroggoTetrisAttack.Entities.State
{
    public class BlockPostPopState : ABlockClearState
    {
        private int _restTimer;

        public BlockPostPopState(int PopIndex, int TotalInMatch) : base(PopIndex, TotalInMatch) { }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            if (CandidateState is BlockReadyState)
            {
                return CandidateState;
            }
            return this;
        }

        public override void OnEnter(Block Target)
        {
            if (_popIndex == _totalInMatch - 1)
            {
                Target.StateMachine.ConsiderStateChange(new BlockReadyState());
            }
        }

        public override void OnExit(Block Target)
        {
            Target.ChangeType(Block.BlockType.Empty);
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            if (++_restTimer == SERIAL_POP_DELAY * (_totalInMatch - _popIndex - 1))
            {
                return new BlockReadyState();
            }
            return this;
        }

        public override bool IsVisible()
        {
            return false;
        }
    }
}
