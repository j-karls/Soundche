using System;
using System.Collections.Generic;
using System.Text;

namespace Soundche.Core.Domain.SongQueueMethod
{
    public interface IQueueMethod
    {
        public Track Next();
    }
}
