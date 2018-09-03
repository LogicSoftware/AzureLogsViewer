using System.Collections;
using System.Collections.Generic;

namespace AzureLogsViewer.Model.Common
{
    class BatchCollection<T> : IEnumerable<IEnumerable<T>>
    {
        private readonly IEnumerable<T> _source;
        private readonly int _batchSize;

        public BatchCollection(IEnumerable<T> source, int batchSize)
        {
            _source = source;
            _batchSize = batchSize;
        }

        public IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            List<T> batch = new List<T>(_batchSize);
            foreach (var item in _source)
            {
                batch.Add(item);

                if (batch.Count == _batchSize)
                {
                    yield return batch;
                    batch = new List<T>(_batchSize);
                }
            }

            if (batch.Count > 0)
            {
                yield return batch;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}