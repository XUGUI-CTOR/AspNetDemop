using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Reflection;

namespace ExpressionNuit;
public class Tests
{
    private List<Person> persons;

    
    [SetUp]
    public void Setup()
    {
        persons = new List<Person>
        {
            new Person{
             Name = "Ðí¹é",
             Level = int.MaxValue
            },
            new Person
            {
                Name = "ÕÅÈý",
                Level = 666
            }
        };
    }

    [Test]
    public void Test1()
    {
        var filter = CreateFilter(666);
        persons.FirstOrDefault(filter.Compile()).Level.Should().Be(666);
    }

    private Expression<Func<Person,bool>> CreateFilter(int level)
    {
        
        var pExp = Expression.Parameter(typeof(Person), "x");
        var prpExp = Expression.Property(pExp, nameof(Person.Level));
        var constantExp = Expression.Constant(level, typeof(int));
        return Expression.Lambda<Func<Person, bool>>(Expression.Equal(prpExp, constantExp),pExp);
    }
}
public class Person
{
    public string Name { get; set; }
    public int Level { get; set; }
}
