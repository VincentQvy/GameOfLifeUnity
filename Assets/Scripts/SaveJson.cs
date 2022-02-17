using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SaveJson : MonoBehaviour
{
    public string directory = "TestDataSave";
    public string JsonfileName = "save.txt";
    public string PNGfileName = "save.png";
    bool canLoad = true;
    // Start is called before the first frame update

    public void saveName(string savename, string saveFormat)
    {
        canLoad = true;
        if (savename.Length == 1)
        {
            canLoad = false;
        }
        JsonfileName = savename + saveFormat;
        PNGfileName = savename + saveFormat;
    }

    public async void SavePNG(SaveObject so)
    {
        Texture2D image = new Texture2D(so.Largeur, so.Hauteur, TextureFormat.RGB24, false);
        for (int y = 0; y < so.Hauteur; y++)
        {
            for (int x = 0; x < so.Largeur; x++)
            {
                Color cellColor = so.Grid[x + y * so.Largeur] == 1 ? Color.white : Color.black;
                image.SetPixel(x, y, cellColor);
            }
        }
        image.Apply();

        // Encode texture into PNG
        byte[] bytes = image.EncodeToPNG();
        var dirPath = Application.persistentDataPath;
        Debug.Log(dirPath);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        using (FileStream sourceStream = new FileStream(Path.Combine(dirPath, PNGfileName), FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
        {
            await sourceStream.WriteAsync(bytes, 0, bytes.Length);
        }
        Object.Destroy(image);
    }

    public void Save(SaveObject so, string format)
    {
        if (format == ".png")
        {
            SavePNG(so);
        }
        string dir =Path.Combine( Application.persistentDataPath , directory);
        Debug.Log(dir);

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(so, true);
        File.WriteAllText(Path.Combine(dir, JsonfileName), json);
    }

    public async void ReadPng(SaveObject so)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(Application.persistentDataPath + "/SaveGameOfLife/" + PNGfileName))
        {
            fileData = File.ReadAllBytes(Application.persistentDataPath + "/SaveGameOfLife/" + PNGfileName);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }

    }

    public SaveObject Load(SaveTest saveTest, string format)
    {
        if (canLoad)
        {
            string dir = Path.Combine(Application.persistentDataPath, directory, JsonfileName);
            SaveObject so = new SaveObject();
            if (File.Exists(dir))
            {
                string json = File.ReadAllText(dir);
                so = JsonUtility.FromJson<SaveObject>(json);
                if (format == ".png")
                {
                    ReadPng(so);
                }
            }
            else
            {
                Debug.Log("Save file does not exist");
            }

            return so;
        }
        else
        {
            return null;
        }
    }
}
