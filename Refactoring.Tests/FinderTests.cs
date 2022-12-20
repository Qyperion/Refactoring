using System;
using System.Collections.Generic;
using Xunit;

namespace Refactoring.Tests
{
    public class FinderTests
    {
        [Theory]
        [MemberData(nameof(FinderTestData.ResultsPlus), MemberType = typeof(FinderTestData))]
        [MemberData(nameof(FinderTestData.ResultsMinus), MemberType = typeof(FinderTestData))]
        [MemberData(nameof(FinderTestData.ResultsEmpty), MemberType = typeof(FinderTestData))]
        public void TestFinder(PT pt, List<Result> results, P expected)
        {
            // Arrange
            var finder = new Finder(results);

            // Act
            var actual = finder.Find(pt); 

            // Assert
            Assert.Equal(expected, actual);
        }
    }

    public class FinderTestData
    {
        public static IEnumerable<object[]> ResultsPlus => new List<object[]>
        {
            new object[] 
            { 
                PT.Plus, 
                new List<Result>
                {
                    new() { Name = "Name1", DoB = new DateTime(2022, 10, 10) },
                    new() { Name = "Name2", DoB = new DateTime(2022,  6,  4) },
                },
                new P
                {
                    P1 = new Result { Name = "Name2", DoB = new DateTime(2022,  6,  4) },
                    P2 = new Result { Name = "Name1", DoB = new DateTime(2022, 10, 10) },
                    D = new DateTime(2022, 10, 10) - new DateTime(2022,  6,  4)
                }
            },
            new object[] 
            { 
                PT.Plus, 
                new List<Result>
                {
                    new() { Name = "Name1", DoB = new DateTime(2018, 10, 10) },
                    new() { Name = "Name2", DoB = new DateTime(2021,  6,  4) },
                    new() { Name = "Name3", DoB = new DateTime(2022, 12, 31) },
                    new() { Name = "Name4", DoB = new DateTime(2022,  5, 13) },
                    new() { Name = "Name5", DoB = new DateTime(2018, 10, 10) },
                    new() { Name = "Name6", DoB = new DateTime(2022, 12, 31) },
                },
                new P
                {
                    P1 = new Result { Name = "Name1", DoB = new DateTime(2018, 10, 10) },
                    P2 = new Result { Name = "Name3", DoB = new DateTime(2022, 12, 31) },
                    D = new DateTime(2022, 12, 31) - new DateTime(2018, 10, 10)
                }
            },
            new object[] 
            { 
                PT.Plus, 
                new List<Result>
                {
                    new() { Name = "Name1", DoB = DateTime.MinValue },
                    new() { Name = "Name2", DoB = DateTime.Now },
                    new() { Name = "Name3", DoB = DateTime.MaxValue },
                },
                new P
                {
                    P1 = new Result { Name = "Name1", DoB = DateTime.MinValue },
                    P2 = new Result { Name = "Name3", DoB = DateTime.MaxValue },
                    D = DateTime.MaxValue - DateTime.MinValue
                }
            }
        };

        public static IEnumerable<object[]> ResultsMinus => new List<object[]>
        {
            new object[] 
            { 
                PT.Minus, 
                new List<Result>
                {
                    new() { Name = "Name1", DoB = new DateTime(2022, 10, 10) },
                    new() { Name = "Name2", DoB = new DateTime(2022,  6,  4) },
                },
                new P
                {
                    P1 = new Result { Name = "Name2", DoB = new DateTime(2022,  6,  4) },
                    P2 = new Result { Name = "Name1", DoB = new DateTime(2022, 10, 10) },
                    D = new DateTime(2022, 10, 10) - new DateTime(2022,  6,  4)
                }
            },
            new object[] 
            { 
                PT.Minus, 
                new List<Result>
                {
                    new() { Name = "Name1", DoB = new DateTime(2022, 12, 31) },
                    new() { Name = "Name2", DoB = new DateTime(2021,  6,  4) },
                    new() { Name = "Name3", DoB = new DateTime(2022, 12, 31) },
                    new() { Name = "Name4", DoB = new DateTime(2022,  5, 13) },
                    new() { Name = "Name5", DoB = new DateTime(2018, 10, 10) },
                    new() { Name = "Name6", DoB = new DateTime(2022, 12, 31) },
                },
                new P
                {
                    P1 = new Result { Name = "Name3", DoB = new DateTime(2022, 12, 31) },
                    P2 = new Result { Name = "Name1", DoB = new DateTime(2022, 12, 31) },
                    D = TimeSpan.Zero
                }
            },
            new object[] 
            { 
                PT.Minus, 
                new List<Result>
                {
                    new() { Name = "Name1", DoB = DateTime.MinValue },
                    new() { Name = "Name2", DoB = DateTime.MaxValue },
                    new() { Name = "Name3", DoB = DateTime.Now },
                    new() { Name = "Name4", DoB = DateTime.MaxValue },
                },
                new P
                {
                    P1 = new Result { Name = "Name4", DoB = DateTime.MaxValue },
                    P2 = new Result { Name = "Name2", DoB = DateTime.MaxValue },
                    D = TimeSpan.Zero
                }
            }
        };

        public static IEnumerable<object[]> ResultsEmpty => new List<object[]>
        {
            new object[] 
            { 
                PT.Plus, 
                new List<Result>(),
                new P()
            },
            new object[] 
            { 
                PT.Minus, 
                new List<Result>
                {
                    new() { Name = "Name1", DoB = new DateTime(2018, 10, 10) }
                },
                new P()
            }
        };
    }
}
