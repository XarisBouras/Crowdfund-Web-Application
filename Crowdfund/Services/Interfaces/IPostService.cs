using Crowdfund.Services.CreateOptions;
using System.Collections.Generic;
using Crowdfund.Services.UpdateOptions;
using Crowdfund.Models;

namespace Crowdfund.Services.Interfaces
{
    public interface IPostService
    {
        Post CreatePost(CreatePostOptions options);
        Post UpdatePost(UpdatePostOptions options,int? postId);
        Post GetPostById(int? id);
        IList<Post> GetAllPosts(int? postId);
        bool DeletePost(int? pojectId, int? id);
    }
}
