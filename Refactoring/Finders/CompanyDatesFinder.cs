using Refactoring.Models;
using System.Collections.Generic;

namespace Refactoring.Finders
{
    public class CompanyDatesFinder  : DateExtremumPairFinder<Company>
    {
        public CompanyDatesFinder(List<Company> elements) : base(elements) { }
    }
}
