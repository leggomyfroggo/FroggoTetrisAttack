namespace FroggoTetrisAttack.Entities.State
{
    public abstract class ABlockClearState : BlockState
    {
        protected const int SERIAL_POP_DELAY = 10;

        protected int _popIndex;
        protected int _totalInMatch;

        public ABlockClearState(int PopIndex, int TotalInMatch)
        {
            _popIndex = PopIndex;
            _totalInMatch = TotalInMatch;
        }
    }
}
