using System;
using System.Collections.Generic;

namespace Utils
{
    // TODO unit tests...
    public class SmartStack<T> where T : ISmartStackElement
    {
        private readonly List<T> _stack = new List<T>();
        public int Size { get; private set; }

        public bool Empty => Size == 0;

        public T Peek => _stack[Size - 1];

        public void Push (T element) {
            // Deactivate old
            if (!Empty) { Peek.Deactivate(); }

            // OnPushed new
            element.OnPushed();

            // Activate new
            element.Activate();
            _stack.Add(element);
            Size++;
        }

        public T Pop () {
            // TODO add trypop?
            if (Empty) { throw new InvalidOperationException("Popping an empty stack!"); }

            // Deactivate top
            Peek.Deactivate();

            // OnPopped top
            Peek.OnPopped();

            // Remove top
            var element = _stack[Size - 1];
            _stack.RemoveAt(Size - 1);

            // Activate new top
            if (!Empty) { Peek.Activate(); }

            return element;
        }
    }

    public interface ISmartStackElement
    {
        void OnPushed ();
        void Activate ();
        void Deactivate ();
        void OnPopped ();
    }
}