namespace FroggoTetrisAttack.Entities.State
{
    public abstract class BlockState
    {
        public enum SwapDirection { None, Up, Right, Down, Left }

        public virtual void OnEnter(Block Target) { }

        public virtual void OnExit(Block Target) { }

        public abstract BlockState Update(float DT, Block Target, BlockContext Context);

        public abstract BlockState ConsiderStateChange(BlockState CandidateState, Block Target);

        public virtual SwapDirection GetSwapDirection()
        {
            return SwapDirection.None;
        }
    }
}
