using System.Collections.Concurrent;
using System.Reflection;

namespace FortyOne.OrchestratR.Extensions;

/// <summary>
/// Represents the outcome of an operation, which can be either a success or a failure.
/// </summary>
public class Result
{
    private static ConcurrentDictionary<Type, ConstructorInfo> _constructorCache = new();

    /// <summary>
    /// Overload to create a successful Result from a value of type T.
    /// </summary>
    public static implicit operator Result(Error error) => Failure(error);

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess => Error.IsEmptyError;

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    public bool IsFailure => !Error.IsEmptyError;

    /// <summary>
    /// Gets the error associated with a failed operation.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Constructor for creating a successful Result.
    /// </summary>
    protected Result()
    {
        Error = Error.Empty;
    }

    /// <summary>
    /// Constructor for creating a failed Result with the specified error.
    /// </summary>
    protected Result(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        if (error.IsEmptyError)
        {
            throw new ArgumentException("Use the Success factory method to create a success result.");
        }

        Error = error;
    }

    /// <summary>
    /// Casts this Result to a Result of type T.
    /// </summary>
    public T Cast<T>()
    {
        return (T)(object)this;
    }

    /// <summary>
    /// Creates a successful Result.
    /// </summary>
    public static Result Success() => new Result();

    /// <summary>
    /// Creates a failed Result with the specified error.
    /// </summary>
    public static Result Failure(Error error) => new Result(error);

    /// <summary>
    /// Creates a successful Result with the specified value.
    /// </summary>
    public static Result<T> Success<T>(T value) where T : class => new Result<T>(value);

    /// <summary>
    /// Creates a failed Result of type T with the specified error.
    /// </summary>
    public static Result<T> Failure<T>(Error error) where T : class => new Result<T>(error);

    /// <summary>
    /// Creates a failed Result of the specified reference type with the given error.
    /// </summary>
    public static Result Failure(Type valueType, Error error)
    {
        ArgumentNullException.ThrowIfNull(valueType);
        if (!valueType.IsClass)
        {
            throw new InvalidOperationException($"The type '{valueType.FullName}' must be a reference type (class) to create a Result<T>.");
        }

        var ctor = _constructorCache.GetOrAdd(valueType, (key) =>
        {
            var resultType = typeof(Result<>).MakeGenericType(valueType);
            var ctor = resultType.GetConstructor(new[] { typeof(Error) })!;
            return ctor;
        });

        var instance = (Result)ctor.Invoke(new object[] { error });

        return instance;
    }

    /// <summary>
    /// Creates a successful result asynchronously with the specified value.
    /// </summary>
    public static ValueTask<Result<T>> SuccessAsync<T>(T value) where T : class => new(Success(value));
}

/// <summary>
/// Represents the outcome of an operation that returns a value of type T, which can be either a success or a failure.
/// </summary>
public class Result<T> : Result where T : class
{
    /// <summary>
    /// Overload to create a successful Result from a value of type T.
    /// </summary>
    public static implicit operator Result<T>(T value) => new(value);

    /// <summary>
    /// Overload to create a failed Result from an Error.
    /// </summary>
    public static implicit operator Result<T>(Error error) => Failure<T>(error);

    private readonly T? _value;

    /// <summary>
    /// Gets the value associated with a successful operation, or null if the operation failed.
    /// </summary>
    public T? Value => IsFailure ? null : _value ?? throw new InvalidOperationException("Value is null despite the operation being marked as successful.");

    internal Result(T value) : base()
    {
        ArgumentNullException.ThrowIfNull(value);

        _value = value;
    }

    /// <summary>
    /// Constructor for creating a failed Result with the specified error.
    /// </summary>
    public Result(Error error) : base(error)
    {
    }
}
