using System;

namespace Refactoring.Models
{
    public record Person
    {
        public string Name { get; init; }
        public DateTime DateOfBirth { get; init; }
    }
}