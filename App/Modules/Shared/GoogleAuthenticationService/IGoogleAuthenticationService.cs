using Google.Apis.Auth;

namespace lanstreamer_api.App.Modules.Shared.GoogleAuthenticationService;

public interface IGoogleAuthenticationService
{
    public Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken);
}