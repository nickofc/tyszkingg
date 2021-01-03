using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Api.V5;
using TwitchLib.Api.V5.Models.Clips;
using TwReplay.Data;

namespace TwReplay.Reupload.Services
{
    public class ReuploadService
    {
        private readonly ReuploadClipService _reuploadClipService;
        private readonly ApplicationDbContext _dbContext;

        public ReuploadService(ReuploadClipService reuploadClipService,
            ApplicationDbContext dbContext)
        {
            _reuploadClipService = reuploadClipService;
            _dbContext = dbContext;
        }

        public async Task Reupload(IEnumerable<Clip> clips,
            CancellationToken cancellationToken)
        {
            var clipItemsToAdd = new List<ClipItem>();

            foreach (var clip in clips)
            {
                var reuploadPayload = await _reuploadClipService
                    .Reupload(clip, cancellationToken);

                var clipItem = new ClipItem
                {
                    Slug = clip.GetSlug(),
                    Description = clip.Title,
                    CreatedAt = clip.CreatedAt,
                    AddedAt = DateTimeOffset.Now,
                    ClipLinkItem = new ClipLinkItem
                    {
                        Availability = Availability.Unknown,
                        ProviderType = reuploadPayload.Provider,
                        Url = reuploadPayload.Url
                    }
                };

                clipItemsToAdd.Add(clipItem);
            }

            await _dbContext.ClipItems.AddRangeAsync(clipItemsToAdd, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}