using System.Net;

while (true)
{
    string spotifyCode = "";
    while (spotifyCode == "")
    {
        try
        {
            //Getting a valid Spotify Link, trying to convert to Spotifycodes download scheme.
            Console.WriteLine("Paste your Spotify Link below:");
            var input = Console.ReadLine();
            if (input != null && input != "" && input.StartsWith("https://open.spotify.com/"))
            {
                spotifyCode = input.Substring(("https://open.spotify.com/").Length);
                spotifyCode = spotifyCode.Substring(0, spotifyCode.IndexOf("?"));
                spotifyCode = spotifyCode.Replace("/", "%3A");
                if (!spotifyCode.Contains("spotify")) spotifyCode = "spotify%3A" + spotifyCode;
                spotifyCode = @"https://spotifycodes.com/downloadCode.php?uri=svg%2F000000%2Fwhite%2F640%2F" + spotifyCode;
            }
            else
            {
                Console.WriteLine("Invalid URL");
                continue;
            }
        }
        catch
        {
            Console.WriteLine("Invalid URL");
            continue;
        }
    }
    //Downloading the actual file
    try
    {
        using (var client = new HttpClient())
        {
            //Downloading the Spotify Code and saving the file
            Console.WriteLine("Downloading the Spotify Code...");
            string content = client.GetAsync(spotifyCode).Result.Content.ReadAsStringAsync().Result;
            //Removing the background so OpenSCAD can read the file properly
            content = content.Replace("<rect x=\"0\" y=\"0\" width=\"400\" height=\"100\" fill=\"#000000\"/>", "");
            File.WriteAllText(AppContext.BaseDirectory + "OpenSCAD\\code.svg", content);

            //Adding a keyring maybe
            string keyChoice = "";
            while (keyChoice != "y" && keyChoice != "n") 
            {
                Console.WriteLine();
                Console.WriteLine("Do you want your Model to have a Keychain ring? [y/n]");
                keyChoice = Console.ReadKey().Key.ToString().ToLower();
            }
            Console.WriteLine();

            //Changing the name of the output file
            string nameChoice = "code.stl";
            Console.WriteLine("Give your output file a name. [ENTER] for \"code.stl\":");
            var nameInput = Console.ReadLine();
            string strNameInput = nameInput != null ? (string)nameInput : ""; 
            nameInput = MakeValidFileName(strNameInput);
            if (nameInput != "") nameChoice = nameInput;
            if (!nameChoice.EndsWith(".stl")) nameChoice = nameChoice + ".stl";


            //Converting the File
            String codePath = "\""+ AppContext.BaseDirectory + "OpenSCAD\\codeConverter.scad\"";
            var process = System.Diagnostics.Process.Start(System.AppContext.BaseDirectory + @"OpenSCAD\openscad.exe", "-o " + nameChoice + " -D keychain=" + (keyChoice == "y" ? "true " : "false ") + codePath);
            Console.WriteLine("Converting with OpenSCAD... (This may take a few seconds)");
            process.WaitForExit();
            Console.WriteLine();
            Console.WriteLine("Success! File saved in program directory as \"" + nameChoice + "\"");
            Console.WriteLine();
        }
    }
    catch 
    {
        Console.WriteLine("Unexpected Error, restarting...");
    }
}

string MakeValidFileName(string name)
{
    string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
    string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

    return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
}