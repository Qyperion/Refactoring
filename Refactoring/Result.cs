using System;

namespace Refactoring
{
    public class Result
    {
        public string Name { get; set; }
        public DateTime DoB { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            return obj != null && Equals(obj as Result);
        }

        public bool Equals(Result other)
        {
            return other != null
                && Name == other.Name 
                && DoB.Equals(other.DoB);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, DoB);
        }
    }
}