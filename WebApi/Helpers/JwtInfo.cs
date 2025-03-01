namespace WebApi.Helpers
{
    public struct JwtInfo
    {
        public static string Issuer = "ChidoSakht.ir";
        public static string Audience = "ChidoSakht.ir";
        public static string SecretKey = "{5CBAA322-1371-4756-A487-3F90A0DFB78C}";
        public static DateTime Expires = DateTime.Now.AddDays(10);
        public static DateTime NotBefore = DateTime.Now;
        public static DateTime ExpiresRefreshToken = DateTime.Now.AddDays(30);
    }
}
