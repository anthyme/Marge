using System;

namespace Marge.Infrastructure
{
    public interface ITransactionFactory
    {
        ITransaction Create();
    }

    public interface ITransaction : IDisposable
    {
        void Complete();
    }
}