using Grpc.Core;

namespace WConnect.Auth.Application.Exceptions;

public class MissingConfigurationParameterException: RpcException
{
    public MissingConfigurationParameterException(string paramName) : base(ErrorStatus(paramName))
    {
    }
    private static Status ErrorStatus(string paramName)
    {
        return new Status(StatusCode.NotFound, ErrorMessage(paramName));
    }
    
    private static string ErrorMessage(string paramName)
    {
        return $"The configuration parameter {paramName} is not configured.";
    }
}