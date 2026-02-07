using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace EIOP.Tools;

public static class ModDetectionData
{
    private static JObject DataCache;

    public static Dictionary<string, string> KnownCheats
    {
        get
        {
            if (Data == null)
                return new Dictionary<string, string>();

            return ((JObject)Data["Known Cheats"]).ToObject<Dictionary<string, string>>();
        }
    }

    public static Dictionary<string, string> KnownMods
    {
        get
        {
            if (Data == null)
                return new Dictionary<string, string>();

            return ((JObject)Data["Known Mods"]).ToObject<Dictionary<string, string>>();
        }
    }

    private static JObject Data
    {
        get
        {
            if (DataCache != null)
                return DataCache;

            try
            {
                using HttpClient    httpClient   = new();
                HttpResponseMessage dataResponse = httpClient.GetAsync("https://www.poopoovr.co.uk/data").Result;
                using Stream        dataStream   = dataResponse.Content.ReadAsStreamAsync().Result;
                using StreamReader  dataReader   = new(dataStream);
                string              content      = dataReader.ReadToEnd().Trim();

                if (content.Contains("<pre>") && content.Contains("</pre>"))
                {
                    int startIndex = content.IndexOf("<pre>") + 5;
                    int endIndex   = content.IndexOf("</pre>");
                    content = content.Substring(startIndex, endIndex - startIndex).Trim();
                }

                DataCache = JObject.Parse(content);

                return DataCache;
            }
            catch
            {
                return null;
            }
        }
    }
}
