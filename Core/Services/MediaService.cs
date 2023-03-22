using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class MediaService : BaseService<Media>,IMediaService
    {
        private readonly IBaseRepository<Media> _baseRepository;

        public MediaService(IBaseRepository<Media> baseRepository) : base(baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task DeleteByUrl(string url)
        {
            var result = await _baseRepository.FindByCondition(m => m.PicUrl == url);
            if(result.FirstOrDefault() != null)
                _baseRepository.Delete(result.FirstOrDefault());
        }
    }
}
