using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SaveJson : MonoBehaviour
{
    public string m_directory = "TestDataSave";
    public string m_JsonfileName = "save.txt";
    public string m_PNGfileName = "save.png";
    bool _canLoad = true;
    // Start is called before the first frame update

    public void SaveName(string savename, string saveFormat)
    {
        _canLoad = true;
        if (savename.Length == 1)
        {
            _canLoad = false;
        }
        m_JsonfileName = savename + saveFormat;
        m_PNGfileName = savename + saveFormat;
        Debug.Log(m_JsonfileName);
    }

    public async void SavePNG(SaveObject so)
    {
        Texture2D image = new Texture2D(so.m_largeur, so.m_hauteur, TextureFormat.RGB24, false);
        for (int y = 0; y < so.m_hauteur; y++)
        {
            for (int x = 0; x < so.m_largeur; x++)
            {
                Color cellColor = so.m_grid[x + y * so.m_largeur] == 1 ? Color.white : Color.black;
                image.SetPixel(x, y, cellColor);
            }
        }
        image.Apply();

        // Encode texture into PNG
        byte[] bytes = image.EncodeToPNG();
        var dirPath = Application.persistentDataPath;
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        using (FileStream sourceStream = new FileStream(Path.Combine(dirPath, "TestDataSave", m_PNGfileName), FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
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
        else
        {
            string dir =Path.Combine( Application.persistentDataPath , m_directory);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonUtility.ToJson(so, true);
            File.WriteAllText(Path.Combine(dir, m_JsonfileName), json);
        }
    }

    public SaveObject ReadPng(SaveObject so)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(Path.Combine(Application.persistentDataPath, "TestDataSave", m_PNGfileName)))
        {
            fileData = File.ReadAllBytes(Path.Combine(Application.persistentDataPath, "TestDataSave", m_PNGfileName));
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            so.m_hauteur = tex.height;
            so.m_largeur = tex.width;
            so.m_vitesse = 1;
            so.m_bordure = 1;
            so.m_grid = new List<int>();
            for (int y = 0; y < tex.height; y++)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    so.m_grid.Add(tex.GetPixel(x, y) == Color.black ? 0 : 1);
                }
            }         
        }
        return so;
    }

    public SaveObject Load(SaveTest saveTest, string format)
    {
        if (_canLoad)
        {
            string dir = Path.Combine(Application.persistentDataPath, m_directory, m_JsonfileName);
            string dirPNG = Path.Combine(Application.persistentDataPath, m_directory, m_PNGfileName);
            SaveObject so = new SaveObject();
            if (File.Exists(dir) || File.Exists(dirPNG))
            {
                if (format == ".png")
                {
                    so = ReadPng(so);
                }
                else
                {
                    string json = File.ReadAllText(dir);
                    so = JsonUtility.FromJson<SaveObject>(json);
                }
            }
            else
            {
                Debug.Log("Save file does not exist");
            }
            GridManager.instance.PosCam(so.m_largeur, so.m_hauteur);
            return so;
        }
        else
        {
            return null;
        }
    }
}
