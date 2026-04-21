using api.Enums;

namespace api.Models;

public abstract class Media
{
    public int Id { get; private set; }
    public string Url { get; private set; }
    public EMediaTypes Type { get; private set; }
    protected Media(string url, EMediaTypes type)
    {
        Url = url;
        Type = type;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected Media() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

}