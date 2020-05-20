using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.CreateOptions;
using Crowdfund.Services.Interfaces;
using Crowdfund.Services.Options.MediaOptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Linq;


namespace Crowdfund.Services
{
    public class MediaService: IMediaService
    {
        private DataContext _context;
        public MediaService(DataContext context)
        {
            _context = context;
        }

        public Media CreateMedia(CreateMediaOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var media = new Media()
            {
                MediaType = (MediaType)options.MediaType
               
            };
            var urlChecking = options.MediaUrl;
            if(media.MediaType == (MediaType)MediaType.Video)
            {

                urlChecking = urlChecking.Trim();
                if(urlChecking.Contains("youtube.com"))
                {
                    media.MediaUrl = urlChecking;
                    Console.WriteLine("valid Video");
                }
                else
                {
                    Console.WriteLine("Not valid Video");
                    return null;
                }
            }
            else
            {
                media.MediaUrl = urlChecking;
                Console.WriteLine("Image");
            }
            
            return media;
        }

        public bool DeleteMedia(int? projectId, int? id)
        {

            var MediaToDelete = _context.Set<Project>()
                                        .Include(m=>m.Medias)
                                        .Where(p => p.ProjectId == projectId)
                                        .SingleOrDefault();

            foreach(var item in MediaToDelete.Medias)
            {
                if (id == item.MediaId)
                {
                    _context.Remove(item);
                }
            }
            
            _context.SaveChanges();

            var checking = _context.Set<Media>().Where(m => m.MediaId== id).SingleOrDefault();
            if (checking != null)
            {
                Console.WriteLine("The Media has not deleted");
                return false;
            }
            else
            {
                Console.WriteLine("The Media has been successfully deleted");
                return true;
            }
        }


    }
}
