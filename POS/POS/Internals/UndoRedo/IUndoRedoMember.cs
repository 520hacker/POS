// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru
using System;

namespace POS.Internals.UndoRedo
{
    public interface IUndoRedoMember
    {
        void OnCommit(object change);
        void OnUndo(object change);
        void OnRedo(object change);
    }
}