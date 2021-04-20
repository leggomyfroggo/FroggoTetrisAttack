using WarnerEngine.Lib.State;

namespace FroggoTetrisAttack.Entities.State
{
    public class PlayFieldStateMachine : StateMachine<PlayFieldState, PlayField, PlayFieldState.PlayFieldStateType>
    {
        public PlayFieldStateMachine(PlayFieldState InitialState, PlayField Target) : base(InitialState, Target) { }
    }
}
