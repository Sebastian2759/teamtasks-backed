namespace Domain.Base
{
    public class ExceptionBase(Guid id, string message, string details = null)
    {
        public Guid Id { get; } = id;
        public string Message { get; } = message;
        public string Details { get; } = details;
    }
}