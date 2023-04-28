using System.Data;
using Amazon.S3.Model;
using lanstreamer_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.services;

public class MainService
{
    public MainService(ApiDbContext apiDbContext, AmazonS3Service amazonS3Service)
    {
        _apiDbContext = apiDbContext;
        _amazonS3Service = amazonS3Service;
    }

    private readonly ApiDbContext _apiDbContext;
    private readonly AmazonS3Service _amazonS3Service;

    public async Task<ActionResult> Login(User user)
    {
        var users = await (from dbUser in _apiDbContext.Users where dbUser.Mail.Equals(dbUser.Mail) select user)
            .ToListAsync();
        if (users.Count > 0)
        {
            return new StatusCodeResult(StatusCodes.Status200OK);
        }

        await _apiDbContext.Users.AddAsync(user);
        await _apiDbContext.SaveChangesAsync();
        return new StatusCodeResult(StatusCodes.Status201Created);
    }

    public async Task<ActionResult> Authorize(String authorizationString, User user)
    {
        var authorization = new Authorization();
        authorization.AuthorizationString = authorizationString;
        authorization.Timestamp = DateTime.Now.ToUniversalTime();

        var users = await (
            from dbUser in _apiDbContext.Users
            where dbUser.Mail.Equals(user.Mail)
            select dbUser).ToListAsync();

        if (users.Count == 0)
        {
            await _apiDbContext.Users.AddAsync(user);
        }

        DateTime yesterday = DateTime.Today.AddDays(-1);
        var outdatedAuthorizations = await (
            from dbAuthorization in _apiDbContext.Authorizations
            where dbAuthorization.Timestamp.Value < yesterday.ToUniversalTime()
            select dbAuthorization).ToListAsync();
        
        outdatedAuthorizations.ForEach(authorization =>
        {
            _apiDbContext.Authorizations.Remove(authorization);
        });

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

        var foundAuthorizationList = await (
            from dbAuthorization in _apiDbContext.Authorizations
            where dbAuthorization.AuthorizationString.Equals(authorizationString)
            select dbAuthorization).ToListAsync();
        if (foundAuthorizationList.Count == 0)
        {
            throw new UnauthorizedAccessException();
        }

        _apiDbContext.Authorizations.Remove(foundAuthorizationList[Index.Start]);
        await _apiDbContext.SaveChangesAsync();
        return configurations.Find(configuration => configuration.Key == "offline_logins")?.Value ?? "0";
    }

    public async Task<String> Download(string operatingSystem)
    {
        List<S3Object> s3Objects = (await _amazonS3Service.GetObjectList("downloads"))
            .FindAll(obj => obj.Key.Contains(operatingSystem))
            .FindAll(obj => _amazonS3Service.GetFilePermissions(obj.Key).Result
                    .FindAll(grant => grant.Grantee.URI == "http://acs.amazonaws.com/groups/global/AllUsers"
                                      && (grant.Permission.Value == "READ" || grant.Permission.Value == "READ_ACP"))
                    .Count == 2
            );
        s3Objects.Sort((obj1, obj2) => obj2.LastModified.CompareTo(obj1.LastModified));
        S3Object s3Object = s3Objects.First();
        return $"https://lanstreamer.s3.eu-west-2.amazonaws.com/{s3Object.Key}";
    }

    public async Task<ActionResult> SaveReferrer(Referrer referrer)
    {
        referrer.Timestamp = DateTime.Now.ToUniversalTime();

        await _apiDbContext.Referrers.AddAsync(referrer);
        await _apiDbContext.SaveChangesAsync();

        return new StatusCodeResult(StatusCodes.Status201Created);
    }
}