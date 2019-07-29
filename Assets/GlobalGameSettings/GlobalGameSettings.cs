using System.IO;

public class GlobalGameSettingsContainer
{
    public byte Players = 1;
    public byte Difficulty = 1;
}

public static class GlobalGameSettings
{
    private static GlobalGameSettingsContainer settings;
    public static byte Players
    {
        get
        {
            if (settings == null)
            {
                settings = XML_to_Class.LoadClassFromXML<GlobalGameSettingsContainer>("SavedData" + Path.DirectorySeparatorChar + "GlobalGameSettings.xml");
            }
            if (settings == null) //file was missing, use default.
            {
                settings = new GlobalGameSettingsContainer();
            }
            return settings.Players;
        }
    }
    public static byte Difficulty
    {
        get
        {
            if (settings == null)
            {
                settings = XML_to_Class.LoadClassFromXML<GlobalGameSettingsContainer>("SavedData" + Path.DirectorySeparatorChar + "GlobalGameSettings.xml");
            }
            if (settings == null) //file was missing, use default.
            {
                settings = new GlobalGameSettingsContainer();
            }
            return settings.Difficulty;
        }
    }
}
