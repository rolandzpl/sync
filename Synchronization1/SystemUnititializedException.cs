using System;
using System.Runtime.Serialization;

namespace Synchronization1
{
    [Serializable]
    internal class SystemUnititializedException : Exception
    {
        public SystemUnititializedException()
        {
        }

        public SystemUnititializedException(string message) : base(message)
        {
        }

        public SystemUnititializedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SystemUnititializedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}