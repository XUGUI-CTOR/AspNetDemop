using ExpressionTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest.Interfaces
{
    public interface IPropertyValidatorFactory
    {
        IEnumerable<Expression> CreateExpression(CreatePropertyValidatorInput input);
    }
}
