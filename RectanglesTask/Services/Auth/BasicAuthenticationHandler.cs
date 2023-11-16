using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using RectanglesTask.Services.UserInterface;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace RectanglesTask.Services.Auth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue headerValue))
                return AuthenticateResult.Fail("Invalid Authorization Header");

            if (headerValue.Scheme != "Basic")
                return AuthenticateResult.Fail("Invalid Scheme");

            string encodedCredentials = headerValue.Parameter;
            byte[] credentialBytes = Convert.FromBase64String(encodedCredentials);
            string[] credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

            string username = credentials[0];
            string password = credentials[1];

            // Implement user validation logic using IUserService
            var user = await _userService.AuthenticateAsync(username, password);

            if (user == null)
                return AuthenticateResult.Fail("Invalid Username or Password");

            var claims = new[] { new Claim(ClaimTypes.Name, user.Id) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }

}
