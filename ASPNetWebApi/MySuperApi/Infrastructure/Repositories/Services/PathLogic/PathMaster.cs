namespace MySuperApi.Infrastructure.Repositories.Services.PathLogic
{
    public class PathMaster : IPathMaster
    {
        public string CreateFileName(string filename)
        {
            return Path.GetFileNameWithoutExtension(filename) + Guid.NewGuid();

        }

        public string CreatePath(string filename)
        {
            var extension = (filename);
            var fileName = CreateFileName(filename);
            return $"/images/uploaded/{fileName}{extension}";
        }
    }
}
