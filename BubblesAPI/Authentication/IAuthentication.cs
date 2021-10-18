namespace BubblesAPI.Authentication
{
    public interface IAuthentication
    {
        public string GetToken(string id, string key, string issuer);
    }
}