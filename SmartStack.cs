using System.Collections.Generic;

namespace Code.Utils
{
    public class SmartStack<T> where T : ISmartStackElement
    {
        private List<T> _stack = new List<T>();
        public int Size { get; private set; }

        public T Peek () {
            return _stack[Size - 1];
        }

        public void Push (T element) {
            _stack.Add(element);
            element.OnPushed();
            Size++;
        }

        public T Pop () {
            
            
            var element = _stack[Size-1];
            _stack.RemoveAt(Size-1);
            return element;
        }
    }

    public interface ISmartStackElement
    {
        void Activate ();
        void OnPushed ();
        void OnPopped ();
        void Deactivate ();
    }
}