using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TwReplay.Data;
using TwReplay.Twitch.Abstraction;

namespace TwReplay.Services
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
            foreach (var clip in clips)
            {
                var reuplaodPayload = await _reuploadClipService
                    .Reupload(clip, cancellationToken);

                var clipItem = new ClipItem
                {
                    Id = Guid.NewGuid().ToString(),
                    ClipInfo =
                    {
                        Id = Guid.NewGuid().ToString(),
                        Slug = clip.Slug,
                        Title = clip.Title,
                        Game = clip.Game,
                        CreatedAt = clip.CreatedAt
                    },
                    Links =
                    {
                        new ClipLinkInfo
                        {
                            Id = Guid.NewGuid().ToString(),
                            Provider = reuplaodPayload.Provider,
                            Url = reuplaodPayload.Url,
                        }
                    },
                    AddedAt = DateTimeOffset.UtcNow
                };

                await _dbContext.Clips.AddAsync(clipItem, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}