using System.Net.Mime;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Grpc.Core;
using SixLabors.ImageSharp.Formats.Png;
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
        ResizeParameters? resizeParameters;
        byte[] firstChunk;

        // set the values from the first stream chunk
        if (await requestStream.MoveNext())
        {
            fileName = requestStream.Current.Name;
            mimeType = requestStream.Current.MimeType;
            resizeParameters = requestStream.Current.ResizeParameters;
            firstChunk = requestStream.Current.Chunk.ToByteArray();
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

        // process the file stream
        using var stream = new MemoryStream();
        using var image = Image.Load(stream);
        if (resizeParameters != null)
        {
            image.Mutate(x => x.Resize(
                new ResizeOptions
                {
                    Size = new Size((int)resizeParameters.Width, (int)resizeParameters.Height),
                    Mode = resizeParameters.KeepAspectRatio ? ResizeMode.Max : ResizeMode.Crop
                }));
        }
        using var outStream = new MemoryStream();
        image.Save(outStream, new PngEncoder());
        var blobTask = blobClient.UploadAsync(
            outStream,
            new BlobHttpHeaders { ContentType = MediaTypeNames.Image.Png });

        // process the rest of the stream
        await stream.WriteAsync(firstChunk);
        while (await requestStream.MoveNext())
        {
            var chunk = requestStream.Current.Chunk.ToByteArray();
            await stream.WriteAsync(chunk);
        }
        stream.Close();

        // wait for the blob to finish uploading
        var blobResult = await blobTask;
        return new FileUploadResponse
        {
            Url = blobClient.Uri.ToString(),
            MimeType = MediaTypeNames.Image.Png
        };

    }
};
