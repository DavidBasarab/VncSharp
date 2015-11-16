using System;
using System.Runtime.Serialization;

namespace VncSharp
{
    public class VncProtocolException : ApplicationException
    {
        public VncProtocolException() { }

        public VncProtocolException(string message)
                : base(message) { }

        public VncProtocolException(string message, Exception inner)
                : base(message, inner) { }

        public VncProtocolException(SerializationInfo info, StreamingContext cxt)
                : base(info, cxt) { }
    }
}