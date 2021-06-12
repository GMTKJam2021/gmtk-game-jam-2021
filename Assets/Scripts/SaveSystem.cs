using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    /// <summary>Saves the game in the appropriate path</summary>
    /// <param name="data">The game data to be saved</param>
    public static void Save(SaveData data)
    {
        string path;
        if (Application.isEditor)
            path = Application.persistentDataPath + "/Orbital.game";
        else
            path = "idbfs/Orbital.game";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>Loads the game from the appropriate path</summary>
    public static SaveData Load()
    {
        string path;
        if (Application.isEditor)
            path = Application.persistentDataPath + "/Orbital.game";
        else
            path = "idbfs/Orbital.game";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save File Not Found");
            return new SaveData();
        }

    }
}
