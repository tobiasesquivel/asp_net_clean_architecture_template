namespace api.Models;

public class PostItem
{

    public int Id { get; private set; }
    public Media Media { get; private set; }
    public int Order { get; private set; }
    public PostItem(Media media, int order)
    {
        Media = media;
        Order = order;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private PostItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}