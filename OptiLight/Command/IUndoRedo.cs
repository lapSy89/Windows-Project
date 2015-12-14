namespace OptiLight.Command {

    // Interface for commands to use Undo Redo controller
    public interface IUndoRedo {
        void Execute();
        void UnExecute();
    }
}
