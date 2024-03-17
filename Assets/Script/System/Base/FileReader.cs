using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class FileReader
{
    public static List<string> ReadFile(string path)
    {
        path = Application.dataPath+ "/" + path;

        if (!File.Exists(path))
            Debug.LogError(path + "  No exsist file");

        StreamReader reader = new StreamReader(path, Encoding.UTF8);
        List<string> lines = new List<string>();
        string tempLine;
        while ((tempLine = reader.ReadLine()) != null)
        {
            lines.Add(tempLine);
        }
        return lines;
    }
}