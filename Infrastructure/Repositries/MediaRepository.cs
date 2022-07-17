using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositries
{
    internal class MediaRepository : BaseRepository<Media>, IMediaRepository
    {
        private readonly EcommerceContext _cntxt;
        public MediaRepository(EcommerceContext cntxt) : base(cntxt)
        {
            this._cntxt = cntxt;
        }


    }
}
