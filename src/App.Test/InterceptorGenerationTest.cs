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
        private WindsorContainer container;
        private List<string> traceMessages;

        [SetUp]
        public void Setup()
        {
            container = new WindsorContainer();

            traceMessages = new List<string>();

            var writer = new ConfigurableTraceWriter(
                (msg, args) => traceMessages.Add(string.Format(msg, args)));

            container.Register(Component.For<ITraceWriter>().Instance(writer));
            container.Register(Component.For<TraceInterceptor>());
        }

        [Test]
        public void Can_Create_An_Interceptor_From_Configuration()
        {
            container.Register(Component.For<ICalculator>()
                                   .ImplementedBy<Calculator>()
                                   .Interceptors(new InterceptorReference(typeof (TraceInterceptor)))
                                   .Anywhere);


            var calculator = container.Resolve<ICalculator>();
            calculator.Sum(2, 3);

            Assert.That(traceMessages[0], Is.StringContaining("Enter: Sum"));
            Assert.That(traceMessages[1], Is.StringContaining("Exit: Sum"));
            
        }

        [Test]
        public void Can_Select_Intercepted_Methods_With_Generation_Hooks()
        {
            container.Register(Component.For<ICalculator>()
                                   .ImplementedBy<Calculator>()
                                   .Interceptors(new InterceptorReference(typeof(TraceInterceptor)))
                                   .Anywhere
                                   .Proxy.Hook(new MethodSelectorProxyGenerationHook("Sum")));


            var calculator = container.Resolve<ICalculator>();
            calculator.Sum(2, 3);

            Assert.That(traceMessages, Is.Not.Empty);

            // Since Mult is not going to be intercepted, no new message should be generated
            traceMessages.Clear();

            calculator.Mult(2, 3);

            Assert.That(traceMessages, Is.Empty);
        }
    }
}
