#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VG.Editor.Features
{
    [InitializeOnLoad]
    public class DeleteEmptyFoldersFeature
    {
        static DeleteEmptyFoldersFeature()
        {
            FindAndDeleteEmptyFolders();
            // EditorApplication.quitting += FindAndDeleteEmptyFolders;
        }

        [MenuItem("Tools/VG/Delete Empty Folders %#d")]
        public static void FindAndDeleteEmptyFolders()
        {
            var emptyDirectories = GetEmptyDirectories();
            if (emptyDirectories == null)
                return;

            foreach (var emptyDirectory in emptyDirectories) DeleteEmptyDirectory(emptyDirectory);
        }

        private static List<DirectoryInfo> GetEmptyDirectories()
        {
            var emptyDirectories = new List<DirectoryInfo>();
            var assetDirectory = new DirectoryInfo(Application.dataPath);

            WalkDirectoryTree(assetDirectory, (directoryInfo, areSubDirectoriesEmpty) =>
            {
                var isEmpty = areSubDirectoriesEmpty && DirectoryIsEmpty(directoryInfo);
                if (isEmpty) emptyDirectories.Add(directoryInfo);
                return isEmpty;
            });

            return emptyDirectories;
        }

        private static bool DirectoryIsEmpty(DirectoryInfo dirInfo)
        {
            FileInfo[] files = null;

            try
            {
                files = dirInfo.GetFiles("*.*");
                files = files.Where(x => !x.Name.EndsWith(".meta")).ToArray();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            return files == null || files.Length == 0;
        }


        private static void DeleteEmptyDirectory(DirectoryInfo emptyDirectory)
        {
            var relativePath = GetRelativePath(emptyDirectory.FullName, Directory.GetCurrentDirectory());
            AssetDatabase.MoveAssetToTrash(relativePath);
            Debug.Log("Empty directory removed at: " + emptyDirectory.FullName);
        }

        // return: Is this directory empty?
        private static bool WalkDirectoryTree(DirectoryInfo root, IsEmptyDirectory pred)
        {
            var subDirectories = root.GetDirectories();

            var areSubDirsEmpty = true;
            foreach (var dirInfo in subDirectories)
                if (!WalkDirectoryTree(dirInfo, pred))
                    areSubDirsEmpty = false;

            var isRootEmpty = pred(root, areSubDirsEmpty);
            return isRootEmpty;
        }

        private static string GetRelativePath(string file, string folder)
        {
            var pathUri = new Uri(file);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                folder += Path.DirectorySeparatorChar;

            var folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString()
                .Replace('/', Path.DirectorySeparatorChar));
        }


        // return: Is this directory empty?
        private delegate bool IsEmptyDirectory(DirectoryInfo dirInfo, bool areSubDirectoriesEmpty);
    }
}

#endif