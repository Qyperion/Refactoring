using System;

namespace Refactoring
{
    public class FinderResult
    {
        public Person Person1 { get; set; }
        public Person Person2 { get; set; }
        public TimeSpan DateOfBirthDifference { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as FinderResult);
        }

        public bool Equals(FinderResult other)
        {
            return other != null
                   && (Person1?.Equals(other.Person1) ?? other.Person1 == null)
                   && (Person2?.Equals(other.Person2) ?? other.Person2 == null)
                   && DateOfBirthDifference.Equals(other.DateOfBirthDifference);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Person1, Person2, DateOfBirthDifference);
        }
    }
}