using ExpressionTest.Common;
using ExpressionTest.Interfaces;
using ExpressionTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest.Impl
{
    public class EnumerablePropertyValidatorFactory : IPropertyValidatorFactory
    {
        public virtual IEnumerable<Expression> CreateExpression(CreatePropertyValidatorInput input)
        {
            if (input.PropertyInfo.PropertyType == typeof(string))
                return Enumerable.Empty<Expression>();
            if (input.PropertyInfo.PropertyType.GetInterfaces().Any(x=>x.IsGenericType && x.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>))))
                return GetExpressions(input);
            return Enumerable.Empty<Expression>();
            
        }
        private IEnumerable<Expression> GetExpressions(CreatePropertyValidatorInput input)
        {
            {
                var argType = input.PropertyInfo.PropertyType.GetGenericArguments().First();
                var inputType = typeof(IEnumerable<>).MakeGenericType(argType);
                Expression checkbodyExp = CreateCheckBodyExpression(argType);
                Expression<Func<string, string>> ErrMessageExp = x => $"{x} must has any";
                var r = (Expression)(this.GetType().GetMethod(nameof(CreateValidateExpression)).MakeGenericMethod(input.PropertyInfo.PropertyType.GetGenericArguments()).Invoke(this, new object[] { input,ExpressionHelper.CreateCheckerExpression(typeof(IEnumerable<>).MakeGenericType(argType),checkbodyExp,ErrMessageExp) }));
                yield return r;
            }
            
        }

        private Expression CreateCheckBodyExpression(Type argumentType)
        {
            var inputType = typeof(IEnumerable<>).MakeGenericType(argumentType);
            var inputExp = Expression.Parameter(inputType, "input");
            var callExp = Expression.Call(typeof(Enumerable).GetMethod(nameof(Enumerable.Any),1,new[] { inputType}).MakeGenericMethod(argumentType), inputExp);
            var funcExp = Expression.GetFuncType(inputType, typeof(bool));
            return Expression.Lambda(funcExp, callExp, inputExp);
        }

        private Expression<Func<string, IEnumerable<T>, ValidateResult>> Func<T>() => (name, value) => value.Any() ? ValidateResult.OK() : ValidateResult.Error($"{name} contains at least one data");
        public Expression CreateValidateExpression<T>(CreatePropertyValidatorInput input,Expression validateFuncExpression)
        {
            //Expression<Func<string, IEnumerable<T>, ValidateResult>> validateFuncExpression = Func<T>();
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
