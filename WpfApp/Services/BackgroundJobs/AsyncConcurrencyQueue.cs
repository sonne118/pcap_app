using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace wpfapp.Services.BackgroundJobs
{
    public class AsyncConcurrencyQueue<T> : IAsyncEnumerable<T>
    {
        private readonly SemaphoreSlim _enumerationSemaphore = new SemaphoreSlim(1);
        private readonly BufferBlock<T> _bufferBlock = new BufferBlock<T>();

        public void Enqueue(T item)
        {
            _bufferBlock.Post(item);          
        }

        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token = default)
        {
            await _enumerationSemaphore.WaitAsync(token);

            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    yield return await _bufferBlock.ReceiveAsync(token);
                }
            }
            finally
            {
                _enumerationSemaphore.Release();
            }
        }
    }
}
