using Crowdfund.Models;
using Crowdfund.Services.CreateOptions;
using Crowdfund.Services.Options.MediaOptions;

namespace Crowdfund.Services.Interfaces
{
    public interface IMediaService
    {
        Media CreateMedia(CreateMediaOptions options);
        bool DeleteMedia(int? projectId, int? id);
    }
}
