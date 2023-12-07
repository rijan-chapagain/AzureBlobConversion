using System.Text;
using Azure.Storage.Blobs;

class Program
{
    static async Task Main(string[] args)
    {
        const string connectionString = "BlobStorageConnString__";
        const string containerName = "BlobStorageContainer__";

        await ConvertBlobNamesToLowercase(connectionString, containerName);
    }

    static async Task ConvertBlobNamesToLowercase(string connectionString, string containerName)
    {
        // Create BlobServiceClient and BlobContainerClient
        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

        // Iterate through each blob in the container
        await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
        {
            Console.WriteLine("In foreach looping blob items: " + blobItem.Name);

            // Get the blob name and convert it to lowercase
            string blobName = blobItem.Name;
            string lowercaseBlobName = blobName.ToLower();

            // Check if conversion is needed
            if (blobName != lowercaseBlobName)
            {
                // Get source and destination blob clients 
                var sourceBlob = blobContainerClient.GetBlobClient(blobName);
                var destinationBlob = blobContainerClient.GetBlobClient(lowercaseBlobName);

                // Download content from the source blob
                var blobDownloadInfo = await sourceBlob.DownloadAsync();
                using (var streamReader = new StreamReader(blobDownloadInfo.Value.Content))
                {
                    // Read content and convert to lowercase
                    var content = await streamReader.ReadToEndAsync();
                    var lowercaseContent = content.ToLower();

                    // Upload the lowercase content to the destination blob
                    using (var streamWriter = new StreamWriter(new MemoryStream(Encoding.UTF8.GetBytes(lowercaseContent))))
                    {
                        await destinationBlob.UploadAsync(streamWriter.BaseStream);
                    }
                }

                // Delete the source blob after successful conversion
                await sourceBlob.DeleteIfExistsAsync();

                Console.WriteLine($"Converted {blobName} to {lowercaseBlobName}");
            }
        }

        Console.WriteLine("Conversion completed.");
    }
}
