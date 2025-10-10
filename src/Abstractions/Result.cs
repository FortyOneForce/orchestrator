namespace FortyOne.OrchestratR
{
    public class Result
    {
        public static implicit operator Result(Error error) => Failure(error);

        public bool IsSuccess => Error.IsEmptyError;
        public bool IsFailure => !Error.IsEmptyError;
        public Error Error { get; }

        protected Result()
        {
            Error = Error.Empty;
        }

        protected Result(Error error)
        {
            ArgumentNullException.ThrowIfNull(error);

            if (error.IsEmptyError)
            {
                throw new ArgumentException("Use the Success factory method to create a success result.");
            }

            Error = error;
        }

        public static Result Success() => new Result();
        public static Result Failure(Error error) => new Result(error);

        public static Result<T> Success<T>(T value) where T : class => new Result<T>(value);
        public static Result<T> Failure<T>(Error error) where T : class => new Result<T>(error);

        public static ValueTask<Result<T>> SuccessAsync<T>(T value) where T : class => new(Result.Success(value));
    }

    public class Result<T> : Result where T : class
    {
        public static implicit operator Result<T>(T value) => new(value);
        public static implicit operator Result<T>(Error error) => Failure<T>(error);

        private readonly T? _value;
        public T? Value => IsFailure ? null : (_value ?? throw new InvalidOperationException("Value is null despite the operation being marked as successful."));

        internal Result(T value) : base()
        {
            ArgumentNullException.ThrowIfNull(value);

            _value = value;
        }

        internal Result(Error error) : base(error)
        {
        }
    }
}
