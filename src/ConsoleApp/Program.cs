using System.Text;
using SmallsOnline.MacOS.DefaultBrowser;

string[] urlSchemes = ["http", "https"];
Handler[] handlers = new Handler[urlSchemes.Length];

// If an argument is provided, set the default browser to the specified bundle ID
// in the supplied argument.
if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
{
    Console.WriteLine($"Setting default browser to '{args[0]}'...");
    for (int i = 0; i < urlSchemes.Length; i++)
    {
        LaunchServicesInterop.SetDefaultHandlerForUrlScheme(urlSchemes[i], args[0]);
    }

    return;
}

// Otherwise, get the default browser for each URL scheme.
for (int i = 0; i < urlSchemes.Length; i++)
{
    string? bundleId = LaunchServicesInterop.GetDefaultHandlerForUrlScheme(urlSchemes[i]);

    if (bundleId is null)
    {
        continue;
    }

    handlers[i] = new(urlSchemes[i], bundleId);
}

if (handlers.Length == 0)
{
    Console.WriteLine("No default browser set for 'http' or 'https'.");
    return;
}

// Start building the output table.
string urlSchemeHeader = "URL Scheme";
string bundleIdHeader = "Bundle ID";

int maxUrlSchemeLength = handlers.Max(handler => handler.UrlScheme.Length);
int maxBundleIdLength = handlers.Max(handler => handler.AppBundleId?.Length ?? 0);

if (urlSchemeHeader.Length > maxUrlSchemeLength)
{
    maxUrlSchemeLength = urlSchemeHeader.Length;
}

if (bundleIdHeader.Length > maxBundleIdLength)
{
    maxBundleIdLength = bundleIdHeader.Length;
}

StringBuilder sb = new();
sb.AppendLine($"{"URL Scheme".PadRight(maxUrlSchemeLength)} | {"Bundle ID".PadRight(maxBundleIdLength)}");
sb.AppendLine($"{new string('-', maxUrlSchemeLength)}-+-{new string('-', maxBundleIdLength)}");

foreach (Handler handler in handlers)
{
    sb.AppendLine($"{handler.UrlScheme.PadRight(maxUrlSchemeLength)} | {handler.AppBundleId?.PadRight(maxBundleIdLength) ?? "<none>"}");
}

Console.WriteLine();
Console.WriteLine(sb.ToString());
