using System;

namespace Ubuntu.Landscape.Exceptions
{
    /// <summary>
    /// InputException
    /// </summary>
    class InputException : Exception
    {
        public InputException(string message) : base(message)
        {
        }
    }
}
