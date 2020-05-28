using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.PostOptions;
using System;

namespace Crowdfund.Core.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;
        public PostService(DataContext context)
        {
            _context = context;
        }

        public Result<Post> CreatePost(CreatePostOptions options)
        {
            if (options == null || string.IsNullOrWhiteSpace(options.Title) || string.IsNullOrWhiteSpace(options.Text))
            {
                return Result<Post>.Failed(StatusCode.BadRequest, "Options Not Valid");
            }

            var post = new Post()
            {
                Title = options.Title,
                Text = options.Text
            };
            
            _context.Add(post);

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<Post>.Failed(StatusCode.InternalServerError, ex.Message);
            }
            return rows <= 0 ?
                Result<Post>.Failed(StatusCode.InternalServerError, "Post Could Not Be Created")
                : Result<Post>.Succeed(post);           
        }

        public Post UpdatePost(Post postToUpdate, UpdatePostOptions options)
        {
            
            if (!string.IsNullOrWhiteSpace(options.Text))
            {
                postToUpdate.Text = options.Text;
            }
            
            if (!string.IsNullOrWhiteSpace(options.Title))
            {
                postToUpdate.Title = options.Title;
            }

            return postToUpdate;
        }

        public Post GetPostById(int? id)
        {
            return id==null ? null : _context.Set<Post>().Find(id);
        }

        public bool DeletePost(Post postToDelete)
        {
            _context.Remove(postToDelete);
            return _context.SaveChanges() > 0;
        }
    }
}
