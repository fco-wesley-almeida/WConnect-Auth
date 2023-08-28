using Grpc.Core;

namespace WConnect.Auth.Application.Exceptions;

public class UserDoesNotExistsException: RpcException
{
    public UserDoesNotExistsException() : base(ErrorStatus)
    {
    }
    private const string ErrorMessage = "The user does not exists.";
    private static Status ErrorStatus => new(StatusCode.NotFound, ErrorMessage);
}