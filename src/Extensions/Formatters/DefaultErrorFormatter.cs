using FortyOne.OrchestratR.Extensions;
using System.Text.RegularExpressions;

namespace FortyOne.OrchestratR.Extensions.Formatters;

/// <summary>
/// Default implementation of <see cref="IErrorFormatter"/> that formats error codes based on request type names.
/// </summary>
public class DefaultErrorFormatter : IErrorFormatter
{
    /// <summary>
    /// Transforms the given error by assigning a formatted error code if it is not already set.
    /// </summary>
    public virtual Error TransformError(Error error, Type requestType, Type responseType)
    {
        if (string.IsNullOrWhiteSpace(error.Code))
        {
            var formatterName = Regex.Replace(requestType.Name, "([A-Z])", "_$1").ToUpper().TrimStart('_');
            var code = $"ERR.{formatterName}.{(error.Exception is null ? "0" : ".500")}";

            error.WithCode(code);
        }

        return error;
    }
}
