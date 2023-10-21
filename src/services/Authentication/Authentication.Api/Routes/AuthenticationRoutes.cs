using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SwapSquare.Authentication.Application.Authentication.Dtos;
using SwapSquare.Authentication.Application.Common.Services.Jwt;
using SwapSquare.Authentication.Application.Common.Services.Password;
using SwapSquare.Authentication.Application.Users;
using SwapSquare.Authentication.Domain.Aggregates.User;

namespace SwapSquare.Authentication.Api.Routes;

public static class AuthenticationRoutes
{
    public const string basePath = "/authentication";
    public static IEndpointRouteBuilder MapAuthenticationRoutes(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(basePath);
        group.MapPost("/signin", SignIn);
        group.MapPost("/signup", SignUp);
        return endpoints;
    }

    public static async Task<Results<Ok<TokenPairResponse>, Conflict, UnprocessableEntity>>
        SignUp(
        CreateUserCredentialsDto createUserCredentialsDto,
        IUserRepository userRepository,
        IPasswordService passwordService,
        IJwtService jwtService)
    {
        (string username, string email, string password) = createUserCredentialsDto;
        (byte[] hash, byte[] salt) = passwordService.GenerateHashAndSalt(password);
        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt
        };
        user.MarkAsNewlyCreated();
        var tokenPair = jwtService.GenerateTokensForUser(user);

        await userRepository.AddAsync(user);
        try
        {
            await userRepository.UnitOfWork.SaveChangesAsync();
        }
        catch
        {
            return TypedResults.Conflict();
        }

        return TypedResults.Ok(tokenPair);
    }

    public static async Task<IResult> SignIn(
        SignInCredentialsDto signInUserCredentialsDto,
        IUserRepository userRepository,
        IPasswordService passwordService,
        IJwtService jwtService)
    {
        (string username, string password) = signInUserCredentialsDto;
        var user = await userRepository.GetByUsernameAsync(username);
        if (user is null)
        {
            return TypedResults.NotFound();
        }
        if (!user.HasCredentialsAuthentication())
        {
            return TypedResults.UnprocessableEntity("User does not have credentials authentication method");
        }
        if (!passwordService.IsValidPassword(password, user.PasswordHash!, user.PasswordSalt!))
        {
            return TypedResults.BadRequest();
        }

        var tokenPair = jwtService.GenerateTokensForUser(user);
        return TypedResults.Ok(tokenPair);
    }
}