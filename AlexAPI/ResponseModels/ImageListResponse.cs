namespace AlexAPI.ResponseModels
{
    public class ImageListResponse
    {
        public int Total { get; set; }
        public ImageResponse Primary { get; set; }
        public List<ImageResponse> Interior { get; set; }
        public List<ImageResponse> Exterior { get; set; }
        public List<ImageResponse> Other { get; set; }
    }

    public class ImageResponse
    {
        public string Title { get; set; }
        public string PhotographerName { get; set; }
        public URLResponse Urls { get; set; }
    }

    public class URLResponse
    {
        public string ExtraLarge { get; set; }
    }
}
