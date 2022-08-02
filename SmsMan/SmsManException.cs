namespace SmsMan;

public class SmsManException : Exception
{
    public SmsManException(string errorCode, string errorMessage, Exception? innerException = null) : base($"{errorCode} - {errorMessage}", innerException)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public string ErrorCode { get; }
    public string ErrorMessage { get; }
}