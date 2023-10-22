using System.Data;
using lanstreamer_api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace lanstreamer_api.services;

public class MainService
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly IMongoCollection<Authorization> _authorizationsCollection;
    private readonly IMongoCollection<Configuration> _configurationsCollection;
    private readonly IMongoCollection<Referrer> _referrersCollection;
    private readonly IMongoCollection<Download> _downloadsCollecion;
    private readonly IMongoCollection<Feedback> _feedbacksCollection;
    private readonly AmazonS3Service _amazonS3Service;

    public MainService(IMongoDatabase database, AmazonS3Service amazonS3Service)
    {
        _usersCollection = database.GetCollection<User>("Users");
        _authorizationsCollection = database.GetCollection<Authorization>("Authorizations");
        _configurationsCollection = database.GetCollection<Configuration>("Configurations");
        _referrersCollection = database.GetCollection<Referrer>("Referrers");
        _downloadsCollecion = database.GetCollection<Download>("Downloads");
        _feedbacksCollection = database.GetCollection<Feedback>("Feedbacks");
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
}
