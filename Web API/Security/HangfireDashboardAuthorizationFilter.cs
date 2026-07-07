using System.Net;
using Hangfire.Dashboard;

namespace Web_API.Security
{
    // Hangfire panelini yalnızca localhost/loopback isteklerine açar.
    // (JWT Bearer, tarayıcı gezintisiyle açılan panele uymadığı için uzaktan erişim tümden kapatılır.)
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var remoteIp = httpContext?.Connection?.RemoteIpAddress;
            if (remoteIp == null)
                return false;

            return IPAddress.IsLoopback(remoteIp);
        }
    }
}
