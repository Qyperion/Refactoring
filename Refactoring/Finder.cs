using System.Collections.Generic;

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

            for(var i = 0; i < _persons.Count - 1; i++)
            {
                for(var j = i + 1; j < _persons.Count; j++)
                {
                    var result = new FinderResult();
                    if(_persons[i].DateOfBirth < _persons[j].DateOfBirth)
                    {
                        result.Person1 = _persons[i];
                        result.Person2 = _persons[j];
                    }
                    else
                    {
                        result.Person1 = _persons[j];
                        result.Person2 = _persons[i];
                    }
                    result.DateOfBirthDifference = result.Person2.DateOfBirth - result.Person1.DateOfBirth;
                    results.Add(result);
                }
            }

            if(results.Count < 1)
            {
                return new FinderResult();
            }

            var extremumResult  = results[0];
            for(var i = 0; i < results.Count; i++)
            {
                switch(dateDifferenceType)
                {
                    case DateDifferenceType.Min:
                        if(results[i].DateOfBirthDifference < extremumResult.DateOfBirthDifference)
                        {
                            extremumResult = results[i];
                        }
                        break;

                    case DateDifferenceType.Max:
                        if(results[i].DateOfBirthDifference > extremumResult.DateOfBirthDifference)
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