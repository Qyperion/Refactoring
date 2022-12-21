using System;

namespace Refactoring.Models
{
    public record Person : IDateProvider
    {
        public string Name { get; init; }
        public DateTime DateOfBirth { get; init; }

        public DateTime GetDate() => DateOfBirth;
    }
}