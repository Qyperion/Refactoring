using System;

namespace Refactoring
{
    public class Person
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            return obj != null && Equals(obj as Person);
        }

        public bool Equals(Person other)
        {
            return other != null
                && Name == other.Name 
                && DateOfBirth.Equals(other.DateOfBirth);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, DateOfBirth);
        }
    }
}