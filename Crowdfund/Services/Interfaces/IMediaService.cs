using Crowdfund.Models;
using Crowdfund.Services.CreateOptions;

namespace Crowdfund.Services.Interfaces
{
    public interface IMediaService
    {
        Media CreateMedia(CreateMediaOptions options);
        bool DeleteMedia(int id);
    }
}
