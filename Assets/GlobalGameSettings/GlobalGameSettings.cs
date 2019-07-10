using System.IO;

public class GlobalGameSettingsContainer
{
    public byte Players;
    public byte Difficulty;
}

public class GlobalGameSettings
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
            return settings.Difficulty;
        }
    }
}
