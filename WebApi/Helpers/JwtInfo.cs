namespace WebApi.Helpers
{
    public struct JwtInfo
    {
        //public static string Issuer = "ChidoSakht.ir";
        //public static string Audience = "ChidoSakht.ir";
        public static string SecretKey = "{518D20C1-7467-47AE-AA90-3F554F0D980E}";
        //public static DateTime Expires = DateTime.Now.AddDays(10);
        public static DateTime NotBefore = DateTime.Now;
        //public static DateTime ExpiresRefreshToken = DateTime.Now.AddDays(30);
    }
}
