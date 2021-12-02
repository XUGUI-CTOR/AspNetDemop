using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest
{
    public class X03PropertyValidationTest04
    {
        private const int Count = 10_000;

        private static Dictionary<Type, Func<object, ValidateResult>> validateFuncs = new Dictionary<Type, Func<object, ValidateResult>>();

        [SetUp]
        public void Init()
        {
            try
            {
                
                validateFuncs[typeof(CreateClaptrapInput)] = CreateCore(typeof(CreateClaptrapInput)).Compile();
                Expression<Func<object, ValidateResult>> CreateCore(Type type)
                {
                    // exp for input
                    var inputExp = Expression.Parameter(typeof(object), "input");

                    // exp for output
                    var resultExp = Expression.Variable(typeof(ValidateResult), "result");

                    // exp for return statement
                    var returnLabel = Expression.Label(typeof(ValidateResult));

                    var innerExps = new List<Expression> { CreateDefaultResult() };

                    var stringProps = type
                        .GetProperties()
                        .Where(x => x.PropertyType == typeof(string));

                    foreach (var propertyInfo in stringProps)
                    {
                       if(propertyInfo.GetCustomAttribute<RequiredAttribute>() != null)
                            innerExps.Add(CreateValidateStringRequiredExpression(propertyInfo));
                        //innerExps.Add(CreateValidateStringMinLengthExpression(propertyInfo));
                        if (propertyInfo.GetCustomAttribute<MinLengthAttribute>() is { } atri)
                            innerExps.Add(CreateValidateStringMinLengthExpression(propertyInfo, atri.Length));
                    }

                    innerExps.Add(Expression.Label(returnLabel, resultExp));

                    // build whole block
                    var body = Expression.Block(
                        new[] { resultExp },
                        innerExps);

                    // build lambda from body
                    var final = Expression.Lambda<Func<object, ValidateResult>>(
                        body,
                        inputExp);
                    return final;

                    Expression CreateDefaultResult()
                    {
                        var okMethod = typeof(ValidateResult).GetMethod(nameof(ValidateResult.OK));
                        Debug.Assert(okMethod != null, nameof(okMethod) + " != null");
                        var methodCallExpression = Expression.Call(okMethod);
                        var re = Expression.Assign(resultExp, methodCallExpression);
                        /**
                         * final as:
                         * result = ValidateResult.Ok()
                         */
                        return re;
                    }

                    Expression CreateValidateStringRequiredExpression(PropertyInfo propertyInfo)
                    {
                        return CreateValidateExpression(propertyInfo, ValidateStringRequiedExp);
                    }

                    Expression CreateValidateStringMinLengthExpression(PropertyInfo propertyInfo,int minlength)
                    {
                        return CreateValidateExpression(propertyInfo, ValidateStringMinLengthExp(minlength));
                    }
                    Expression CreateValidateExpression(PropertyInfo propertyInfo, Expression<Func<string, string, ValidateResult>> lambdaExp)
                    {
                        var isOkProperty = typeof(ValidateResult).GetProperty(nameof(ValidateResult.IsOK));
                        //Debug.Assert(minLengthMethod != null, nameof(minLengthMethod) + " != null");
                        Debug.Assert(isOkProperty != null, nameof(isOkProperty) + " != null");
                        var convertExp = Expression.Convert(inputExp, type);
                        var namePropExp = Expression.Property(convertExp, propertyInfo);
                        var nameNameExp = Expression.Constant(propertyInfo.Name);

                        var requiredMethodExp = Expression.Invoke(lambdaExp,
                            nameNameExp,
                            namePropExp);
                        var assignExp = Expression.Assign(resultExp, requiredMethodExp);
                        var resultIsOkPropertyExp = Expression.Property(resultExp, isOkProperty);
                        var conditionExp = Expression.IsFalse(resultIsOkPropertyExp);
                        var ifThenExp =
                            Expression.IfThen(conditionExp,
                                Expression.Return(returnLabel, resultExp));
                        var re = Expression.Block(
                            new[] { resultExp },
                            assignExp,
                            ifThenExp);
                        return re;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Test]
        public void Run()
        {
            var input = new CreateClaptrapInput() { Name = "wasd", NickName = "warboss" };
            var validate = validateFuncs[typeof(CreateClaptrapInput)];
            var result = validate(input);
            result.IsOK.Should().Be(true);
        }

        [Test]
        public void Run2()
        {
            string str = null;
            (str is {Length: 0 }).Should().Be(true);
        }

        //public static ValidateResult Validate(CreateClaptrapInput input)
        //{
        //    return _func.Invoke(input, 3);
        //}

        public static ValidateResult ValidateStringRequired(string name, string value)
        {
            return string.IsNullOrEmpty(value)
                ? ValidateResult.Error($"missing {name}")
                : ValidateResult.OK();
        }
        private static Expression<Func<string, string, ValidateResult>> ValidateStringRequiedExp =>
            (name, value) => string.IsNullOrWhiteSpace(value) ? ValidateResult.Error($"missing {name}") : ValidateResult.OK();

        public static ValidateResult ValidateStringMinLength(string name, string value, int minLength)
        {
            return value.Length < minLength
                ? ValidateResult.Error($"Length of {name} should be great than {minLength}")
                : ValidateResult.OK();
        }
        private static Expression<Func<string, string, ValidateResult>> ValidateStringMinLengthExp(int minLength) => (name, value) => value.Length < minLength
                      ? ValidateResult.Error($"Length of {name} should be great than {minLength}")
                      : ValidateResult.OK();


    }
}
