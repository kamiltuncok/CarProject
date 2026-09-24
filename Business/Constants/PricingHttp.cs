namespace Business.Constants
{
    /// <summary>
    /// RL fiyatlandırma servisine giden HttpClient'ın adı.
    /// Startup ve Autofac aynı ada baktığı için yapılandırma tek yerde kalır.
    /// </summary>
    public static class PricingHttp
    {
        public const string ClientName = "pricing";
    }
}
