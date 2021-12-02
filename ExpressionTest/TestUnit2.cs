using Autofac;
using ExpressionTest.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace ExpressionTest
{
    public class TestUnit2
    {
        private IValidatorFactory validator;
        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ValidatorModule());
            var container = builder.Build();
            validator = container.Resolve<IValidatorFactory>();
        }

        [Test]
        public void Test1()
        {
            var validatorFunc = validator.GetValidator(typeof(CreateClaptrapInput));
            var input = new CreateClaptrapInput()
            {
                Name = "warboss",
                Age = 1,
                NickName = "warboss2020",
                Achievements = new List<int>() { 1}
            };
            var r = validatorFunc(input);
            r.ErrorMessage.Should().BeNullOrEmpty();
            r.IsOK.Should().BeTrue();
        }

        [Test]
        public void Test2()
        {
            var list = new List<int>();
            var temp = list.GetType().GetInterfaces().Where(i=>i.IsGenericType).Select(i => i.GetGenericTypeDefinition()).ToList();
            list.GetType().GetGenericTypeDefinition().GetInterfaces().Any(i => i == typeof(IEnumerable<>)).Should().BeTrue();
        }

        

    }
}
