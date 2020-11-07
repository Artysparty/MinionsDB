using System;
using System.Collections.Generic;
using System.Text;

namespace list
{
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
}
