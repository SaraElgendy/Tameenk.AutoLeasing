namespace Tameenk.AutoLeasing.Identity
{
    public class IdentitySettings
    {
        public string JWT_Secret { get; set; }
        public bool ValidateIssuerSigningKey { get; set; } = true;
        public bool ValidateIssuer { get; set; } = false;
        public bool ValidateAudience { get; set; } = false;
        public int Expires { get; set; } = 10;
        public bool SaveToken { get; set; } = false;
        public bool RequireHttpsMetadata { get; set; } = false;
    }
}
