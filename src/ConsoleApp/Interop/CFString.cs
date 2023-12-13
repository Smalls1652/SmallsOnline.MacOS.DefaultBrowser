using System.Runtime.InteropServices;
using System.Text;

namespace SmallsOnline.MacOS.DefaultBrowser;

/// <summary>
/// Represents a Core Foundation String (CFString) in .NET.
/// </summary>
internal partial class CFString : IDisposable
{
    private IntPtr _cfStringPtr;

    public CFString(string str)
    {
        _cfStringPtr = CFStringCreateWithCString(IntPtr.Zero, str, CFStringEncoding.UTF8);
    }

    internal CFString(IntPtr ptr)
    {
        _cfStringPtr = ptr;
    }

    internal IntPtr GetCFStringPtr()
    {
        return _cfStringPtr;
    }

    internal string? GetCFStringAsString()
    {
        CFStringEncoding encoding = CFStringEncoding.UTF8;

        IntPtr cStrPtr = CFStringGetCStringPtr(_cfStringPtr, encoding);

        if (cStrPtr != IntPtr.Zero)
        {
            return Marshal.PtrToStringUTF8(cStrPtr);
        }

        IntPtr cfDataPtr = CFStringCreateExternalRepresentation(IntPtr.Zero, _cfStringPtr, encoding, 0);
        try
        {
            unsafe
            {
                byte* ptr = CFDataGetBytePtr(cfDataPtr);
                long length = CFStringGetLength(_cfStringPtr);

                return Encoding.UTF8.GetString(ptr, (int)length);
            }
        }
        finally
        {
            Console.WriteLine("CFRelease on cfDataPtr");
            CFRelease(cfDataPtr);
        }
    }

    public void Dispose()
    {
        if (_cfStringPtr != IntPtr.Zero)
        {
            CFRelease(_cfStringPtr);
            _cfStringPtr = IntPtr.Zero;
        }
    }

    public override string ToString()
    {
        return GetCFStringAsString() ?? string.Empty;
    }

    [LibraryImport(MacOSLibraries.CoreFoundationLibrary)]
    private static partial IntPtr CFStringGetCStringPtr(IntPtr theString, CFStringEncoding encoding);

    [LibraryImport(MacOSLibraries.CoreFoundationLibrary)]
    private static unsafe partial byte* CFDataGetBytePtr(IntPtr data);

    [LibraryImport(MacOSLibraries.CoreFoundationLibrary, StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr CFStringCreateExternalRepresentation(IntPtr allocator, IntPtr theString, CFStringEncoding encoding, int lossByte);

    [LibraryImport(MacOSLibraries.CoreFoundationLibrary, StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr CFStringCreateWithCString(IntPtr allocator, string cStr, CFStringEncoding encoding);

    [LibraryImport(MacOSLibraries.CoreFoundationLibrary)]
    private static partial long CFStringGetLength(IntPtr theString);

    [LibraryImport(MacOSLibraries.CoreFoundationLibrary)]
    private static partial void CFRelease(IntPtr cf);

    private enum CFStringEncoding : uint
    {
        UTF8 = 0x08000100
    }
}