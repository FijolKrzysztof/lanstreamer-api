using System.Data;
using lanstreamer_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.services;

public class MainService
{
    public MainService(ApiDbContext apiDbContext, AmazonS3Service amazonS3Service, IConfiguration configuration)
    {
        _apiDbContext = apiDbContext;
        _amazonS3Service = amazonS3Service;
        _configuration = configuration;
    }

    private readonly ApiDbContext _apiDbContext;
    private readonly AmazonS3Service _amazonS3Service;
    private readonly IConfiguration _configuration;

    public async Task<ActionResult> Login(User user)
    {
        var users = await (from dbUser in _apiDbContext.Users where dbUser.Mail.Equals(dbUser.Mail) select user).ToListAsync();
        if (users.Count > 0)
        {
            return new StatusCodeResult(StatusCodes.Status200OK);
        }
        await _apiDbContext.Users.AddAsync(user);
        await _apiDbContext.SaveChangesAsync();
        return new StatusCodeResult(StatusCodes.Status201Created);
    }

    public async Task<ActionResult> Authorize(Authorization authorization)
    {
        await _apiDbContext.Authorizations.AddAsync(authorization);
        await _apiDbContext.SaveChangesAsync();
        return new StatusCodeResult(StatusCodes.Status201Created);
    }

    public async Task<string> AppAccess(string authorizationString, string version)
    {
        var configurations = await (
            from dbConfiguration in _apiDbContext.Configurations
            select dbConfiguration).ToListAsync();
        var versionConfig = configurations.Find(configuration => configuration.Key == "version")?.Value;
        if (version != versionConfig)
        {
            throw new VersionNotFoundException();
        }
        var authorizations = await (from dbAuthorization in _apiDbContext.Authorizations
            where dbAuthorization.AuthorizationString.Equals(authorizationString)
            select dbAuthorization).ToListAsync();
        if (authorizations.Count == 0)
        {
            throw new UnauthorizedAccessException();
        }
        _apiDbContext.Authorizations.Remove(authorizations[Index.Start]);
        await _apiDbContext.SaveChangesAsync();
        return configurations.Find(configuration => configuration.Key == "offline_logins")?.Value ?? "0";
    }

    public async Task<String> Download(string operatingSystem)
    {
        var configurations = await (
            from dbConfiguration in _apiDbContext.Configurations
            where dbConfiguration.Key.Equals($"{operatingSystem}_link")
            select dbConfiguration).ToListAsync();
        return configurations.First().Value ?? throw new DataException();
    }
}