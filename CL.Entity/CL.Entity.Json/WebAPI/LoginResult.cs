
namespace CL.Json.Entity.WebAPI
{
    public class LoginResult : JsonResult
    {
        public LoginData Data { set; get; }
    }

    public class OutLoginResult : JsonResult
    {
        public string Token { set; get; }
    }
}
