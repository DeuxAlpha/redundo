namespace Application.Services.Authentication
{
    public class StandardAuthenticationModel
    {
        public string JwtToken { get; set; }
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}