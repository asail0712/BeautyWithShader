using System;
using UnityEngine;

namespace Granden
{
    public class LogCommand : ICommand
    {
        private readonly Func<string> logMessageFactory;

        public LogCommand(string logMessage)
        {
            logMessageFactory = () => logMessage;
        }

        public LogCommand(Func<string> logMessageFactory)
        {
            this.logMessageFactory = logMessageFactory;
        }

        public void Execute()
        {
            Debug.Log(logMessageFactory.Invoke());
        }
    }
}