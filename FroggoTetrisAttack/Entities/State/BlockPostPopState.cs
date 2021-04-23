namespace FroggoTetrisAttack.Entities.State
{
    public class BlockPostPopState : ABlockClearState
    {
        private int _restTimer;

        public BlockPostPopState(int PopIndex, int TotalInMatch) : base(PopIndex, TotalInMatch) { }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override void OnExit(Block Target)
        {
            Target.ChangeType(Block.BlockType.Empty);
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            if (++_restTimer == SERIAL_POP_DELAY * (_totalInMatch - _popIndex))
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
