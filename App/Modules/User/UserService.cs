using System.Net;
using Google.Apis.Auth;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.App.Modules;

public class UserService
{
    public async Task<ActionResult<UserDto>> Create(UserDto userDto, string idToken)
    {
        try
        {
            await GoogleJsonWebSignature.ValidateAsync(idToken);
        }
        catch (InvalidJwtException)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Invalid google id token");
        }
        
        
        // try
        // {
        //     // Weryfikacja tokenu dostępowego z Google
        //     var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
        //
        //     // Pomyślna weryfikacja - przetwarzanie danych użytkownika
        //     var userId = payload.Subject; // ID użytkownika Google
        //     var email = payload.Email; // Adres e-mail użytkownika
        //     var pictureUrl = payload.Picture; // URL do zdjęcia profilowego użytkownika
        //
        //     // Tutaj możesz zapisać informacje o użytkowniku w bazie danych lub przekazać je dalej w odpowiedzi
        //
        //     return Ok(new { UserId = userId, Email = email, PictureUrl = pictureUrl });
        // }
        // catch (InvalidJwtException)
        // {
        //     return BadRequest("Invalid token");
        // }
    }
    
    public Task<ActionResult> Update(UserDto userDto)
    {
        throw new NotImplementedException();
    }
}