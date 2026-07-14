namespace ExoLab.Input
{
    using System.Collections.Generic;
    using UnityEngine;

    public class CommandManager : MonoBehaviour
    {
        private Stack<ICommand> undoStack = new Stack<ICommand>();
        private Stack<ICommand> redoStack = new Stack<ICommand>();

        /// <summary>
        /// Can he cancel actions
        /// </summary>
        public bool CanUndo => undoStack.Count > 0;

        private void OnEnable()
        {
            InteractionInputController.OnEscapePressed += Undo;
            
        }

        private void OnDisable()
        {
            InteractionInputController.OnEscapePressed -= Undo;
        }

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            this.undoStack.Push(command);

            // Если мы сделали новое действие после отмены, 
            // история для Redo должна очиститься
            this.redoStack.Clear();
        }

        private void Undo()
        {
            if (this.undoStack.Count > 0)
            {
                ICommand command = undoStack.Pop();
                command.Undo();
                this.redoStack.Push(command);
            }
        }

        private void Redo()
        {
            if (this.redoStack.Count > 0)
            {
                ICommand command = this.redoStack.Pop();
                command.Execute();
                this.undoStack.Push(command);
            }
        }
    }
}
