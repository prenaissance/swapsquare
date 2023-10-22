using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Grpc.Core;
using SwapSquare.GrpcFileUpload;

namespace SwapSquare.FileUpload.Services;

public class FileUploadService(
    BlobServiceClient blobServiceClient
    ) : GrpcFileUpload.FileUpload.FileUploadBase
{
    public override async Task<FileUploadResponse> UploadPublicFile(
        IAsyncStreamReader<FileUploadRequest> requestStream,
        ServerCallContext context)
    {
        string fileName;
        string mimeType;

        // set the values from the first stream chunk
        if (await requestStream.MoveNext())
        {
            fileName = requestStream.Current.Name;
            mimeType = requestStream.Current.MimeType;
        }
        else
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "No file name provided"));
        }

        // create a new blob container
        var containerClient = blobServiceClient.GetBlobContainerClient("public");
        await containerClient.CreateIfNotExistsAsync(
            PublicAccessType.BlobContainer);

        // create a new blob
        var blobClient = containerClient.GetBlobClient(fileName);
        if (await blobClient.ExistsAsync())
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "File already exists"));
        }

        //
    }
};
