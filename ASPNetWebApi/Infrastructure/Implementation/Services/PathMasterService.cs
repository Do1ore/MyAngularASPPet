using Infrastructure.Abstraction.Services;

namespace Infrastructure.Implementation.Services.PathLogic
{
    public class PathMasterService : IPathMasterService
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
