namespace FroggoTetrisAttack.Entities.State
{
    public class PlayFieldActiveState : PlayFieldState
    {
        public override PlayFieldStateType GetStateType()
        {
            return PlayFieldStateType.Active;
        }

        public override void Enter(PlayField Target, PlayFieldState PreviousState) { }

        public override void Exit(PlayField Target, PlayFieldState IncomingState) { }        

        public override PlayFieldState Update(PlayField Target, float DT)
        {
            return this;
        }

        public override PlayFieldState ConsiderStateChange(PlayFieldState CandidateState, PlayField Target)
        {
            return this;
        }
    }
}
