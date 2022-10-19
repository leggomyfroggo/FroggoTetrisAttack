namespace FroggoTetrisAttack.Entities.State
{
    public class BlockFallingState : BlockState
    {
        public const int FALL_SPEED = 8;

        private int _offset;
        private bool _latch;

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
            else
            {
                _offset += FALL_SPEED;
                if (_offset >= Block.BLOCK_SIZE)
                {
                    _latch = true;
                    _offset -= Block.BLOCK_SIZE;
                }
            }
            return this;
        }

        public override SwapDirection GetSwapDirection(bool Peak = false)
        {
            if (_latch)
            {
                _latch = false;
                return SwapDirection.Down;
            }
            else if (Peak)
            {
                return SwapDirection.Down;
            }
            return SwapDirection.None;
        }

        public override int GetSwapOffsetMagnitude()
        {
            return _offset;
        }

        public override Block.BlockFace GetBlockFace()
        {
            return Block.BlockFace.Fall;
        }
    }
}
