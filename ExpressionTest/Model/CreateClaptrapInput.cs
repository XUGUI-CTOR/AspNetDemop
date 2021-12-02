using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest
{
    public class CreateClaptrapInput
    {
        [MinLength(3), Required, MaxLength(10)]
        public string Name { get; set; }
        [MinLength(3), Required]
        public string NickName { get; set; }
        [System.ComponentModel.DataAnnotations.Range(0, 200)]
        public int Age { get; set; }
        public List<int> Achievements { get; set; }
    }
}
