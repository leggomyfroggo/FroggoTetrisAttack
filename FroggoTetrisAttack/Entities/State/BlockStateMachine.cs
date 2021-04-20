using WarnerEngine.Lib.State;

namespace FroggoTetrisAttack.Entities.State
{
    public class BlockStateMachine : StateMachine<BlockState, Block, BlockState.BlockStateType>
    {
        public BlockStateMachine(BlockState InitialState, Block Target) : base(InitialState, Target) { }
    }
}
