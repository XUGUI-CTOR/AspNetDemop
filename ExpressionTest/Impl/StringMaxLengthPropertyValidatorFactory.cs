using ExpressionTest.Common;
using ExpressionTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTest
{
    public class StringMaxLengthPropertyValidatorFactory : PropertyValidatorFactoryBase<string>
    {
        private static Expression<Func<string, string, ValidateResult>> Func(int MaxLength) => (name, value) => value.Length > MaxLength
            ? ValidateResult.Error($"Length if {name} should be less than {MaxLength}")
            : ValidateResult.OK();

        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            if (input.PropertyInfo.GetCustomAttribute<MaxLengthAttribute>() is { } p)
            {
                Expression<Func<string, bool>> checkbodyExp = x => string.IsNullOrEmpty(x) || x.Length > p.Length;
                Expression<Func<string, string>> ErrMessageExp = (s) => $"Length of {s} should be less than {p.Length}";
                yield return CreateValidateExpression(input, ExpressionHelper.CreateCheckerExpression(typeof(string), checkbodyExp, ErrMessageExp));
            }
        }
    }
}
