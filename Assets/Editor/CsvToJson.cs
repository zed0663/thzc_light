using UnityEngine;
using UnityEditor;
using SimpleJSON;
using System.IO;
using System.Text;

public class CsvToJson
{
    [MenuItem("Tools/BuildAssetsBundle")]
    static void BuildAssetsBundle()
    {
        string path = Application.streamingAssetsPath + "/AssetBundle";
        if (Directory.Exists(path))
        {
            Directory.Delete(path);
        }

        Directory.CreateDirectory(path);
        
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.iOS);
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/CsvToJson/RoleIdentity")]
    static void RoleIdentityCsvToJson()
    {
        ToJson("roleIdentity");
    }

    [MenuItem("Tools/CsvToJson/Stage")]
    static void LevelCsvToJson()
    {
        ToJson("stage");
    }

    [MenuItem("Tools/CsvToJson/Idiom")]
    static void IdiomCsvToJson()
    {
        ToJson("idiom");
    }

    [MenuItem("Tools/CsvToJson/DailyReward")]
    static void TaskCsvToJson()
    {
        ToJson("dailyReward");
    }

    [MenuItem("Tools/CsvToJson/RedBagCash")]
    static void RedBagCashCsvToJson()
    {
        ToJson("redBagCash");
    }

    [MenuItem("Tools/ClearPrefs")]
    static void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    static void ToJson(string fileName)
    {
        var csv = File.ReadAllText(Application.dataPath+ "/CSV/"+ fileName+".csv");
        var levelStrContent = csv.Split(new char[] { '\n', '\r' });
        var jsonContent = new StringBuilder();
        var jsonArray = new JSONArray();
        for (int i = 1; i < levelStrContent.Length; i++)
        {
            var values = levelStrContent[i].Split(',');
            if (values.Length > 1)
            {
                JSONObject json = new JSONObject();
                var keys = levelStrContent[0].Split(',');
                for (int j = 0; j < keys.Length; j++)
                {
                    json.Add(keys[j], values[j]);
                }
                jsonArray.Add(json);
            }
        }

        var bytes = Encoding.UTF8.GetBytes(jsonArray.ToString());
        var path = Application.dataPath + string.Format("/Resources/Configs/Json/{0}.json", fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        using (FileStream fsWrite = new FileStream(Application.dataPath + string.Format("/Resources/Configs/Json/{0}.json", fileName), FileMode.OpenOrCreate))
        {
            fsWrite.Write(bytes, 0, bytes.Length);
        }
        Debug.Log(string.Format("Refresh:{0}\n", fileName) + jsonArray.ToString());
        AssetDatabase.Refresh();
    }
}
