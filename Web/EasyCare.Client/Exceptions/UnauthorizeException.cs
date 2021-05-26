namespace EasyCare.Client.Exceptions
{
    public class UnauthorizeException : System.Exception
    {
        public UnauthorizeException()
            : base("Unauthorize request.") { }
    }
}