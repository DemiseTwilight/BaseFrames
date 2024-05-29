using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class FindReferences : Editor {
    [MenuItem("Assets/Find References", false, 10)]
    static private void Find() {
        Dictionary<string, string> guidDics = new Dictionary<string, string>();
        foreach(Object o in Selection.objects) {
            string path = AssetDatabase.GetAssetPath(o);
            if(!string.IsNullOrEmpty(path)) {
                string guid = AssetDatabase.AssetPathToGUID(path);
                if(!guidDics.ContainsKey(guid)) {
                    guidDics[guid] = o.name;
                }
            }
        }

        if(guidDics.Count > 0) {
            List<string> withoutExtensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset" };
            string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
            for(int i = 0; i < files.Length; i++) {
                string file = files[i];
                if(i % 20 == 0) {
                    bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配资源中", file, (float)i / (float)files.Length);
                    if(isCancel) {
                        break;
                    }
                }
                var readAllText = File.ReadAllText(file);
                foreach(KeyValuePair<string, string> guidItem in guidDics) {
                    if(Regex.IsMatch(readAllText, guidItem.Key)) {
                        Debug.Log(string.Format("name: {0} file: {1}", guidItem.Value, file), AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));
                    }
                }
            }
            EditorUtility.ClearProgressBar();
            Debug.Log("匹配结束");
        }
    }

    static private string GetRelativeAssetsPath(string path) {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }
}
