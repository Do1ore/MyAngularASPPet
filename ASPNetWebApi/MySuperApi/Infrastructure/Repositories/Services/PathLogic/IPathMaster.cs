namespace MySuperApi.Infrastructure.Repositories.Services.PathLogic
{
    public interface IPathMaster
    {
        public string CreatePath(string filename);
        public string CreateFileName(string filename);
    }
}
