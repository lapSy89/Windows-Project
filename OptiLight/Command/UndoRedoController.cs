using System;
using System.Collections.Generic;
using System.Linq;

namespace OptiLight.Command {

    public class UndoRedoController {
        
        //Undo / Redo stacks
        private readonly Stack<IUndoRedo> undoStack = new Stack<IUndoRedo>();
        private readonly Stack<IUndoRedo> redoStack = new Stack<IUndoRedo>();

        //We use the Singleton design pattern for our constructor
        public static UndoRedoController Instance { get; } = new UndoRedoController();
        private UndoRedoController() { }

        //Used for adding the Undo/Redo command to the "undoStack", clears the "redoStack",
        // and executes the command
        public void AddAndExecute(IUndoRedo command) {
            undoStack.Push(command);
            redoStack.Clear();
            command.Execute();
        }

        //This informs the View when the Undo command can be used
        public bool CanUndo() => undoStack.Any();

        //Undoes the Undo/Redo command that was last executed, if possible
        public void Undo() {
            if (!undoStack.Any()) throw new InvalidOperationException();
            var command = undoStack.Pop();
            redoStack.Push(command);
            command.UnExecute();
        }

        //This informs the View when the Redo command can be used
        public bool CanRedo() => redoStack.Any();

        //Redoes the Undo/Redo command that was last unexecuted, if possible
        public void Redo() {
            if (!redoStack.Any()) throw new InvalidOperationException();
            var command = redoStack.Pop();
            undoStack.Push(command);
            command.Execute();
        }
    }
}
