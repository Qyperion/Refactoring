using System;

namespace Refactoring.Models
{
    public record FinderResult
    {
        public Person Person1 { get; init; }
        public Person Person2 { get; init; }
        public TimeSpan DateOfBirthDifference => Person2.DateOfBirth - Person1.DateOfBirth;
    }
}