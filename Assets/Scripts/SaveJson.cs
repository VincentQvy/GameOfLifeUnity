using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public static class SaveJson
{
    public static string directory = "TestDataSave";
    public static string fileName = "save.txt";
    static bool canLoad = true;
    // Start is called before the first frame update

    public static void saveName(string savename)
    {
        canLoad = true;
        if (savename.Length == 1)
        {
            canLoad = false;
        }
        fileName = savename + ".txt";
    }

    public static void Save(SaveObject so)
    {
        string dir =Path.Combine( Application.persistentDataPath , directory);
        Debug.Log(dir);

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(so, true);
        File.WriteAllText(Path.Combine(dir, fileName), json);
    }

    public static SaveObject Load(SaveTest saveTest)
    {
        string dir = Path.Combine(Application.persistentDataPath, directory, fileName);
        SaveObject so = new SaveObject();
        if (canLoad)
        {

            if (File.Exists(dir))
            {
                string json = File.ReadAllText(dir);
                so = JsonUtility.FromJson<SaveObject>(json);

            }
            else
            {
                Debug.Log("Save file does not exist");
            }

        }
        return so;
    }
}
