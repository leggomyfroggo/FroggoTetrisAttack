namespace FroggoTetrisAttack.Entities.State
{
    public class BlockReadyState : BlockState
    {
        public override BlockState ConsiderStateChange(BlockState CandidateState, Block Target)
        {
            if (CandidateState is BlockSwappingState state) 
            {
                return state;
            }
            return this;
        }

        public override BlockState Update(float DT, Block Target, BlockContext Context)
        {
            if (Target.BType != Block.BlockType.Empty && Context.Bottom?.BType == Block.BlockType.Empty)
            {
                return new BlockPreFallState();
            }
            return this;
        }
    }
}
