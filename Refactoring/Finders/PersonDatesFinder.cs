using System.Collections.Generic;
using Refactoring.Models;

namespace Refactoring.Finders
{
    public class PersonDatesFinder : DateExtremumPairFinder<Person>
    {
        public PersonDatesFinder(List<Person> elements) : base(elements) { }
    }
}