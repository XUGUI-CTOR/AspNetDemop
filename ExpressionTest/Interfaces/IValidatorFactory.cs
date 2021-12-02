using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest.Interfaces
{
    public interface IValidatorFactory
    {
        Func<object, ValidateResult> GetValidator(Type type);
    }
}
