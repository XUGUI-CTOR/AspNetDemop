using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ex = System.Linq.Expressions;
using System.Linq;
using FluentAssertions;

namespace Newbe.Expression.Test
{
    public class Tests
    {
        private static Func<CreateClaptrapInput, ValidateResult> _func;
        [SetUp]
        public void Setup()
        {
            _func = CreateCore().Compile();
            ex.Expression<Func<CreateClaptrapInput,ValidateResult>> CreateCore()
            {
                var inputExp = ex.Expression.Parameter(typeof(CreateClaptrapInput), "input");
                var returnLabExp = ex.Expression.Label(typeof(ValidateResult));
                var resultExp = ex.Expression.Variable(typeof(ValidateResult), "result");
                var bodyExps = new List<ex.Expression>() { CreateDefaultValue() };
                foreach (var propertyInfo in typeof(CreateClaptrapInput).GetProperties().Where(p => p.PropertyType == typeof(string)))
                {
                    if(propertyInfo.GetCustomAttribute<RequiredAttribute>() is { })
                        bodyExps.Add(CreateValidateStringRequiedExpression(propertyInfo));
                    if (propertyInfo.GetCustomAttribute<MinLengthAttribute>() is MinLengthAttribute attr)
                        bodyExps.Add(CreateValidateStringMinLengthExpression(propertyInfo, attr.Length));
                }
                bodyExps.Add(ex.Expression.Return(returnLabExp, resultExp));
                bodyExps.Add(ex.Expression.Label(returnLabExp, resultExp));
                var body = ex.Expression.Block(new[] { resultExp }, bodyExps);
                return ex.Expression.Lambda<Func<CreateClaptrapInput, ValidateResult>>(body, inputExp);
                ex.Expression CreateDefaultValue()
                {
                    var method = typeof(ValidateResult).GetMethod(nameof(ValidateResult.OK));
                    var callExp = ex.Expression.Call(method);
                    return ex.Expression.Assign(resultExp,callExp);
                }
                ex.Expression CreateValidateStringRequiedExpression(PropertyInfo propertyInfo)
                {
                    var method = typeof(Tests).GetMethod(nameof(Tests.ValidateStringRequied));
                    var inputPropertyExp = ex.Expression.Property(inputExp, propertyInfo);
                    var propertyNameExp = ex.Expression.Constant(propertyInfo.Name);
                    var callExp = ex.Expression.Call(method, propertyNameExp, inputPropertyExp);
                    var assign = ex.Expression.Assign(resultExp, callExp);
                    var isOKExp = ex.Expression.Property(resultExp, nameof(ValidateResult.IsOK));
                    var ifThenExp = ex.Expression.IfThen(ex.Expression.IsFalse(isOKExp), ex.Expression.Return(returnLabExp, resultExp));
                    return ex.Expression.Block(new[] { resultExp }, assign, ifThenExp);
                }
                ex.Expression CreateValidateStringMinLengthExpression(PropertyInfo propertyInfo,int minLength)
                {
                    var method = typeof(Tests).GetMethod(nameof(Tests.ValidateStringMinLngth));
                    var inputPropertyExp = ex.Expression.Property(inputExp, propertyInfo);
                    var propertyNameExp = ex.Expression.Constant(propertyInfo.Name);
                    var minLengthExp = ex.Expression.Constant(minLength);
                    var callExp = ex.Expression.Call(method, propertyNameExp, inputPropertyExp, minLengthExp);
                    var assign = ex.Expression.Assign(resultExp, callExp);
                    var isOKExp = ex.Expression.Property(resultExp, nameof(ValidateResult.IsOK));
                    var ifThenExp = ex.Expression.IfThen(ex.Expression.IsFalse(isOKExp), ex.Expression.Return(returnLabExp, resultExp));
                    return ex.Expression.Block(new[] { resultExp }, assign, ifThenExp);
                }
            }
        }

        [Test]
        public void Test1()
        {
            var input = new CreateClaptrapInput { Name = "123" };
            var r = _func(input);
            r.IsOK.Should().BeTrue();
        }

        public static ValidateResult ValidateStringRequied(string name, string value) => !string.IsNullOrWhiteSpace(value) ? ValidateResult.OK() : ValidateResult.Error($"{name} is requied");
        public static ValidateResult ValidateStringMinLngth(string name,string value,int minlength)=>value.Length < minlength?ValidateResult.Error($"{name} length must be greater than {minlength}"):ValidateResult.OK();
    }

    public class CreateClaptrapInput
    {
        [MinLength(3),Required]
        public string Name { get; set; }
    }

    public struct ValidateResult
    {
        public bool IsOK { get; set; }
        public string ErrMessage { get; set; }
        public static ValidateResult OK()
        {
            return new ValidateResult() { IsOK = true };
        }
        public static ValidateResult Error(string errMessage)
        {
            return new ValidateResult() { IsOK = false, ErrMessage = errMessage };
        }
    }
}