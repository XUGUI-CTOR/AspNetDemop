using Autofac;
using ExpressionTest.Interfaces;
using ExpressionTest.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest
{
    public class X03PropertyValidationTest10
    {
        private IValidatorFactory _factory = null!;
        [SetUp]
        public void SetUp()
        {
            var builder = new  ContainerBuilder();
            builder.RegisterModule<ValidatorModule>();
            var container = builder.Build();
            _factory = container.Resolve<IValidatorFactory>();
        }

        [Test]
        public void Run()
        {
            var input = new CreateClaptrapInput() { Name = "warboss", NickName = "Nick",Age = -2 };
            var validator = _factory.GetValidator(input.GetType());
            var r = validator.Invoke(input);
            r.IsOK.Should().BeTrue();
        }
    }
}
