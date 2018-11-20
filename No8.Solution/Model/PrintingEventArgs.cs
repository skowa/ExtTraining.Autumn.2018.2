using System;

namespace No8.Solution
{
    public sealed class PrintingEventArgs : EventArgs
    {
        public PrintingEventArgs(DateTime time, string message)
        {
            Time = time;
            Message = message;
        }

        public DateTime Time { get; }

        public string Message { get; }
    }
}