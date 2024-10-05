namespace DataAccessLayer.Exceptions;

public class RecordNotFoundException(string? message) : Exception(message)
{
}
