namespace SmallsOnline.MacOS.DefaultBrowser;

/// <summary>
/// Represents a handler for a URL scheme in a macOS default browser application.
/// </summary>
public class Handler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Handler"/> class with the specified URL scheme.
    /// </summary>
    /// <param name="urlScheme">The URL scheme to handle.</param>
    public Handler(string urlScheme)
    {
        UrlScheme = urlScheme;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Handler"/> class with the specified URL scheme and app bundle ID.
    /// </summary>
    /// <param name="urlScheme">The URL scheme to handle.</param>
    /// <param name="appBundleId">The bundle ID of the default browser application.</param>
    public Handler(string urlScheme, string appBundleId) : this(urlScheme)
    {
        AppBundleId = appBundleId;
    }

    /// <summary>
    /// The URL scheme to handle.
    /// </summary>
    public string UrlScheme { get; set; }

    /// <summary>
    /// The bundle ID set as the default for the <see cref="UrlScheme"/>.
    /// </summary>
    public string? AppBundleId { get; set; } 
}