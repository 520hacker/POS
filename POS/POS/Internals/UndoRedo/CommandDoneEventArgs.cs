using System;

namespace POS.Internals.UndoRedo
{
    public class CommandDoneEventArgs : EventArgs
    {
        public readonly CommandDoneType CommandDoneType;

        public CommandDoneEventArgs(CommandDoneType type)
        {
            this.CommandDoneType = type;
        }
    }
}