using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{

    public List<Dictionary<string, object>> data_Quiz;
    string ResourcesPath = "RandomQuiz2";

    private void Awake()
    {

        if (GameObject.Find("GameDataManager_DontDestroy"))
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        gameObject.name = "GameDataManager_DontDestroy";

        if (data_Quiz == null)
        {
            data_Quiz = Read(ResourcesPath);

            for (var i = 0; i < data_Quiz.Count; i++)
            {
                Debug.Log(data_Quiz[i]["번호"] + " _ " + data_Quiz[i]["왼쪽문제"] + " _ " + data_Quiz[i]["보기1"] + " _ " +
                data_Quiz[i]["오른쪽문제"] + " _ " + data_Quiz[i]["보기2"] + "_" + data_Quiz[i]["정답"]);
            }
        }
    }

    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();

            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                value = value.Replace("<br>", "\n");
                value = value.Replace("<c>", ",");
                object finalvalue = value;
                int n;
                float f;

                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}