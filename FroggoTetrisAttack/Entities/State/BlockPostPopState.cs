namespace FroggoTetrisAttack.Entities.State
{
    public class BlockPostPopState : BlockState
    {
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
            return new BlockReadyState();
        }
    }
}
