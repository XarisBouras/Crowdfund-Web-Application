using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.CreateOptions;
using Crowdfund.Services.Interfaces;
using Crowdfund.Services.UpdateOptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

        public Post UpdatePost(UpdatePostOptions options,int? postId)
        {
            if (options == null || postId==null)
            {
                return null;
            }
            var postToUpdate = _context.Set<Post>().Where(p => p.PostId == postId).SingleOrDefault();
            if (!string.IsNullOrWhiteSpace(options.text))
            {
                postToUpdate.Text = options.text;
            }
            _context.SaveChanges();
            return postToUpdate;
        }

        public Post GetPostById(int? id)
        {
            if(id==null)
            {
                return null;
            }
            var postIds = _context.Set<Post>().AsQueryable();
            var post = postIds.Where(u => u.PostId == id).SingleOrDefault();
            return post;
        }

        public IList<Post> GetAllPosts(int? postId)
        {
            if(postId==null)
            {
                return null;
            }
            var list = _context.Set<Post>().Where(p=>p.PostId==postId).ToList();
            return list;
        }

        public bool DeletePost(int? projectId,int? postid)
        {
            if(projectId == null || postid ==null)
            {
                return false;
            }
            var PostToDeleteFromProject = _context.Set<Project>()
                                       .Include(p => p.Posts)
                                       .Where(p => p.ProjectId == projectId)
                                       .SingleOrDefault();
            if (PostToDeleteFromProject==null)
            {
                return false;
            }
            
            foreach (var item in PostToDeleteFromProject.Posts)
            {
                if (postid == item.PostId)
                {
                    _context.Remove(item);
                }
            }

            _context.SaveChanges();
            
            var checking= _context.Set<Post>().Where(p => p.PostId == postid).SingleOrDefault();
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
