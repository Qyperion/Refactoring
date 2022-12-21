using System;
using Refactoring.Models;

namespace Refactoring.Finders
{
    public record FinderResult<T> where T : IDateProvider
    {
        public FinderResult() { }

        public FinderResult(T element1, T element2)
        {
            Element1 = element1;
            Element2 = element2;
            DateDifference = element2.GetDate() - element1.GetDate();
        }

        public T Element1 { get; }
        public T Element2 { get; }
        public TimeSpan DateDifference { get; }
    }
}