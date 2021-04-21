namespace FroggoTetrisAttack.Entities.State
{
    public class BlockStateMachine
    {
        private Block _target;

        public BlockState CurrentState { get; private set; }

        public BlockStateMachine(BlockState InitialState, Block Target)
        {
            _target = Target;
            CurrentState = InitialState;
            CurrentState.OnEnter(_target);
        }

        public void Update(float DT, BlockContext Context)
        {
            var maybeNewState = CurrentState.Update(DT, _target, Context);
            if (maybeNewState != CurrentState)
            {
                CurrentState.OnExit(_target);
                CurrentState = maybeNewState;
                CurrentState.OnEnter(_target);
            }
        }

        public void ConsiderStateChange(BlockState CandidateState)
        {
            var maybeNewState = CurrentState.ConsiderStateChange(CandidateState, _target);
            if (maybeNewState != CurrentState)
            {
                CurrentState.OnExit(_target);
                CurrentState = maybeNewState;
                CurrentState.OnEnter(_target);
            }
        }
    }
}
