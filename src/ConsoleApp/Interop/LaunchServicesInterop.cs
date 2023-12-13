using System.Runtime.InteropServices;
using CFStringRef = System.IntPtr;

namespace SmallsOnline.MacOS.DefaultBrowser;

/// <summary>
/// Implements interop with the macOS ApplicationServices (Launch Services) framework.
/// </summary>
/// <remarks>
/// For more information, see:
/// <see href="https://developer.apple.com/documentation/coreservices/launch_services"/>
/// </remarks>
internal static partial class LaunchServicesInterop
{
    /// <summary>
    /// Returns the bundle identifier of the user’s preferred default handler for the specified URL scheme.
    /// </summary>
    /// <param name="urlScheme">he URL scheme for which the application bundle identifier is to be returned.</param>
    /// <returns>The application bundle identifier of the specified URL scheme.</returns>
    internal static string? GetDefaultHandlerForUrlScheme(string urlScheme)
    {
        using CFString urlSchemeCFString = new(urlScheme);

        var defaultHandler = LSCopyDefaultHandlerForURLScheme(urlSchemeCFString.GetCFStringPtr());

        using CFString defaultHandlerCFString = new(defaultHandler);

        return defaultHandlerCFString.ToString();
    }

    /// <summary>
    /// Sets the user’s preferred default handler for the specified URL scheme.
    /// </summary>
    /// <param name="urlScheme">The URL scheme for which the handler is to be set.</param>
    /// <param name="bundleIdentifier">The bundle identifier that is to be set as the handler for the URL scheme specified by <paramref name="urlScheme"/>.</param>
    internal static void SetDefaultHandlerForUrlScheme(string urlScheme, string bundleIdentifier)
    {
        using var urlSchemeCFString = new CFString(urlScheme);
        using var bundleIdentifierCFString = new CFString(bundleIdentifier);

        LSSetDefaultHandlerForURLScheme(urlSchemeCFString.GetCFStringPtr(), bundleIdentifierCFString.GetCFStringPtr());
    }

    /// <summary>
    /// Returns the bundle identifier of the user’s preferred default handler for the specified URL scheme.
    /// </summary>
    /// <remarks>
    /// <see href="https://developer.apple.com/documentation/coreservices/1441725-lscopydefaulthandlerforurlscheme"/>
    /// </remarks>
    /// <param name="inUrlScheme">The URL scheme for which the application bundle identifier is to be returned.</param>
    /// <returns>The application bundle identifier of the specified URL scheme.</returns>
    [LibraryImport(MacOSLibraries.ApplicationServicesLibrary)]
    private static partial CFStringRef LSCopyDefaultHandlerForURLScheme(CFStringRef inUrlScheme);

    /// <summary>
    /// Sets the user’s preferred default handler for the specified URL scheme.
    /// </summary>
    /// <remarks>
    /// <see href="https://developer.apple.com/documentation/coreservices/1447760-lssetdefaulthandlerforurlscheme"/>
    /// </remarks>
    /// <param name="inUrlScheme">The URL scheme for which the handler is to be set.</param>
    /// <param name="inHandlerBundleId">The bundle identifier that is to be set as the handler for the URL scheme specified by <paramref name="inUrlScheme"/>.</param>
    /// <returns></returns>
    [LibraryImport(MacOSLibraries.ApplicationServicesLibrary)]
    private static partial CFStringRef LSSetDefaultHandlerForURLScheme(CFStringRef inUrlScheme, CFStringRef inHandlerBundleId);
}