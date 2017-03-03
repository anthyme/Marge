using System;

namespace Marge.Infrastructure
{
    public abstract class Command : Value
    {
        public abstract Guid CommandId { get; }

        public bool IsFirstCommand { get; set; }
    }
}
