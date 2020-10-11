using System.Collections.Generic;
using System.Collections;
using System;

namespace list
{
    public class Program
    {
        static void Main(string[] args)
        {
            DoubleLinkedList<Minion> minionsList = new DoubleLinkedList<Minion>();
           
            minionsList.Add(new Minion(1, "Kevin", 13, 2));
            minionsList.Add(new Minion(2, "Bob", 16, 3));
            minionsList.Add(new Minion(3, "Ben", 11, 1));

            minionsList.AddFirst(new Minion(4, "Stuart", 20, 4));
            foreach (var item in minionsList)
            {
                Console.WriteLine(item);
            }
            //Удаление элемента списка с индексом 2
            minionsList.Remove(2);

            //Вывод списка в обратном порядке
            foreach (var t in minionsList.BackEnumerator())
            {
                Console.WriteLine(t);
            }
        }
    }
    public class Minion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int TownId { get; set; }

        public Minion(int id, string name, int age, int townId)
        {
            Id = id;
            Name = name;
            Age = age;
            TownId = townId;
        }

        public override string ToString()
        {
            return
                $"{{ {nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Age)}: {Age}, {nameof(TownId)}: {TownId} }}";
        }
    }

    //Узел
    public class DoubleNode<T>
    {
        public DoubleNode(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public DoubleNode<T> Previous { get; set; }
        public DoubleNode<T> Next { get; set; }

        public override string ToString()
        {
            return $"{nameof(Data)}: {Data}";
        }
    }
    //Список
    public class DoubleLinkedList<T> : IEnumerable<T>  // двусвязный список
    {
        DoubleNode<T> head; // головной/первый элемент
        DoubleNode<T> tail; // последний/хвостовой элемент
        int count;  // количество элементов в списке

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