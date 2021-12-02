using ExpressionTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ExpressionTest.Common;

namespace ExpressionTest.Impl
{
    public class IntRangePropertyValidatorFactory : PropertyValidatorFactoryBase<int>
    {
        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            if (input.PropertyInfo.GetCustomAttribute<RangeAttribute>() is RangeAttribute p)
            {
                Expression<Func<int, bool>> checkbodyExp = x => x < (int)p.Minimum || x > (int)p.Maximum;
                Expression<Func<string, string>> ErrMessageExp = (s) => $"{s} must range in {p.Minimum} and {p.Maximum}";
                yield return CreateValidateExpression(input, ExpressionHelper.CreateCheckerExpression(typeof(int), checkbodyExp, ErrMessageExp));
            }
        }

        private Expression<Func<string, int, ValidateResult>> Func(int minnum, int maxnum) => (name, value) => value >= minnum && value <= maxnum ? ValidateResult.OK() : ValidateResult.Error($"{name} must range in {minnum} and {maxnum}");
    }
}
