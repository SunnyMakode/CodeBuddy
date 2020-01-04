using Microsoft.AspNetCore.Http;

namespace CodeBuddy.Api.Helpers
{
    public static class Extension
    {
        public static void AddApplicationError(this HttpResponse response, string message )
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Access-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
