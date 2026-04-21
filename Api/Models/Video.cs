using api.Enums;

namespace api.Models;

public class Video : Media
{
    public int DurationSeconds { get; private set; }

    public Video(string url, int durationSeconds) : base(url, EMediaTypes.Video)
    {
        DurationSeconds = durationSeconds;
    }

}