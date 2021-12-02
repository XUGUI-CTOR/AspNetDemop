using ExpressionTest.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace ExpressionTest.Impl
{
    public class IEnumerableGenericHasAnyPropertyValicatorFactory<T> : PropertyValidatorFactoryBase<IEnumerable<T>>
    {
        private Expression<Func<string, IEnumerable<T>, ValidateResult>> Func() => (name, value) => value.Count() > 0 ? ValidateResult.OK() : ValidateResult.Error($"{name} must has anyone");
        protected override IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input)
        {
            if (input.PropertyInfo.PropertyType.GetInterfaces().Any(x => x == typeof(IEnumerable<T>)))
               yield return CreateValidateExpression(input, Func());
        }
    }
}
