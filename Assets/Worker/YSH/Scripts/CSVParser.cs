using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CSVParser
{
    const string DATA_DIRECTORY = "Data";

    public static bool GetDataString(string fileName, out string[] lines)
    {
        TextAsset text = Resources.Load<TextAsset>($"{DATA_DIRECTORY}/{fileName}");
        if (text == null)
        {
            Debug.LogError("잘못된 파일입니다!");
            lines = null;
            return false;
        }
        lines = text.text.Split('\n');
        return true;
    }
}
