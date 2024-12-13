using System;

namespace Application.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(){}
        
        public AuthenticationException(string message) : base(message){}
    }
}