using System;

namespace DAMZ_1192024.Auth;

public interface IJwtAuthenticationService
{
    string Authenticate(string userName);
}
