using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Castle.DynamicProxy;

namespace App
{
    public class TraceInterceptor : StandardInterceptor
    {
        private readonly ITraceWriter writer;

        public TraceInterceptor(ITraceWriter writer)
        {
            this.writer = writer;
        }

        protected override void PreProceed(IInvocation invocation)
        {
            writer.Write("Enter: {0}", invocation.Method.Name);
        }

        protected override void PostProceed(IInvocation invocation)
        {
            writer.Write("Exit: {0}", invocation.Method.Name);
        }
    }
}
