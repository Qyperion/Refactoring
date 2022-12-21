using System;

namespace Refactoring.Models
{
    public record PersonFinderResult
    {
        public PersonFinderResult() { }

        public PersonFinderResult(Person person1, Person person2)
        {
            Person1 = person1;
            Person2 = person2;
            DateOfBirthDifference = Person2.DateOfBirth - Person1.DateOfBirth;
        }

        public Person Person1 { get; }
        public Person Person2 { get; }
        public TimeSpan DateOfBirthDifference { get; }
    }
}