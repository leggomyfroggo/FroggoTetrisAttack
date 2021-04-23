namespace FroggoTetrisAttack.Entities.State
{
    public class BlockPreClearState : ABlockClearState
    {
        private const int FLASH_TIME = 60;

        private int _flashTimer;

        public BlockPreClearState(int PopIndex, int TotalInMatch) : base(PopIndex, TotalInMatch) { }

        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            if (++_flashTimer == FLASH_TIME)
            {
                return new BlockPopState(_popIndex, _totalInMatch);
            }
            return this;
        }

        public override Block.BlockFace GetBlockFace()
        {
            if (_flashTimer % 4 < 2)
            {
                return Block.BlockFace.Flash1;
            }
            return Block.BlockFace.Flash2;
        }
    }
}
