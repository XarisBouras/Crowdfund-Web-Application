using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.MediaOptions;
using System;


namespace Crowdfund.Core.Services
{
    public class MediaService : IMediaService
    {
        private readonly DataContext _context;

        public MediaService(DataContext context)
        {
            _context = context;
        }

        public Result<Media> CreateMedia(CreateMediaOptions options)
        {
            if (options == null || string.IsNullOrWhiteSpace(options.MediaUrl))
            {
                return null;
            }

            var url = options.MediaUrl;

            if (options.MediaType == (MediaType) MediaType.Video)
            {
                url = url.Trim();
                if (!url.Contains("youtube.com"))
                {
                    return Result<Media>.Failed(StatusCode.BadRequest, "Only youtube videos supported");
                }
            }

            var media = new Media
            {
                MediaUrl = url,
                MediaType = options.MediaType
            };

            _context.Add(media);

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<Media>.Failed(StatusCode.InternalServerError, ex.Message);
            }

            return rows <= 0
                ? Result<Media>.Failed(StatusCode.InternalServerError, "Media Could Not Be Created")
                : Result<Media>.Succeed(media);
        }

        public bool DeleteMedia(Media mediaToDelete)
        {
            _context.Remove(mediaToDelete);
            return _context.SaveChanges() > 0;
        }
    }
}