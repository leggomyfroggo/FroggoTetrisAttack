using Microsoft.Xna.Framework;

namespace FroggoTetrisAttack.Entities.State
{
    public abstract class BlockState
    {
        public enum SwapDirection { None, Up, Right, Down, Left }

        public virtual void OnEnter(Block Target) { }

        public virtual void OnExit(Block Target) { }

        public abstract BlockState Update(float DT, Block Target, BlockContext Context);

        public abstract BlockState ConsiderStateChange(BlockState CandidateState, Block Target);

        public virtual SwapDirection GetSwapDirection(bool Peak = false)
        {
            return SwapDirection.None;
        }

        public virtual int GetSwapOffsetMagnitude()
        {
            return 0;
        }

        public Vector2 GetSwapOffset()
        {
            switch (GetSwapDirection(Peak: true))
            {
                case SwapDirection.Up:
                    return new Vector2(0, -1) * GetSwapOffsetMagnitude();
                case SwapDirection.Right:
                    return new Vector2(1, 0) * GetSwapOffsetMagnitude();
                case SwapDirection.Down:
                    return new Vector2(0, 1) * GetSwapOffsetMagnitude();
                case SwapDirection.Left:
                    return new Vector2(-1, 0) * GetSwapOffsetMagnitude();
            }
            return Vector2.Zero;
        }
    }
}
