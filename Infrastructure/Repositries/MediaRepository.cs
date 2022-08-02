using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositries
{
    public class MediaRepository : BaseRepository<Media>, IMediaRepository
    {
        private readonly EcommerceContext _cntxt;
        public MediaRepository(EcommerceContext cntxt) : base(cntxt)
        {
            this._cntxt = cntxt;
        }

        public async Task DeleteByUrl(string url)
        {
          var media = await  _cntxt.Medias.Where(m => m.PicUrl == url).FirstOrDefaultAsync();
            if (media != null)  _cntxt.Medias.Remove(media);
            await _cntxt.SaveChangesAsync();
        }
    }
}
