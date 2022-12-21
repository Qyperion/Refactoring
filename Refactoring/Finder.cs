using System.Collections.Generic;
using Refactoring.Enums;
using Refactoring.Models;

namespace Refactoring
{
    public class Finder
    {
        private readonly List<Person> _persons;

        public Finder(List<Person> persons)
        {
            _persons = persons;
        }

        public FinderResult Find(DateDifferenceType dateDifferenceType)
        {
            var results = new List<FinderResult>();

            for (int i = 0; i < _persons.Count - 1; i++)
            {
                for (int j = i + 1; j < _persons.Count; j++)
                {
                    FinderResult result;
                    if (_persons[i].DateOfBirth < _persons[j].DateOfBirth)
                    {
                        result = new FinderResult
                        {
                            Person1 = _persons[i],
                            Person2 = _persons[j]
                        };
                    }
                    else
                    {
                        result = new FinderResult
                        {
                            Person1 = _persons[j],
                            Person2 = _persons[i]
                        };
                    }

                    results.Add(result);
                }
            }

            if (results.Count < 1)
            {
                return new FinderResult();
            }

            var extremumResult = results[0];
            for (int i = 0; i < results.Count; i++)
            {
                switch (dateDifferenceType)
                {
                    case DateDifferenceType.Min:
                        if (results[i].DateOfBirthDifference < extremumResult.DateOfBirthDifference)
                        {
                            extremumResult = results[i];
                        }
                        break;

                    case DateDifferenceType.Max:
                        if (results[i].DateOfBirthDifference > extremumResult.DateOfBirthDifference)
                        {
                            extremumResult = results[i];
                        }
                        break;
                }
            }

            return extremumResult;
        }
    }
}