namespace ExoLab.Input
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
