using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Model;

public class SocialLinks
{
    [Url]
    public Uri? LinkedIn { get; init; }

    [Url]
    public Uri? Facebook { get; init; }

    [Url]
    public Uri? GitHub { get; init; }

    [Url]
    public Uri? Twitter { get; init; }

    [Url]
    public Uri? StackOverflow { get; init; }
}