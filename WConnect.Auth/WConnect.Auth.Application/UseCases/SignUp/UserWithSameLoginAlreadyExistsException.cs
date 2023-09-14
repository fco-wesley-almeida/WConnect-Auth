using Grpc.Core;

namespace WConnect.Auth.Application.Exceptions;

public class UserWithSameLoginAlreadyExistsException: RpcException
{
    public UserWithSameLoginAlreadyExistsException() : base(ErrorStatus)
    {
    }
    private const string ErrorMessage = "A user with same login already exists.";
    private static Status ErrorStatus => new(StatusCode.AlreadyExists, ErrorMessage);
}