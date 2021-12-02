using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest.Common
{
    public class ExpressionHelper
    {
        public static Expression CreateCheckerExpression(
            Type inputType,
            Expression checkBodyExp,
            Expression ErrMessageFuncExp)
        {

            var nameExp = Expression.Parameter(typeof(string), "name");
            var valueExp = Expression.Parameter(inputType, "value");
            var resultEpx = Expression.Variable(typeof(ValidateResult), "result");
            var returnLabel = Expression.Label(typeof(ValidateResult));

            var checkBody = Expression.Invoke(checkBodyExp, valueExp);
            var errorCallExp = Expression.Invoke(ErrMessageFuncExp, nameExp);
            var validationOkCallExp = Expression.Call(typeof(ValidateResult).GetMethod(nameof(ValidateResult.OK)));
            var validationErrCallExp = Expression.Call(typeof(ValidateResult).GetMethod(nameof(ValidateResult.Error)), errorCallExp);
            var ifthenExp = Expression.IfThenElse(checkBody, Expression.Assign(resultEpx, validationErrCallExp), Expression.Assign(resultEpx, validationOkCallExp));
            var bodyExp = Expression.Block(new[] { resultEpx }, 
                ifthenExp,
                Expression.Return(returnLabel,resultEpx),Expression.Label(returnLabel,resultEpx));
            var funcType = Expression.GetFuncType(typeof(string), inputType, typeof(ValidateResult));
            return Expression.Lambda(funcType, bodyExp, nameExp, valueExp);
        }
    }
}
