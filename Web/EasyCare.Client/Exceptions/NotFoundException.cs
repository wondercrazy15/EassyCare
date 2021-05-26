using System;

namespace EasyCare.Client.Exceptions
{
    [Serializable]
    public class NotFoundException : System.Exception
    {
        public NotFoundException()
            : base("Invalid request address.")
        {
        }
    }
}