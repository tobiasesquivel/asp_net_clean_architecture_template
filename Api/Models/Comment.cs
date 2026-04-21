using api.Interfaces;

namespace api.Models;

public class Comment : Entity
{
    public int Id { get; private set; }
    public int PublicationId { get; private set; }
    public int? ParentcommentId { get; private set; }

    public Comment? ParentComment { get; private set; }
    public string Text { get; private set; }

    public IReadOnlyList<Comment> Replies => _replies.AsReadOnly();
    private List<Comment> _replies = [];
    public Comment(int publicationId, string text)
    {
        PublicationId = publicationId;
        Text = text;
    }
}