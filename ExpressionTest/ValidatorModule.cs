using Autofac;
using ExpressionTest.Impl;
using ExpressionTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module = Autofac.Module;

namespace ExpressionTest
{
    public class ValidatorModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>().SingleInstance();
            builder.RegisterType<StringRequiredPropertyValidatorFactory>().As<IPropertyValidatorFactory>().SingleInstance();
            builder.RegisterType<StringLengthPropertyValidatorFactory>().As<IPropertyValidatorFactory>().SingleInstance();
            builder.RegisterType<StringMaxLengthPropertyValidatorFactory>().As<IPropertyValidatorFactory>().SingleInstance();
            builder.RegisterType<IntMinValuePropertyValidatorFactory>().As<IPropertyValidatorFactory>().SingleInstance();
            builder.RegisterType<IntRangePropertyValidatorFactory>().As<IPropertyValidatorFactory>().SingleInstance();
            //builder.RegisterType<IEnumerableGenericHasAnyPropertyValicatorFactory<int>>().As<IPropertyValidatorFactory>().SingleInstance();
            builder.RegisterType<EnumerablePropertyValidatorFactory>().As<IPropertyValidatorFactory>().SingleInstance();
        }
    }
}
