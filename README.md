# SeafClient

This project is a wrapper for the [Seafile](https://www.seafile.com) web api  for .Net as portable class library (PCL).
It can be used in desktop apps, Windows Store applications as well as in apps for Windows Phone 7 and 8.x.

The aim is to create a library to easily access a Seafile server and the files stored there through a .Net application in a strong-typed fashion (no custom JSON parsing and with meaningful error messages, etc. The library uses the async/await methods.

## Usage example (C#)

```C#
using SeafClient;

async Task Demo()
{
    Uri serverUri= new Uri("https://seacloud.cc", UriKind.Absolute);
    string username = "testuser@internet.com";
    char[] password = new char[] { 't', 'e', 's', 't' };

    try {
        // authenticate with the Seafile server and retrieve a Session
        SeafSession session = await SeafSession.Establish(serverUri, username, password);
        
        // connection was successful
        // btw: the password array is now empty (all elements are now char 0)
        // now retrieve some information about the account
        var accountInfo = await session.CheckAccountInfo();
        Debug.WriteLine(String.Format("Nickname: {0}\nUsed Storage: {1:d} Bytes\nQuota: {2}",
            accountInfo.Nickname, accountInfo.Usage, accountInfo.Usage, accountInfo.HasUnlimitedSpace ? "unlimited" : 
            (accountInfo.Quota.ToString() + "Bytes")));
        // get the url of the user avatar
        var userAvatar = await session.GetUserAvatar(128);
        Debug.WriteLine("Url to user's avatar: " + userAvatar.Url);

        // get the libraries
        var libraries = await session.ListLibraries();
        foreach (var lib in libraries)
            Debug.WriteLine(lib.Name + " " + lib.Timestamp.ToString());

        // list root contents of the first library
        var firstLib = libraries.FirstOrDefault();
        if (firstLib != null)
        {
            var content = await session.ListDirectory(firstLib, "/");
            foreach (var dirEntry in content)
                if (dirEntry.Type == SeafClient.Types.DirEntryType.File)
                    Debug.WriteLine(dirEntry.Name);
                else
                    Debug.WriteLine(dirEntry.Name + " - " + dirEntry.Size.ToString() + " Bytes");
        }

    } catch (SeafException e)
    {
                Debug.WriteLine(String.Format("An error occured: {0} (ErrorCode: {1} ({2}))", e.Message, 
                e.SeafError.SeafErrorCode, e.SeafError.HttpStatusCode));
    }
}
```
