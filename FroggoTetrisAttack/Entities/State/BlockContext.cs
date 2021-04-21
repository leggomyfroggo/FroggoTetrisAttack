namespace FroggoTetrisAttack.Entities.State
{
    public class BlockContext
    {
        public readonly Block Top;
        public readonly Block Right;
        public readonly Block Bottom;
        public readonly Block Left;

        public BlockContext(Block Top, Block Right, Block Bottom, Block Left)
        {
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
            this.Left = Left;
        }
    }
}
