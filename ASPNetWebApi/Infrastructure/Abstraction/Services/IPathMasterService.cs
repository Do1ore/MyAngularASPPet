namespace Infrastructure.Abstraction.Services
{
    public interface IPathMasterService
    {
        public string CreatePath(string filename);
        public string CreateFileName(string filename);
    }
}
