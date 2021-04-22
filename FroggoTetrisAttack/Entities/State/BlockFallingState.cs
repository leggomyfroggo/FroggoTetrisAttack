namespace FroggoTetrisAttack.Entities.State
{
    public class BlockFallingState : BlockState
    {
        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            var bottomBlock = Context.Bottom;
            // TODO: Handle case where block below is in pre-fall
            if (
                bottomBlock == null ||
                (
                    bottomBlock.BType != Block.BlockType.Empty &&
                    !(bottomBlock.StateMachine.CurrentState is BlockFallingState)
                )
            )
            {
                return new BlockReadyState();
            }
            return this;
        }

        public override SwapDirection GetSwapDirection(bool Peak = false)
        {
            return SwapDirection.Down;
        }
    }
}
