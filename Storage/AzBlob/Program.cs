using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

Console.WriteLine("Azure Blob Storage exercise\n");

// Run the examples asynchronously, wait for the results before proceeding
ProcessAsync().GetAwaiter().GetResult();

Console.WriteLine("Press enter to exit the sample application.");
Console.ReadLine();

static async Task ProcessAsync()
{
    // Copy the connection string from the portal in the variable below.
    string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=learnblog;AccountKey=hQH1JaKLoji8hhU7oa/nw3megYA2YUR5zyzUZ2Ns0o9tAv3bjSz5L74ELfVqHeJo3fNPlQqbyfHa+AStKS3X3Q==;EndpointSuffix=core.windows.net";

    // Create a client that can authenticate with a connection string
    BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

    // 1. Create container
    string containerName = "learnblob-container" + Guid.NewGuid().ToString();
    BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
    Console.WriteLine("A container named '" + containerName + "' has been created. ");
    Console.WriteLine("Press 'Enter' to continue.");
    Console.ReadLine();

    // 2. upload blog
    // create a test file
    string fileName = "testfile" + Guid.NewGuid().ToString() + ".txt";
    string localFilePath = Path.Combine("./data/", fileName);
    await File.WriteAllTextAsync(localFilePath, "Hello, World!");

    // Get a reference to the blob
    BlobClient blobClient = containerClient.GetBlobClient(fileName);
    Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
    // Open the file and upload its data
    using FileStream uploadFileStream = File.OpenRead(localFilePath);
    await blobClient.UploadAsync(uploadFileStream, true);
    Console.WriteLine("\nThe file was uploaded. We'll verify by listing the blobs next.");
    Console.WriteLine("Press 'Enter' to continue.");
    Console.ReadLine();

    // Delete the container and clean up local files created
    Console.WriteLine("\n\nDeleting blob container...");
    await containerClient.DeleteAsync();

    Console.WriteLine("Deleting the local source and downloaded files...");
    File.Delete(localFilePath);

    Console.WriteLine("Finished cleaning up.");
}