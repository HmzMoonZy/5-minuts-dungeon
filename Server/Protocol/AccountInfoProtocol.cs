using System;

[Serializable]
public class AccountInfoProtocol
{
    public string Username { get; private set; }
    public string Password { get; private set; }

    public AccountInfoProtocol(string username, string password)
    {
        this.Username = username;
        this.Password = password;
    }

    public override string ToString()
    {
        return "username : " + Username + " password : " + Password;
    }
}
