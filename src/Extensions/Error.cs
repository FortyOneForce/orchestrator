namespace FortyOne.OrchestratR.Extensions;

/// <summary>
/// Represents an error that can occur during the processing of a request.
/// </summary>
public sealed class Error
{
    internal readonly static Error Empty = new Error() { IsEmptyError = true };

    internal bool IsEmptyError = false;

    /// <summary>
    /// Message describing the error.
    /// </summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>
    /// Code representing the error.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Exception associated with the error, if any.
    /// </summary>
    public Exception? Exception { get; private set; }

    /// <summary>
    /// Nested errors providing additional context.
    /// </summary>
    public Error[] Errors { get; private set; } = Array.Empty<Error>();

    /// <summary>
    /// Dictionary for storing additional metadata about the error.
    /// </summary>
    public IDictionary<string, string?> Extensions { get; } = new Dictionary<string, string?>();

    /// <summary>
    /// Private constructor to enforce the use of static creation methods.
    /// </summary>
    private Error()
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Error"/> class with the specified message.
    /// </summary>
    public Error(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Message = message;
        Code = string.Empty;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Error"/> class with the specified message and code.
    /// </summary>
    private Error(string message, string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        Message = message;
        Code = code;
    }

    /// <summary>
    /// Modifies the error code.
    /// </summary>
    public Error WithCode(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        Code = code;

        return this;
    }

    /// <summary>
    /// Modifies the error message.
    /// </summary>
    public Error WithMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        Message = message;

        return this;
    }

    /// <summary>
    /// Sets the associated exception.
    /// </summary>
    public Error WithException(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        Exception = exception;

        return this;
    }

    /// <summary>
    /// Sets the nested errors.
    /// </summary>
    public Error WithErrors(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        Errors = errors.ToArray();

        return this;
    }

    /// <summary>
    /// Sets the nested errors.
    /// </summary>
    public Error WithErrors(params Error[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        Errors = errors;

        return this;
    }

    /// <summary>
    /// Adds or updates an entry in the extensions dictionary.
    /// </summary>
    public Error WithExtension(string key, string? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        Extensions[key] = value;

        return this;
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified message.
    /// </summary>
    public static Error Create(string message) => new Error(message);

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified message and code.
    /// </summary>
    public static Error Create(string message, string code) => new Error(message, code);
}
