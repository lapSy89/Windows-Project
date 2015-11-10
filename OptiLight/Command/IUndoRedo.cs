namespace OptiLight.Command {

    public interface IUndoRedo {
        void Execute();
        void UnExecute();
    }
}
