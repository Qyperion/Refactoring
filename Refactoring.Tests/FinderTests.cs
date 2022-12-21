using System;
using System.Collections.Generic;
using Refactoring.Enums;
using Refactoring.Models;
using Xunit;

namespace Refactoring.Tests
{
    public class FinderTests
    {
        [Theory]
        [MemberData(nameof(FinderTestData.PersonsWithMaxDateDifferenceType), MemberType = typeof(FinderTestData))]
        [MemberData(nameof(FinderTestData.PersonsWithMinDateDifferenceType), MemberType = typeof(FinderTestData))]
        [MemberData(nameof(FinderTestData.PersonsWithLessThanTwoRecords), MemberType = typeof(FinderTestData))]
        public void TestPersonsFinder(DateDifferenceType dateDifferenceType, List<Person> persons, PersonFinderResult expected)
        {
            // Arrange
            var finder = new PersonsFinder(persons);

            // Act
            var actual = finder.Find(dateDifferenceType);

            // Assert
            Assert.Equal(expected, actual);

            if (actual.Person1 != null && actual.Person2 != null)
                Assert.Equal(actual.DateOfBirthDifference, actual.Person2.DateOfBirth - actual.Person1.DateOfBirth);
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
                new PersonFinderResult
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
                new PersonFinderResult
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
                new PersonFinderResult
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
                new PersonFinderResult
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
                new PersonFinderResult
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
                new PersonFinderResult
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
                new PersonFinderResult()
            },
            new object[] 
            { 
                DateDifferenceType.Min, 
                new List<Person>
                {
                    new() { Name = "Name1", DateOfBirth = new DateTime(2018, 10, 10) }
                },
                new PersonFinderResult()
            }
        };
    }
}
