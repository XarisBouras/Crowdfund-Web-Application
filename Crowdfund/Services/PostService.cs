using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.CreateOptions;
using Crowdfund.Services.Interfaces;
using Crowdfund.Services.UpdateOptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crowdfund.Services
{
    public class PostService : IPostService
    {
        private DataContext _context;
        public PostService(DataContext context)
        {
            _context = context;
        }

        public Post CreatePost(CreatePostOptions options)
        {
            if (options == null)
            {
                return null;
            }
            var post = new Post()
            {
                Text = options.text
            };
            _context.Add(post);
            return _context.SaveChanges() > 0 ? post : null;
        }

        public Post UpdatePost(UpdatePostOptions options)
        {
            if (options == null)
            {
                return null;
            }
            var postToUpdate = _context.Set<Post>().Where(p => p.PostId == options.PostId).SingleOrDefault();
            if (!string.IsNullOrEmpty(options.text))
            {
                postToUpdate.Text = options.text;
            }
            _context.SaveChanges();
            return postToUpdate;
        }

        public Post GetPostById(int id)
        {
            var postIds = _context.Set<Post>().AsQueryable();
            var post = postIds.Where(u => u.PostId == id).SingleOrDefault();
            return post;
        }

        public IList<Post> GetAllPosts()
        {
            var list = _context.Set<Post>().ToList();
            return list;
        }

        public bool DeletePost(int id)
        {
            var PostToDelete = _context.Set<Post>().Where(p => p.PostId == id).SingleOrDefault();
            _context.Remove(PostToDelete);
            _context.SaveChanges();
            
            var checking= _context.Set<Post>().Where(p => p.PostId == id).SingleOrDefault();
            if (checking != null)
            {
                Console.WriteLine("The POST has not deleted");
                return false;
            }
            else
            {
                Console.WriteLine("The POST has been successfully deleted");
                return true;
            }
        }
    }
}
