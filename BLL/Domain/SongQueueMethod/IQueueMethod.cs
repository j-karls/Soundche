using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.SongQueueMethod
{
    public interface IQueueMethod
    {
        public Track Next();
    }
}
