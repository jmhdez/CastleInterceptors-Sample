using System;
using System.Collections.Generic;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;

namespace App.Test
{
    [TestFixture]
    public class InterceptorGenerationTest
    {
        [Test]
        public void Can_Create_An_Interceptor_From_Configuration()
        {
            var container = new WindsorContainer();

            var messages = new List<string>();

            var writer = new ConfigurableTraceWriter(
                (msg, args) => messages.Add(string.Format(msg, args)));

            container.Register(Component.For<ITraceWriter>().Instance(writer));

            container.Register(Component.For<TraceInterceptor>());

            container.Register(Component.For<ICalculator>()
                                   .ImplementedBy<Calculator>()
                                   .Interceptors(new InterceptorReference(typeof (TraceInterceptor)))
                                   .Anywhere);


            var calculator = container.Resolve<ICalculator>();
            calculator.Sum(2, 3);

            Assert.That(messages[0], Is.StringContaining("Enter: Sum"));
            Assert.That(messages[1], Is.StringContaining("Exit: Sum"));
            
        }

        [Test]
        public void Can_Select_Intercepted_Methods_With_Generation_Hooks()
        {
            var container = new WindsorContainer();

            var messages = new List<string>();

            var writer = new ConfigurableTraceWriter(
                (msg, args) => messages.Add(string.Format(msg, args)));

            container.Register(Component.For<ITraceWriter>().Instance(writer));

            container.Register(Component.For<TraceInterceptor>());

            container.Register(Component.For<ICalculator>()
                                   .ImplementedBy<Calculator>()
                                   .Interceptors(new InterceptorReference(typeof(TraceInterceptor)))
                                   .Anywhere
                                   .Proxy.Hook(new MethodSelectorProxyGenerationHook("Sum")));


            var calculator = container.Resolve<ICalculator>();
            calculator.Sum(2, 3);

            Assert.That(messages, Is.Not.Empty);

            // Since Mult is not going to be intercepted, no new message should be generated
            messages.Clear();

            calculator.Mult(2, 3);

            Assert.That(messages, Is.Empty);
        }
    }
}
