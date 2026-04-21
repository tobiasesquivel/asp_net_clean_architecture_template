using api.Enums;

namespace api.Models;

public class Photo : Media
{
    // There might be properties in the future
    public Photo(string url) : base(url, EMediaTypes.Photo)
    {

    }

    private Photo() : base() { }

}