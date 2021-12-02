using ExpressionTest.Common;
using ExpressionTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTest
{
    public class StringLengthPropertyValidatorFactory : PropertyValidatorFactoryBase<string>
    {
        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            if (input.PropertyInfo.GetCustomAttribute<MinLengthAttribute>() is { } v)
            {
                Expression<Func<string, bool>> checkbodyExp = x => string.IsNullOrEmpty(x) || x.Length < v.Length;
                Expression<Func<string, string>> ErrMessageExp = (s) => $"Length of {s} should be great than {v.Length}";
                yield return CreateValidateExpression(input, ExpressionHelper.CreateCheckerExpression(typeof(string), checkbodyExp, ErrMessageExp));
            }
        }
        //private static Expression<Func<string, string, ValidateResult>> CreateValidateStringMinLengthExp(int minLength) => (name, value) =>
        //          string.IsNullOrEmpty(value) || value.Length < minLength
        //              ? ValidateResult.Error($"Length of {name} should be great than {minLength}")
        //              : ValidateResult.OK();
    }
}
