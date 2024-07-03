using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileHandler
{
    private string dataDirPath = string.Empty;
    private string dataFileName = string.Empty;

    public FileHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData LoadFromFile()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData readData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToRead = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToRead = reader.ReadToEnd();
                    }
                }

                readData =  JsonUtility.FromJson<GameData>(dataToRead);
            }catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        return readData;
    }

    public void SaveToFile(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToWrite = JsonUtility.ToJson(data, true);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToWrite);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}
