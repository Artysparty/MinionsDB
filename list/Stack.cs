using System;
using System.Collections.Generic;
using System.Text;

namespace Stack
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace Task1
    {
        public class NewStack<T> where T : class
        {
            public T[] Mas { get; set; }
            public int Amount { get; set; }
            public int Capacity { get; set; }

            public NewStack(int capacity)
            {
                Mas = new T[capacity];
                Capacity = capacity;
                Amount = 0;
            }

            public NewStack() : this(4)
            {
            }

            private void Resize(int capacity)
            {
                T[] array = new T[capacity];
                Array.Copy(Mas, array, Capacity);
                Capacity = capacity;
                Mas = array;
            }

            public void Push(T element)
            {
                if (Amount == Capacity)
                {
                    Resize(Capacity * 2);
                }

                Mas[Amount++] = element;
            }

            public T Pop()
            {
                if (Amount == 0)
                {
                    throw new InvalidOperationException("Error: stack should be not empty");
                }
                Amount--;
                T element = Mas[Amount];
                Mas[Amount] = null;
                return element;
            }

            public IEnumerator<T> GetEnumerator()
            {
                int amount = Amount;
                for (int i = 0; i < amount; i++)
                {
                    yield return Pop();
                }
            }
        }
    }
}
