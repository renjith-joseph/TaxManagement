using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using NUnit.Framework.Constraints;

namespace TaxManagement.Security
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserRepository _userRepository;
       
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock, IUserRepository userRepository
            ) : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Basic ".Length).Trim();
                System.Console.WriteLine(token);
                var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var credentials = credentialstring.Split(':');
                if (_userRepository.Authenticate(credentials[0],credentials[1]).Result)
                {
                    var claims = new[] { new Claim("name", credentials[0]), new Claim(ClaimTypes.Role, "Admin") };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }

                Response.StatusCode = 401;
                Response.Headers.Add("WWW-Authenticate", "Basic realm=\"dotnetthoughts.net\"");
                return Task.FromResult(AuthenticateResult.Fail("Invalid User Credentials"));
            }
            else
            {
                Response.StatusCode = 401;
                Response.Headers.Add("WWW-Authenticate", "Basic realm=\"dotnetthoughts.net\"");
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }
    }
}
