using Refactoring.Enums;
using Refactoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Refactoring.Finders
{
    public class DateExtremumPairFinder<T> where T : class, IDateProvider
    {
        private readonly List<T> _elements;

        protected DateExtremumPairFinder(List<T> elements)
        {
            _elements = elements;
        }

        /// <summary>
        /// Find pair of elements that have min/max date difference
        /// </summary>
        /// <param name="dateDifferenceType">Date difference type (Min or Max)</param>
        /// <returns cref="FinderResult{T}">Result that contains pair of elements and date difference</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public FinderResult<T> Find(DateDifferenceType dateDifferenceType)
        {
            // Якщо кількість елементів менше 2, то повертаємо пустий результат, щоб зберегти поведінку оригінального методу.
            // Але краще було би повертати null або кидати якийсь ексепшен
            if (_elements.Count <= 1)
                return new FinderResult<T>();

            // В оригінальній реалізації методу алгоритмічна складність була O(N^2), а використання пам'яті O(N*logN)
            // В новій реалізації алгоритмічна складність O(N*logN), а використання пам'яті O(N)
            // Спочатку сортуємо список елементів по даті.
            var elementsSortedByDate = _elements.OrderBy(x => x.GetDate()).ToList();

            FinderResult<T> result = dateDifferenceType switch
            {
                // Для пошуку мінімальної різниці між датами проходимо по відсортованому масиві і шукаємо найменшу різницю дат сусідніх елементів
                DateDifferenceType.Min => FindPairWithMinDateDiff(elementsSortedByDate),
                // Для пошуку максимальної різниці між датами просто рахуємо різницю між першим та останім елементом у відсортованому масиві
                DateDifferenceType.Max => FindPairWithMaxDateDiff(elementsSortedByDate),

                _ => throw new ArgumentOutOfRangeException(nameof(dateDifferenceType), dateDifferenceType, null)
            };

            // В оригінальній реалізації при наявності декількох елементів з однією датою поверталася пара елементів, котрі зустрічаються самі перші в масиві.
            // Але оскільки ми відсортували оригінальний масив, то вишеописана поведінка не гарантується.
            // Тому для збереження оригінальної поведінки проходимося послідовно по масиву з оригінальним порядком і шукаємо перші входження по даті знайдених пар елементів.
            // Якщо для клієнтів даного методу підходить будь-яка пара елементів з min/max різницею дат, то метод GetResultWithOriginalOrder можна видалити.
            return GetResultWithOriginalOrder(result, dateDifferenceType);
        }

        private FinderResult<T> GetResultWithOriginalOrder(FinderResult<T> result, DateDifferenceType dateDifferenceType)
        {
            if (_elements.Count == 2)
                return result;

            T element1 = null, element2 = null;

            foreach (var element in _elements)
            {
                if (element1 == null && element.GetDate() == result.Element1.GetDate())
                {
                    element1 = element;
                    continue;
                }

                if (element2 == null && element.GetDate() == result.Element2.GetDate())
                {
                    element2 = element;

                    if (element1 != null)
                        break;
                }
            }

            return dateDifferenceType switch
            {
                DateDifferenceType.Min => new FinderResult<T>(element2, element1),
                DateDifferenceType.Max => new FinderResult<T>(element1, element2),
                _ => throw new ArgumentOutOfRangeException(nameof(dateDifferenceType), dateDifferenceType, null)
            };
        }

        private static FinderResult<T> FindPairWithMinDateDiff(IReadOnlyList<T> elements)
        {
            TimeSpan minDateDiff = TimeSpan.MaxValue;
            FinderResult<T> result = null;

            for (int i = 0; i < elements.Count - 1; i++)
            {
                var currentElement = elements[i];
                var nextElement = elements[i + 1];
                var currentDateDiff = nextElement.GetDate() - currentElement.GetDate();

                if (currentDateDiff < minDateDiff)
                {
                    result = new FinderResult<T>(currentElement, nextElement);
                    minDateDiff = currentDateDiff;
                }
            }

            return result;
        }

        private static FinderResult<T> FindPairWithMaxDateDiff(IReadOnlyCollection<T> elements)
        {
            return new FinderResult<T>(elements.First(), elements.Last());
        }
    }
}
