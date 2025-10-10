namespace FortyOne.OrchestratR
{
    public sealed class Error
    {
        internal readonly static Error Empty = new Error() { IsEmptyError = true };

        internal bool IsEmptyError = false;

        public string Message { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public Type? Source { get; private set; }
        public Type? Target { get; set; }
        public Exception? Exception { get; private set; }
        public Error[] Errors { get; private set; } = Array.Empty<Error>();
        public IDictionary<string, string?> Extensions { get; } = new Dictionary<string, string?>();

        private Error()
        {
        }

        public Error(string message)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message);
            Message = message;
            Code = string.Empty;
        }

        private Error(string message, string code)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(code);

            Message = message;
            Code = code;
        }

        public Error WithSource(Type source)
        {
            ArgumentNullException.ThrowIfNull(source);

            Source = source;

            return this;
        }

        public Error WithTarget(Type target)
        {
            ArgumentNullException.ThrowIfNull(target);

            Target = target;

            return this;
        }

        public Error WithException(Exception exception)
        {
            ArgumentNullException.ThrowIfNull(exception);

            Exception = exception;

            return this;
        }

        public Error WithErrors(IEnumerable<Error> errors)
        {
            ArgumentNullException.ThrowIfNull(errors);

            Errors = errors.ToArray();

            return this;
        }

        public Error WithErrors(params Error[] errors)
        {
            ArgumentNullException.ThrowIfNull(errors);

            Errors = errors;

            return this;
        }

        public Error WithExtension(string key, string? value)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(key);

            Extensions[key] = value;

            return this;
        }

        public static Error Create(string message) => new Error(message);
        public static Error Create(string message, string code) => new Error(message, code);
    }
}
