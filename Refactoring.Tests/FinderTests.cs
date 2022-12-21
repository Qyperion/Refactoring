using System;
using System.Collections.Generic;
using Refactoring.Enums;
using Refactoring.Finders;
using Refactoring.Models;
using Xunit;

namespace Refactoring.Tests
{
    public class FinderTests
    {
        [Theory]
        [MemberData(nameof(FinderTestData.PersonsWithMaxDateDifferenceType), MemberType = typeof(FinderTestData))]
        [MemberData(nameof(FinderTestData.PersonsWithMinDateDifferenceType), MemberType = typeof(FinderTestData))]
        public void TestPersonDatesFinder(DateDifferenceType dateDifferenceType, List<Person> persons, FinderResult<Person> expected)
        {
            // Arrange
            var finder = new PersonDatesFinder(persons);

            // Act
            var actual = finder.Find(dateDifferenceType);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(actual.DateDifference, actual.Element2.DateOfBirth - actual.Element1.DateOfBirth);
        }

        [Theory]
        [MemberData(nameof(FinderTestData.PersonsWithLessThanTwoRecords), MemberType = typeof(FinderTestData))]
        public void TestPersonDatesFinder_WhenPersonsWithLessThanTwoRecords(DateDifferenceType dateDifferenceType, List<Person> persons, FinderResult<Person> expected)
        {
            // Arrange
            var finder = new PersonDatesFinder(persons);

            // Act
            var actual = finder.Find(dateDifferenceType);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(FinderTestData.CompaniesTestData), MemberType = typeof(FinderTestData))]
        public void TestCompanyDatesFinder(DateDifferenceType dateDifferenceType, List<Company> companies, FinderResult<Company> expected)
        {
            // Arrange
            var finder = new CompanyDatesFinder(companies);

            // Act
            var actual = finder.Find(dateDifferenceType);

            // Assert
            Assert.Equal(expected, actual);

            if (actual.Element1 != null && actual.Element2 != null)
                Assert.Equal(actual.DateDifference, actual.Element2.DateOfFoundation - actual.Element1.DateOfFoundation);
        }
    }

    public class FinderTestData
    {
        public static IEnumerable<object[]> PersonsWithMaxDateDifferenceType => new List<object[]>
        {
            new object[]
            {
                DateDifferenceType.Max,
                new List<Person>
                {
                    new() { Name = "Name1", DateOfBirth = new DateTime(2022, 10, 10) },
                    new() { Name = "Name2", DateOfBirth = new DateTime(2022,  6,  4) },
                },
                new FinderResult<Person>
                (
                    new Person { Name = "Name2", DateOfBirth = new DateTime(2022,  6,  4) },
                    new Person { Name = "Name1", DateOfBirth = new DateTime(2022, 10, 10) }
                )
            },
            new object[] 
            { 
                DateDifferenceType.Max, 
                new List<Person>
                {
                    new() { Name = "Name0", DateOfBirth = new DateTime(2018, 10, 10) },
                    new() { Name = "Name1", DateOfBirth = new DateTime(2018, 10, 10) },
                    new() { Name = "Name2", DateOfBirth = new DateTime(2021,  6,  4) },
                    new() { Name = "Name3", DateOfBirth = new DateTime(2022, 12, 31) },
                    new() { Name = "Name4", DateOfBirth = new DateTime(2022,  5, 13) },
                    new() { Name = "Name5", DateOfBirth = new DateTime(2018, 10, 10) },
                    new() { Name = "Name6", DateOfBirth = new DateTime(2022, 12, 31) },
                },
                new FinderResult<Person>
                (
                    new Person { Name = "Name0", DateOfBirth = new DateTime(2018, 10, 10) },
                    new Person { Name = "Name3", DateOfBirth = new DateTime(2022, 12, 31) }
                )
            },
            new object[] 
            { 
                DateDifferenceType.Max, 
                new List<Person>
                {
                    new() { Name = "Name1", DateOfBirth = DateTime.MinValue },
                    new() { Name = "Name2", DateOfBirth = DateTime.Now },
                    new() { Name = "Name3", DateOfBirth = DateTime.MaxValue },
                },
                new FinderResult<Person>
                (
                    new Person { Name = "Name1", DateOfBirth = DateTime.MinValue },
                    new Person { Name = "Name3", DateOfBirth = DateTime.MaxValue }
                )
            }
        };

        public static IEnumerable<object[]> PersonsWithMinDateDifferenceType => new List<object[]>
        {
            new object[]
            {
                DateDifferenceType.Min,
                new List<Person>
                {
                    new() { Name = "Name1", DateOfBirth = new DateTime(2022, 10, 10) },
                    new() { Name = "Name2", DateOfBirth = new DateTime(2022,  6,  4) },
                },
                new FinderResult<Person>
                (
                    new Person { Name = "Name2", DateOfBirth = new DateTime(2022,  6,  4) },
                    new Person { Name = "Name1", DateOfBirth = new DateTime(2022, 10, 10) }
                )
            },
            new object[] 
            { 
                DateDifferenceType.Min, 
                new List<Person>
                {
                    new() { Name = "Name1", DateOfBirth = new DateTime(2022, 12, 31) },
                    new() { Name = "Name2", DateOfBirth = new DateTime(2021,  6,  4) },
                    new() { Name = "Name3", DateOfBirth = new DateTime(2022, 12, 31) },
                    new() { Name = "Name4", DateOfBirth = new DateTime(2022,  5, 13) },
                    new() { Name = "Name5", DateOfBirth = new DateTime(2018, 10, 10) },
                    new() { Name = "Name6", DateOfBirth = new DateTime(2022, 12, 31) },
                },
                new FinderResult<Person>
                (
                    new Person { Name = "Name3", DateOfBirth = new DateTime(2022, 12, 31) },
                    new Person { Name = "Name1", DateOfBirth = new DateTime(2022, 12, 31) }
                )
            },
            new object[] 
            { 
                DateDifferenceType.Min, 
                new List<Person>
                {
                    new() { Name = "Name1", DateOfBirth = DateTime.MinValue },
                    new() { Name = "Name2", DateOfBirth = DateTime.MaxValue },
                    new() { Name = "Name3", DateOfBirth = DateTime.Now },
                    new() { Name = "Name4", DateOfBirth = DateTime.MaxValue },
                },
                new FinderResult<Person>
                (
                    new Person { Name = "Name4", DateOfBirth = DateTime.MaxValue },
                    new Person { Name = "Name2", DateOfBirth = DateTime.MaxValue }
                )
            }
        };

        public static IEnumerable<object[]> PersonsWithLessThanTwoRecords => new List<object[]>
        {
            new object[] 
            { 
                DateDifferenceType.Max, 
                new List<Person>(),
                new FinderResult<Person>()
            },
            new object[] 
            { 
                DateDifferenceType.Min, 
                new List<Person>
                {
                    new() { Name = "Name1", DateOfBirth = new DateTime(2018, 10, 10) }
                },
                new FinderResult<Person>()
            }
        };

        public static IEnumerable<object[]> CompaniesTestData => new List<object[]>
        {
            new object[]
            {
                DateDifferenceType.Max,
                new List<Company>
                {
                    new() { Name = "Microsoft",  DateOfFoundation = new DateTime(1975, 4, 4) },
                    new() { Name = "Apple Inc.", DateOfFoundation = new DateTime(1976, 4, 1) },
                },
                new FinderResult<Company>
                (
                    new Company { Name = "Microsoft",  DateOfFoundation = new DateTime(1975, 4, 4) },
                    new Company { Name = "Apple Inc.", DateOfFoundation = new DateTime(1976, 4, 1) }
                )
            },
            new object[] 
            { 
                DateDifferenceType.Max, 
                new List<Company>
                {
                    new() { Name = "Siemens",     DateOfFoundation = new DateTime(1847, 10, 1) },
                    new() { Name = "Rand Inc.",   DateOfFoundation = new DateTime(1847, 10, 1) },
                    new() { Name = "Microsoft",   DateOfFoundation = new DateTime(1975,  4, 4) },
                    new() { Name = "Xiaomi",      DateOfFoundation = new DateTime(2010,  4, 6) },
                    new() { Name = "Apple Inc.",  DateOfFoundation = new DateTime(1976,  4, 1) },
                    new() { Name = "Tesla, Inc.", DateOfFoundation = new DateTime(2003,  7, 1) },
                },
                new FinderResult<Company>
                (
                    new Company { Name = "Siemens", DateOfFoundation = new DateTime(1847, 10, 1) },
                    new Company { Name = "Xiaomi",  DateOfFoundation = new DateTime(2010,  4, 6) }
                )
            },
            new object[] 
            { 
                DateDifferenceType.Min, 
                new List<Company>
                {
                    new() { Name = "Siemens",     DateOfFoundation = new DateTime(1847, 10, 1) },
                    new() { Name = "Xiaomi",      DateOfFoundation = new DateTime(2010,  4, 6) },
                    new() { Name = "Tesla, Inc.", DateOfFoundation = new DateTime(2003,  7, 1) },
                    new() { Name = "Rand Inc.",   DateOfFoundation = new DateTime(2010,  4, 6) },
                },
                new FinderResult<Company>
                (
                    new Company { Name = "Rand Inc.", DateOfFoundation = new DateTime(2010, 4, 6) },
                    new Company { Name = "Xiaomi",    DateOfFoundation = new DateTime(2010, 4, 6) }
                )
            }
        };
    }
}
