using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTest
{
    public class Tests
    {
        private const int Count = 1000;
        private static Func<CreateClaptrapInput, ValidateResult> _func;
        [SetUp]
        public void Setup()
        {
            _func = CreateCore().Compile();
            Expression<Func<CreateClaptrapInput, ValidateResult>> CreateCore()
            {
                var inputExp = Expression.Parameter(typeof(CreateClaptrapInput), "input");
                var returnLable = Expression.Label(typeof(ValidateResult));
                var resultExp = Expression.Variable(typeof(ValidateResult), "result");
                var innerExps = new List<Expression> { CreateDefaultValue() };
                foreach (var propertyInfo in typeof(CreateClaptrapInput).GetProperties().Where(e=>e.PropertyType == typeof(string)))
                {
                    if(propertyInfo.GetCustomAttribute<RequiredAttribute>() is not null)
                    {
                        innerExps.Add(CreateValidateRequiedValue(propertyInfo));
                    }
                    if(propertyInfo.GetCustomAttribute<MinLengthAttribute>() is MinLengthAttribute min)
                    {
                        var minConstantExp = Expression.Constant(min.Length, typeof(int));
                        innerExps.Add(CreateValidateMinLengthValue(propertyInfo, minConstantExp));
                    }
                    
                }
                innerExps.Add(Expression.Label(returnLable, resultExp));
                Expression bodyExp = Expression.Block(new[] {resultExp }, innerExps);
                return Expression.Lambda<Func<CreateClaptrapInput,ValidateResult>>(bodyExp, inputExp);
                Expression CreateDefaultValue()
                {
                    var method = typeof(ValidateResult).GetMethod(nameof(ValidateResult.OK));
                    var callExp = Expression.Call(method);
                    var assignExp = Expression.Assign(resultExp, callExp);
                    return assignExp;
                }
                
                Expression CreateValidateRequiedValue(PropertyInfo property)
                {
                    var method = typeof(Tests).GetMethod(nameof(Tests.ValidateNameRequired));
                    var NameExp = Expression.Constant(property.Name, typeof(string));
                    var ValueExp = Expression.Property(inputExp, property);
                    var callExp = Expression.Call(method, NameExp, ValueExp);
                    var assginExp = Expression.Assign(resultExp, callExp);
                    var isOkExp = Expression.Property(resultExp, typeof(ValidateResult).GetProperty(nameof(ValidateResult.IsOK)));
                    var judgeExp = Expression.IfThen(Expression.IsFalse(isOkExp), Expression.Return(returnLable, resultExp));
                    return Expression.Block(assginExp, judgeExp);
                }
                Expression CreateValidateMinLengthValue(PropertyInfo property,ConstantExpression minLengthConstantExp)
                {
                    var method = typeof(Tests).GetMethod(nameof(Tests.ValidateNameMinLength));
                    var NameExp = Expression.Constant(property.Name, property.PropertyType);
                    var ValueExp = Expression.Property(inputExp, property);
                    var callExp = Expression.Call(method, NameExp, ValueExp, minLengthConstantExp);
                    var assginExp = Expression.Assign(resultExp, callExp);
                    var isOkExp = Expression.Property(resultExp, typeof(ValidateResult).GetProperty(nameof(ValidateResult.IsOK)));
                    var ifThenExp = Expression.IfThen(Expression.IsFalse(isOkExp), Expression.Return(returnLable, resultExp));
                    return Expression.Block(assginExp, ifThenExp);
                }
            }
        }

        [Test]
        public void Test1()
        {
            var input = new CreateClaptrapInput() { Name = "wasd", NickName = "warboss" };
            var result = _func(input);
            result.IsOK.Should().BeTrue();
        }
        [Test]
        public void T2()
        {
          
        }

        public static ValidateResult Validate(CreateClaptrapInput input) => _func(input);
        public static ValidateResult ValidateNameRequired(string name, string value) => string.IsNullOrWhiteSpace(value) ? ValidateResult.Error($"missing {name}") : ValidateResult.OK();
        public static ValidateResult ValidateNameMinLength(string name, string value, int minlength) => value.Length < minlength ? ValidateResult.Error($"length of {name} should be great than {minlength}") : ValidateResult.OK();
    }

    

   
}