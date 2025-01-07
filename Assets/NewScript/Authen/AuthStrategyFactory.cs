using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthStrategyFactory 
{
    private readonly string googleClientID;
    private string email;
    private string password;

    public AuthStrategyFactory(string googleClientID)
    {
        this.googleClientID = googleClientID;
    }

    public IAuthStrategy CreateStrategy(AuthProvider provider)
    {
        Debug.Log(email);
        Debug.Log(password);

        return provider switch
        {
            AuthProvider.Facebook => new FacebookAuthStrategy(),
            AuthProvider.Google => new GoogleAuthStrategy(googleClientID),
            AuthProvider.Anonymous => new AnonymousAuthStrategy(),
            AuthProvider.Email => new EmailAuthStrategy(email, password),
            _ => throw new ArgumentException("Unsupported provider", nameof(provider))
        };
    }

    public void SetEmailPassword(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}
public enum AuthProvider
{
    Facebook,
    Google, 
    Anonymous, 
    Email
}