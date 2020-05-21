using Crowdfund.Services.CreateOptions;
using System.Collections.Generic;
using Crowdfund.Services.UpdateOptions;
using Crowdfund.Models;

namespace Crowdfund.Services.Interfaces
{
    public interface IPostService
    {
        Post CreatePost(CreatePostOptions options);
        Post UpdatePost(Post post, UpdatePostOptions options);
        Post GetPostById(int? id);
        bool DeletePost(Post post);
    }
}
