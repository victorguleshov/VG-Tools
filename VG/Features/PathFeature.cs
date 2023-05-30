using System.IO;
using UnityEditor;
using UnityEngine;

namespace VG.Features
{
    public static class PathFeature
    {
#if UNITY_EDITOR
        [MenuItem("Tools/VG/Clear All")]
#endif
        public static void ClearAll()
        {
            ClearPlayerPrefs();
            ClearPersistentData();
        }

#if UNITY_EDITOR
        [MenuItem("Tools/VG/Clear Player Prefs")]
#endif
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

#if UNITY_EDITOR
        [MenuItem("Tools/VG/Clear Persistent Data")]
#endif
        public static void ClearPersistentData()
        {
            foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
            {
                var dataDir = new DirectoryInfo(directory);
                dataDir.Delete(true);
            }

            foreach (var file in Directory.GetFiles(Application.persistentDataPath))
            {
                var fileInfo = new FileInfo(file);
                fileInfo.Delete();
            }
        }

#if UNITY_EDITOR
        [MenuItem("Tools/VG/Open Persistent Data Path")]
        public static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

#endif
    }
}