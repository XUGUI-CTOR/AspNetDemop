using ExpressionTest.Interfaces;
using ExpressionTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace ExpressionTest
{
    public abstract class PropertyValidatorFactoryBase<T> : IPropertyValidatorFactory
    {

        

        public virtual IEnumerable<Expression> CreateExpression(CreatePropertyValidatorInput input)
        {
            if (input.PropertyInfo.PropertyType != typeof(T))
                return Enumerable.Empty<Expression>();
            return CreateExpressionCore(input);
        }
        protected abstract IEnumerable<Expression> CreateExpressionCore(CreatePropertyValidatorInput input);
        protected Expression CreateValidateExpression(CreatePropertyValidatorInput input, Expression validateFuncExpression)
        {
            var propertyInfo = input.PropertyInfo;
            var isOkProperty = typeof(ValidateResult).GetProperty(nameof(ValidateResult.IsOK));
            var convertExp = Expression.Convert(input.InputExpression, input.InputType);
            var propExp = Expression.Property(convertExp, propertyInfo);
            var nameContestExp = Expression.Constant(propertyInfo.Name, typeof(string));
            var requiredMethodExp = Expression.Invoke(validateFuncExpression, nameContestExp, propExp);
            var assignExp = Expression.Assign(input.ResultExpression, requiredMethodExp);
            var resultIsOkExp = Expression.Property(input.ResultExpression, isOkProperty);
            var ifthenExp = Expression.IfThen(Expression.IsFalse(resultIsOkExp), Expression.Return(input.ReturnLabel, input.ResultExpression));
            return Expression.Block(assignExp, ifthenExp);
        }
    }
}
