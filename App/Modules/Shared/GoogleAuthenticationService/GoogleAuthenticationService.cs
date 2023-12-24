using Google.Apis.Auth;

namespace lanstreamer_api.App.Modules.Shared.GoogleAuthenticationService;

public class GoogleAuthenticationService : IGoogleAuthenticationService
{
    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
        return payload;
    }
}