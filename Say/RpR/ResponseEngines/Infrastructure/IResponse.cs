namespace RpR.ResponseEngines.Infrastructure
{
    public interface IResponse
    {
        string Get(string target, string innerTarget, string id = "main", bool isJSON = false);
    }
}
