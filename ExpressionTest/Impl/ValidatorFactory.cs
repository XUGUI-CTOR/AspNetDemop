using ExpressionTest.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IEnumerable<IPropertyValidatorFactory> propertyValidatorFactories;
        public ValidatorFactory(IEnumerable<IPropertyValidatorFactory> propertyValidatorFactories)
        {
            this.propertyValidatorFactories = propertyValidatorFactories;
        }
        private static readonly ConcurrentDictionary<Type, Func<object, ValidateResult>> validateFuncs = new ConcurrentDictionary<Type, Func<object, ValidateResult>>();

        public Func<object, ValidateResult> GetValidator(Type type)
        {
            return validateFuncs.GetOrAdd(type, CreateValidator);
        }

        private Func<object, ValidateResult> CreateValidator(Type type)
        {
            return CreateCore().Compile();
            Expression<Func<object, ValidateResult>> CreateCore()
            {
                var inputExp = Expression.Parameter(typeof(object), "input");
                var resultExp = Expression.Variable(typeof(ValidateResult), "res");
                var returnLabel = Expression.Label(typeof(ValidateResult));
                var innerExps = new List<Expression>() { CreateDefaultResult() };
                var validateExpressions = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                    .SelectMany(p => propertyValidatorFactories
                        .SelectMany(f => f.CreateExpression(new Model.CreatePropertyValidatorInput
                        {
                            InputExpression = inputExp,
                            InputType = type,
                            ResultExpression = resultExp,
                            ReturnLabel = returnLabel,
                            PropertyInfo = p
                        })));
                innerExps.AddRange(validateExpressions);
                innerExps.Add(Expression.Label(returnLabel, resultExp));
                var body = Expression.Block(new[] { resultExp }, innerExps);
                return Expression.Lambda<Func<object, ValidateResult>>(body, inputExp);
                Expression CreateDefaultResult()
                {
                    var okMetnhod = typeof(ValidateResult).GetMethod(nameof(ValidateResult.OK));
                    var callExp = Expression.Call(okMetnhod);
                    var assignExp = Expression.Assign(resultExp, callExp);
                    return assignExp;
                }
            }
        }
    }
}
