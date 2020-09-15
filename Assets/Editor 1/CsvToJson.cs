using UnityEngine;
using UnityEditor;
using SimpleJSON;
using System.IO;
using System.Text;

public class CsvToJson
{
    //打包、、、、、、、、、、、、、、、、、、、、、、
    [MenuItem("Tools/打包")]
    public static void BundlerAssets()
    {
        // Debug.LogError("打包Assets");
        BuildPipeline.BuildAssetBundles(GetOutAssetsDirecotion(), BuildAssetBundleOptions.None, BuildTarget.iOS);
        AssetDatabase.Refresh();
    }

    public static string GetOutAssetsDirecotion()
    {
        string assetBundleDirectory = Application.streamingAssetsPath;
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        return assetBundleDirectory;
    }

    //  设置资源名称/////////////////////////////

    [MenuItem("Tools/设置Assetbundle名字")]
    public static void SetAssetBundellabls()
    {
        CheckFileSystemInfo();
    }

    public static void CheckFileSystemInfo()  //检查目标目录下的文件系统
    {
        AssetDatabase.RemoveUnusedAssetBundleNames(); //移除没有用的assetbundlename
        Object obj = Selection.activeObject;    // Selection.activeObject 返回选择的物体
        string path = AssetDatabase.GetAssetPath(obj);//选中的文件夹
        Debug.Log(path);

        CoutineCheck(path);
    }

    public static void CheckFileOrDirectory(FileSystemInfo fileSystemInfo, string path) //判断是文件还是文件夹
    {
        FileInfo fileInfo = fileSystemInfo as FileInfo;
        if (fileInfo != null)
        {
            SetBundleName(path);
        }
        else
        {
            CoutineCheck(path);
        }
    }

    public static void CoutineCheck(string path)   //是文件夹，继续向下
    {
        DirectoryInfo directory = new DirectoryInfo(@path);
        FileSystemInfo[] fileSystemInfos = directory.GetFileSystemInfos();

        foreach (var item in fileSystemInfos)
        {
            // Debug.Log(item);
            int idx = item.ToString().LastIndexOf(@"/");//得到最后一个'\'的索引
            string name = item.ToString().Substring(idx + 1);//截取后面的作为名称
            Debug.Log(name);
            if (!name.Contains(".meta"))
            {
                CheckFileOrDirectory(item, path + "/" + name);  //item  文件系统，加相对路径
            }
        }
    }

    public static void SetBundleName(string path)  //设置assetbundle名字
    {
        var importer = AssetImporter.GetAtPath(path);
        string[] strs = path.Split('.');
        string[] dictors = strs[0].Split('/');
        string name = dictors[dictors.Length - 2] + "_" + dictors[dictors.Length - 1];
        if (importer != null)
        {
            importer.assetBundleVariant = "ab";
            importer.assetBundleName = name;
        }
        else
            Debug.Log("importer是空的");
    }

    [MenuItem("Tools/CsvToJson/生成RotaryTable(转盘)配置数据")]
    static void RotaryTableCsvToJson()
    {
        ToJson("rotaryTable");
    }

    [MenuItem("Tools/CsvToJson/生成Object(物品)配置数据")]
    static void ObjectCsvToJson()
    {
        ToJson("object");
    }

    [MenuItem("Tools/CsvToJson/生成Withdrawal(提现)配置数据")]
    static void withdrawalCsvToJson()
    {
        ToJson("withdrawal");
    }

    [MenuItem("Tools/CsvToJson/生成DailyReward(签到)配置数据")]
    static void dailyRewardCsvToJson()
    {
        ToJson("dailyReward");
    }

    [MenuItem("Tools/ClearPrefs")]
    static void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
    }


    static void ToJson(string fileName)
    {
        var csv = File.ReadAllText(Application.dataPath + "/CSV/" + fileName + ".csv");
        var levelStrContent = csv.Split(new char[] { '\n', '\r' });
        Debug.Log(levelStrContent[2]);
        var jsonArray = new JSONArray();
        for (int i = 3; i < levelStrContent.Length; i++)
        {
            var values = levelStrContent[i].Split(',');
            if (values.Length > 1)
            {
                JSONNode json = new JSONObject();
                var keys = levelStrContent[2].Split(',');
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
