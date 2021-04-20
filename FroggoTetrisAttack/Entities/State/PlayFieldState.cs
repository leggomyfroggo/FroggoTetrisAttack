using WarnerEngine.Lib.State;

namespace FroggoTetrisAttack.Entities.State
{
    public abstract class PlayFieldState : StateBase<PlayFieldState, PlayField, PlayFieldState.PlayFieldStateType>
    {
        public enum PlayFieldStateType { Active, Paused }
    }
}
