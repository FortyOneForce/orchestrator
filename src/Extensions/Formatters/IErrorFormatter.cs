using FortyOne.OrchestratR.Extensions;

namespace FortyOne.OrchestratR.Extensions.Formatters;

/// <summary>
/// Service to format error codes based on request and response types.
/// </summary>
public interface IErrorFormatter
{
    /// <summary>
    /// Transforms the given error based on the request and response types.
    /// </summary>
    Error TransformError(Error error, Type requestType, Type responseType);
}
