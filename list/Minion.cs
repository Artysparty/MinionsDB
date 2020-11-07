using System;
using System.Collections.Generic;
using System.Text;

namespace list
{
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
        public int CompareTo(Minion other)
        {
            return this.Id.CompareTo(other.Id);
        }

        public override string ToString()
        {
            return
                $"{{ {nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Age)}: {Age}, {nameof(TownId)}: {TownId} }}";
        }
    }
}
