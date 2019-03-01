namespace GraphQLExample.Infrastructure.Storages
{
    public class StorageSelector
    {
        public StorageType StorageType { get; }

        public StorageSelector(StorageType storageType)
            => StorageType = storageType;
    }
}