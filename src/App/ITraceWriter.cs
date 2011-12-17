using System;

namespace App
{
    public interface ITraceWriter
    {
        void Write(string message, params object[] args);
    }

    public class ConfigurableTraceWriter : ITraceWriter
    {
        private readonly Action<string, object[]> write;

        public ConfigurableTraceWriter(Action<string, object[]> write)
        {
            this.write = write;
        }

        public void Write(string message, params object[] args)
        {
            write(message, args);
        }
    }
}