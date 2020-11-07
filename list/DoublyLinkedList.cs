using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace list
{
    public class DoubleLinkedList<T> : IEnumerable<T>  // двусвязный список
    {
        DoubleNode<T> head; // головной/первый элемент
        DoubleNode<T> tail; // последний/хвостовой элемент
        int count;  // количество элементов в списке

        public T this[int index]
        {
            get => Get(index);
            set => Set(value, index);
        }

        // добавление элемента
        public void Add(T data)
        {
            DoubleNode<T> node = new DoubleNode<T>(data);

            if (head == null)
                head = node;
            else
            {
                tail.Next = node;
                node.Previous = tail;
            }
            tail = node;
            count++;
        }
        //Добавить в начало
        public void AddFirst(T data)
        {
            DoubleNode<T> node = new DoubleNode<T>(data);
            DoubleNode<T> temp = head;
            node.Next = temp;
            head = node;
            if (count == 0)
                tail = head;
            else
                temp.Previous = node;
            count++;
        }
        //Вернуть запись
        private DoubleNode<T> GetNode(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException($"Index {index} is invalid; Only {Count} elements exist");

            DoubleNode<T> current = head;
            int k = 0;
            while (k < index)
            {
                current = current.Next;
                k++;
            }


            return current;
        }

        public T Get(int index)
        {
            return GetNode(index).Data;
        }
        // Удалить элемент списка
        public void Remove(int index)
        {
            var node = GetNode(index);

            if (head == node)
            {
                if (head.Next != null)
                {
                    head.Next.Previous = null;
                    head = head.Next;
                }
                else
                {
                    head = null;
                }
            }
            else if (tail == node)
            {
                if (tail.Previous != null)
                {
                    tail.Previous.Next = null;
                    tail = tail.Previous;
                }
                else
                {
                    tail = null;
                }
            }
            else
            {
                node.Previous.Next = node.Next;
                node.Next.Previous = node.Previous;
            }

            count--;
        }

        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }
        //Очистить список
        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }
        //Проверка на наличие записи
        public bool Contains(T data)
        {
            DoubleNode<T> current = head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            DoubleNode<T> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        //Установить данные
        public void Set(T data, int index)
        {
            GetNode(index).Data = data;
        }

        public IEnumerable<T> BackEnumerator()
        {
            DoubleNode<T> current = tail;
            while (current != null)
            {
                yield return current.Data;
                current = current.Previous;
            }
        }
    }
}
