using ExpressionTest.Common;
using ExpressionTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTest
{
    public class IntMinValuePropertyValidatorFactory : PropertyValidatorFactoryBase<int>
    {
        private static Expression<Func<string, int, ValidateResult>> Func(int minValue) => (name, value) => value.ToString().Length < minValue
            ? ValidateResult.Error($"Value if {name} should be great then {minValue}")
            : ValidateResult.OK();
        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            if (input.PropertyInfo.GetCustomAttribute<MinLengthAttribute>() is { } p)
            {
                Expression<Func<int, bool>> checkbodyExp = x => x.ToString().Length < p.Length;
                Expression<Func<string, string>> ErrMessageExp = (s) => $"Value if {s} should be great then {p.Length}";
                yield return CreateValidateExpression(input, ExpressionHelper.CreateCheckerExpression(typeof(int), checkbodyExp, ErrMessageExp));
            }
 
        }
    }
}
