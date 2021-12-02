using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest
{
    public struct ValidateResult
    {
        public ValidateResult(string errorMessage, bool isOK)
        {
            ErrorMessage = errorMessage;
            IsOK = isOK;
        }
        public string ErrorMessage { get; set; }
        public bool IsOK { get; set; }
        public static ValidateResult OK() => new ValidateResult(string.Empty, true);
        public static ValidateResult Error(string message) => new ValidateResult(message, false);

    }
}
