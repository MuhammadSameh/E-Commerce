﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IMediaService:IBaseService<Media>
    {
        public Task DeleteByUrl(string url);
    }
}
