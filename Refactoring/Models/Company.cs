using System;

namespace Refactoring.Models
{
    public record Company : IDateProvider
    {
        public string Name { get; init; }
        public DateTime DateOfFoundation { get; init; }
        public DateTime GetDate() => DateOfFoundation;
    }
}
