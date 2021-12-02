using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest.Model
{
    public class CreatePropertyValidatorInput
    {

        public Type InputType { get; set; } = null;
        public Expression InputExpression { get; set; } = null;
        public PropertyInfo PropertyInfo { get; set; } = null;
        public ParameterExpression ResultExpression { get; set; } = null;
        public LabelTarget ReturnLabel { get; set; } = null;
    }
}
