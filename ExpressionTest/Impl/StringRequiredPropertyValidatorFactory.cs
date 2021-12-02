using ExpressionTest.Common;
using ExpressionTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest
{
    public class StringRequiredPropertyValidatorFactory : PropertyValidatorFactoryBase<string>
    {
        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            if (input.PropertyInfo.GetCustomAttribute<RequiredAttribute>() is not null)
            {
                Expression<Func<string, bool>> checkbodyExp = x => string.IsNullOrEmpty(x);
                Expression<Func<string, string>> ErrMessageExp = x => $"missing {x}";
                yield return CreateValidateExpression(input, ExpressionHelper.CreateCheckerExpression(typeof(string), checkbodyExp,ErrMessageExp));
            }
        }

        private static Expression<Func<string, string, ValidateResult>> CreateValidateStringRequiedExpression => (name, value) => string.IsNullOrEmpty(value)
                            ? ValidateResult.Error($"missing {name}")
                            : ValidateResult.OK();
    }
}
