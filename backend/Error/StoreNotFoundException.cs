
namespace backend.Error
{
    public class StoreNotFoundException : Exception
    {
        public StoreNotFoundException()
        {
        }

        public StoreNotFoundException(string? message) : base(message)
        {
        }

        public StoreNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}