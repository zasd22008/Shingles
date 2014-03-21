using System;

namespace Shingles.Errors
{
    public class ShingleException : Exception
    {
        public ShingleException(string message):base(message)
        {
            
        }        
    }

    public class InSufficientTextLengthException : ShingleException
    {
        public InSufficientTextLengthException(string message):base(message)
        {
        }
    }
}