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

        public async Task<T> TryDequeue(CancellationToken token = default)
        {
            await _enumerationSemaphore.WaitAsync(token);

            try
            {
                while (await _bufferBlock.OutputAvailableAsync())
                {
                     return await _bufferBlock.ReceiveAsync();

                }
            }
            finally 
            {
                _enumerationSemaphore.Release();
            }
            return await Task.FromResult<T>(default(T));

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
