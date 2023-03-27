using ST.Services.Image.Interfaces;

namespace ST.Services.Image.Models;

public class ImageSingleton : IImageSingleton
{
    public List<Guid> Images { get; set; } = new()
    {
        new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")
    };
}