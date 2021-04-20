using WarnerEngine.Lib.State;

namespace FroggoTetrisAttack.Entities.State
{
    public abstract class BlockState : StateBase<BlockState, Block, BlockState.BlockStateType>
    { 
        public enum BlockStateType { Ready, Swapping, Flashing, PrePop, Popping, PostPop, PreFall, Falling }
    }
}
