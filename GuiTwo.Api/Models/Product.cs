using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GuiTwo.Api.Models
{
    public class Product
    {
        [Description("ID")]
        public int Id { get; set; }
        [Description("产品名称")]
        public string Name { get; set; }
        [Description("产品类型")]
        public string Category { get; set; }
        [Description("产品价格")]
        public decimal Price { get; set; }
    }
}