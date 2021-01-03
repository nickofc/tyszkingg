using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
            foreach (var clip in clips)
            {
                var reuplaodPayload = await _reuploadClipService
                    .Reupload(clip, cancellationToken);

                throw new NotImplementedException();
                
                var clipItem = new ClipItem
                {
                    Id = Guid.NewGuid().ToString(),
                    ClipInfo =
                    {
                        Id = Guid.NewGuid().ToString(),
                        // Slug = clip.Slug,
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