using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using MRTK.Tutorials.AzureCloudServices.Scripts.Domain;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;

namespace AzureStorage
{
    public class StorageAccountManager : MonoBehaviour
    {
        [Header("Connect Settings")]
        [SerializeField]
        private string connectionString = default;

        [Header("Table Settings")]
        [SerializeField]
        private string tableName = "objects";
        [SerializeField]
        private string partitionKey = "main";
        [SerializeField]
        private bool tryCreateTableOnStart = true;

        [Header("Blob Settings")]
        [SerializeField]
        private string blockBlobContainerName = "tracked-objects-thumbnails";
        [SerializeField]
        private bool tryCreateBlobContainerOnStart = true;
        [SerializeField]
        private Texture2D testTex;
        public Texture2D returned;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onStorageAccountManagerReady = default;
        [SerializeField]
        private UnityEvent onStorageAccountManagerInitFailed = default;

        private CloudStorageAccount storageAccount;

        private CloudTableClient cloudTableClient;
        private CloudTable cloudTable;

        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;

        private async void Awake()
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);

            await InitializeTable();
            await InitializeContainer();

            onStorageAccountManagerReady?.Invoke();
        }

        public async Task InitializeTable()
        {
            cloudTableClient = storageAccount.CreateCloudTableClient();
            cloudTable = cloudTableClient.GetTableReference(tableName);
            if (tryCreateTableOnStart)
            {
                try
                {
                    if (await cloudTable.CreateIfNotExistsAsync())
                    {
                        Debug.Log($"Created table {tableName}.");
                    }
                }
                catch (StorageException ex)
                {
                    Debug.LogError("Failed to connect with Azure Storage.\nIf you are running with the default storage emulator configuration, please make sure you have started the storage emulator.");
                    Debug.LogException(ex);
                    onStorageAccountManagerInitFailed?.Invoke();
                }
            }
        }

        public async Task InitializeContainer()
        {
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(blockBlobContainerName);
            if (tryCreateBlobContainerOnStart)
            {
                try
                {
                    if (await blobContainer.CreateIfNotExistsAsync())
                    {
                        Debug.Log($"Created container {blockBlobContainerName}.");
                    }
                }
                catch (StorageException ex)
                {
                    Debug.LogError("Failed to connect with Azure Storage.\nIf you are running with the default storage emulator configuration, please make sure you have started the storage emulator.");
                    Debug.LogException(ex);
                    onStorageAccountManagerInitFailed?.Invoke();
                }
            }

        }

        public async Task<bool> UploadOrUpdateTable(Table tableData)
        {
            if (string.IsNullOrWhiteSpace(tableData.PartitionKey))
            {
                tableData.PartitionKey = partitionKey;
            }

            var insertOrMergeOperation = TableOperation.InsertOrMerge(tableData);
            var result = await cloudTable.ExecuteAsync(insertOrMergeOperation);

            return result.Result != null;
        }

        public async Task<List<Table>> GetAllTableData()
        {
            var query = new TableQuery<Table>();
            var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, null);

            return segment.Results;
        }

        public async Task<Table> FindTableEntityById(string id)
        {
            var retrieveOperation = TableOperation.Retrieve<Table>(partitionKey, id);
            var result = await cloudTable.ExecuteAsync(retrieveOperation);
            var trackedObject = result.Result as Table;

            return trackedObject;
        }

        public async Task<Table> FindTableEntityByName(string entityName)
        {
            var query = new TableQuery<Table>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, entityName)));
            var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, null);

            return segment.Results.FirstOrDefault();
        }

        public async Task<bool> DeleteTableEntity(Table entity)
        {
            var deleteOperation = TableOperation.Delete(entity);
            var result = await cloudTable.ExecuteAsync(deleteOperation);

            return result.HttpStatusCode == (int)HttpStatusCode.OK;
        }

        public async Task<string> UploadBlob(byte[] data, string blobName)
        {
            var blockBlob = blobContainer.GetBlockBlobReference(blobName);
            await blockBlob.UploadFromByteArrayAsync(data, 0, data.Length);

            return blockBlob.Name;
        }

        public async Task<byte[]> DownloadBlob(string blobName)
        {
            var blockBlob = blobContainer.GetBlockBlobReference(blobName);
            using (var stream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(stream);
                return stream.ToArray();
            }
        }

        public async Task<bool> DeleteBlob(string blobName)
        {
            var blockBlob = blobContainer.GetBlockBlobReference(blobName);
            return await blockBlob.DeleteIfExistsAsync();
        }
    }
}
