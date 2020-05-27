using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Options.PostOptions;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IPostService
    {
        Result<Post> CreatePost(CreatePostOptions options);
        Post UpdatePost(Post post, UpdatePostOptions options);
        Post GetPostById(int? id);
        bool DeletePost(Post post);
    }
}
