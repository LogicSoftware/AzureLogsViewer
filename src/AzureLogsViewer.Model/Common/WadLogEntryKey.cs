namespace AzureLogsViewer.Model.Common
{
    public class WadLogEntryKey
    {
        public WadLogEntryKey()
        { }

        public WadLogEntryKey(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        protected bool Equals(WadLogEntryKey other)
        {
            return string.Equals(PartitionKey, other.PartitionKey) && string.Equals(RowKey, other.RowKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WadLogEntryKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((PartitionKey != null ? PartitionKey.GetHashCode() : 0)*397) ^ (RowKey != null ? RowKey.GetHashCode() : 0);
            }
        }
    }
}