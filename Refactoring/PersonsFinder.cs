using System;
using System.Collections.Generic;
using System.Linq;
using Refactoring.Enums;
using Refactoring.Models;

namespace Refactoring
{
    public class PersonsFinder
    {
        private readonly List<Person> _persons;

        public PersonsFinder(List<Person> persons)
        {
            _persons = persons;
        }

        /// <summary>
        /// Find pair of persons that have min/max birth of date difference
        /// </summary>
        /// <param name="dateDifferenceType">Date difference type (Min or Max)</param>
        /// <returns cref="PersonFinderResult">Result that contains pair of persons and date of birth difference</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public PersonFinderResult Find(DateDifferenceType dateDifferenceType)
        {
            // Якщо кількість персон менше 2, то повертаємо пустий результат, щоб зберегти поведінку оригінального методу.
            // Але краще було би повертати null або кидати якийсь ексепшен
            if (_persons.Count <= 1)
                return new PersonFinderResult();

            // В оригінальній реалізації методу алгоритмічна складність була O(N^2), а використання пам'яті O(N*logN)
            // В новій реалізації алгоритмічна складність O(N*logN), а використання пам'яті O(N)
            // Спочатку сортуємо список персон по даті народження.
            var personsSortedByDoB = _persons.OrderBy(x => x.DateOfBirth).ToList();

            PersonFinderResult result = dateDifferenceType switch
            {
                // Для пошуку мінімальної різниці між датами народження проходимо по відсортованому масиві і шукаємо найменшу різницю дат народження сусідніх персон
                DateDifferenceType.Min => FindMinDateOfBirthDiff(personsSortedByDoB),
                // Для пошуку максимальної різниці між датами народження просто рахуємо різницю між першою та останьою персоною у відсортованому масиві
                DateDifferenceType.Max => FindMaxDateOfBirthDiff(personsSortedByDoB),

                _ => throw new ArgumentOutOfRangeException(nameof(dateDifferenceType), dateDifferenceType, null)
            };

            // В оригінальній реалізації при наявності декількох персон з однією датою народження поверталася пара персон, котрі зустрічаються самі перші в масиві.
            // Але оскільки ми відсортували оригінальний масив, то вишеописана поведінка не гарантується.
            // Тому для збереження оригінальної поведінки проходимося послідовно по масиву з оригінальним порядком і шукаємо перші входження по даті народження знайдених пар персон.
            // Якщо для клієнтів даного методу підходить будь-яка пара персон з min/max різницею дат народження, то метод GetResultWithOriginalOrder можна видалити.
            return GetResultWithOriginalOrder(result, dateDifferenceType);
        }

        private PersonFinderResult GetResultWithOriginalOrder(PersonFinderResult result, DateDifferenceType dateDifferenceType)
        {
            if (_persons.Count == 2)
                return result;

            Person person1 = null, person2 = null;

            foreach (var person in _persons)
            {
                if (person1 == null && person.DateOfBirth == result.Person1.DateOfBirth)
                {
                    person1 = person;
                    continue;
                }

                if (person2 == null && person.DateOfBirth == result.Person2.DateOfBirth)
                {
                    person2 = person;
                    
                    if (person1 != null)
                        break;
                }
            }

            return dateDifferenceType switch
            {
                DateDifferenceType.Min => new PersonFinderResult(person2, person1),
                DateDifferenceType.Max => new PersonFinderResult(person1, person2),
                _ => throw new ArgumentOutOfRangeException(nameof(dateDifferenceType), dateDifferenceType, null)
            };
        }

        private static PersonFinderResult FindMinDateOfBirthDiff(IReadOnlyList<Person> persons)
        {
            TimeSpan minDateOfBirthDiff = TimeSpan.MaxValue;
            PersonFinderResult result = null;

            for (int i = 0; i < persons.Count - 1; i++)
            {
                var currentPerson = persons[i];
                var nextPerson = persons[i + 1];
                var currentDateOfBirthDiff = nextPerson.DateOfBirth - currentPerson.DateOfBirth;

                if (currentDateOfBirthDiff < minDateOfBirthDiff)
                {
                    result = new PersonFinderResult(currentPerson, nextPerson);
                    minDateOfBirthDiff = currentDateOfBirthDiff;
                }
            }

            return result;
        }

        private static PersonFinderResult FindMaxDateOfBirthDiff(IReadOnlyCollection<Person> persons)
        {
            return new PersonFinderResult(persons.First(), persons.Last());
        }
    }
}