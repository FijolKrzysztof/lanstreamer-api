using System.Data;
using Amazon.S3.Model;
using lanstreamer_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.services;

using MongoDB.Driver;

public class MainService
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly IMongoCollection<Authorization> _authorizationsCollection;
    private readonly IMongoCollection<Configuration> _configurationsCollection;
    private readonly IMongoCollection<Referrer> _referrersCollection;
    private readonly IMongoCollection<Download> _downloadsCollecion;
    private readonly AmazonS3Service _amazonS3Service;

    public MainService(IMongoDatabase database, AmazonS3Service amazonS3Service)
    {
        _usersCollection = database.GetCollection<User>("Users");
        _authorizationsCollection = database.GetCollection<Authorization>("Authorizations");
        _configurationsCollection = database.GetCollection<Configuration>("Configurations");
        _referrersCollection = database.GetCollection<Referrer>("Referrers");
        _downloadsCollecion = database.GetCollection<Download>("Downloads");
        _amazonS3Service = amazonS3Service;
    }

    public async Task<ActionResult> Login(User user)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Mail, user.Mail);
        var users = await _usersCollection.FindAsync(filter);

        if (await users.AnyAsync())
        {
            return new StatusCodeResult(StatusCodes.Status200OK);
        }

        await _usersCollection.InsertOneAsync(user);
        return new StatusCodeResult(StatusCodes.Status201Created);
    }

    public async Task<ActionResult> Authorize(string authorizationString, User user)
    {
        var authorization = new Authorization
        {
            AuthorizationString = authorizationString,
            Timestamp = DateTime.UtcNow
        };

        var filter = Builders<User>.Filter.Eq(u => u.Mail, user.Mail);
        var users = await _usersCollection.FindAsync(filter);

        if (!await users.AnyAsync())
        {
            await _usersCollection.InsertOneAsync(user);
        }

        var yesterday = DateTime.UtcNow.AddDays(-1);
        var outdatedAuthorizationsFilter = Builders<Authorization>.Filter.Lt(a => a.Timestamp, yesterday);
        await _authorizationsCollection.DeleteManyAsync(outdatedAuthorizationsFilter);

        await _authorizationsCollection.InsertOneAsync(authorization);
        return new StatusCodeResult(StatusCodes.Status201Created);
    }

    public async Task<string> AppAccess(string authorizationString, string version)
    {
        var configurations = await _configurationsCollection.Find(_ => true).ToListAsync();
        var versionConfig = configurations.Find(configuration => configuration.Key == "version")?.Value;
        if (version != versionConfig)
        {
            throw new VersionNotFoundException();
        }

        var authorizationFilter = Builders<Authorization>.Filter.Eq(a => a.AuthorizationString, authorizationString);
        var foundAuthorization = await _authorizationsCollection.Find(authorizationFilter).FirstOrDefaultAsync();
        if (foundAuthorization == null)
        {
            throw new UnauthorizedAccessException();
        }

        await _authorizationsCollection.DeleteOneAsync(authorizationFilter);
        return configurations.Find(configuration => configuration.Key == "offline_logins")?.Value ?? "0";
    }

    public async Task<string> Download(string operatingSystem)
    {
        var download = new Download();
        download.Timestamp = DateTime.UtcNow;

        await _downloadsCollecion.InsertOneAsync(download);
        
        var s3Objects = (await _amazonS3Service.GetObjectList("downloads"))
            .FindAll(obj => obj.Key.Contains(operatingSystem))
            .FindAll(obj => _amazonS3Service.GetFilePermissions(obj.Key).Result
                .FindAll(grant => grant.Grantee.URI == "http://acs.amazonaws.com/groups/global/AllUsers"
                                  && (grant.Permission.Value == "READ" || grant.Permission.Value == "READ_ACP"))
                .Count == 2);

        s3Objects.Sort((obj1, obj2) => obj2.LastModified.CompareTo(obj1.LastModified));
        var s3Object = s3Objects.First();
        return $"https://lanstreamer.s3.eu-west-2.amazonaws.com/{s3Object.Key}";
    }

    public async Task<ActionResult> SaveReferrer(Referrer referrer)
    {
        referrer.Timestamp = DateTime.UtcNow;

        await _referrersCollection.InsertOneAsync(referrer);
        return new StatusCodeResult(StatusCodes.Status201Created);
    }
}
