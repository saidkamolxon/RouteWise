namespace RouteWise.Service.Services.DitatTms;

public record DitatTmsApiCredentials(
    string BaseUrl,
    string AccountId,
    string ApplicationRole,
    string Username,
    string Password,
    string Token
);
