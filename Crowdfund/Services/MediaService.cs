using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.CreateOptions;
using Crowdfund.Services.Interfaces;
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
            _context.Add(media);
            return _context.SaveChanges() > 0 ? media : null;
        }

        public bool DeleteMedia(int id)
        {
            var MediaToDelete = _context.Set<Media>().Where(m => m.MediaId == id).SingleOrDefault();
            _context.Remove(MediaToDelete);
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
