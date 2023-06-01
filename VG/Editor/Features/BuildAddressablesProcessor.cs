/// <summary>
/// The script gives you choice to whether to build addressable bundles when clicking the build button.
/// For custom build script, call PreExport method yourself.
/// For cloud build, put BuildAddressablesProcessor.PreExport as PreExport command.
/// Discussion: https://forum.unity.com/threads/how-to-trigger-build-player-content-when-build-unity-project.689602/
///
/// License: The MIT License https://opensource.org/licenses/MIT
/// </summary>

using System;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace VG.Editor.Features
{
    internal class BuildAddressablesProcessor
    {
        public static string customBuildPath = "";

        /// <summary>
        ///     Run a clean build before export.
        /// </summary>
        public static void PreExport()
        {
            Debug.Log("BuildAddressablesProcessor.PreExport start");
            AddressableAssetSettings.CleanPlayerContent(
                AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
            AddressableAssetSettings.BuildPlayerContent();
            Debug.Log("BuildAddressablesProcessor.PreExport done");
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
        }

        private static void BuildPlayerHandler(BuildPlayerOptions options)
        {
            if (EditorUtility.DisplayDialog("Build with Addressables",
                    "Do you want to build a clean addressables before export?",
                    "Build with Addressables", "Skip"))
                PreExport();

            // BuildToPath(options);

            BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
        }

        private static void BuildToPath(BuildPlayerOptions options)
        {
            var productName = PlayerSettings.productName;

            var devOrProd = EditorUserBuildSettings.development ? "[DEV]" : "[PROD]";


            var firstPartOfBuildPath =
                string.IsNullOrWhiteSpace(customBuildPath) ? Application.dataPath : customBuildPath;

            var buildPath = string.Empty;
            switch (options.target)
            {
                case BuildTarget.Android when
                    options.locationPathName.EndsWith(".apk", StringComparison.InvariantCultureIgnoreCase):
                    buildPath = Path.Combine(firstPartOfBuildPath,
                        $"../Builds/{EditorUserBuildSettings.activeBuildTarget}/{PlayerSettings.productName}{devOrProd}[{DateTime.Now:dd-MM-yy HH-mm}]");
                    buildPath = Path.GetFullPath(buildPath);

                    buildPath += ".apk";
                    break;
                case BuildTarget.StandaloneWindows or BuildTarget.StandaloneWindows64 when
                    options.locationPathName.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase):
                    buildPath = Path.Combine(firstPartOfBuildPath,
                        $"../Builds/{EditorUserBuildSettings.activeBuildTarget}/{PlayerSettings.productName}{devOrProd}[{DateTime.Now:dd-MM-yy HH-mm}]/{PlayerSettings.productName}");
                    buildPath = Path.GetFullPath(buildPath);

                    buildPath += ".exe";
                    break;
                case BuildTarget.StandaloneOSX:
                    buildPath = Path.Combine(firstPartOfBuildPath,
                        $"../Builds/{EditorUserBuildSettings.activeBuildTarget}/{PlayerSettings.productName}{devOrProd}[{DateTime.Now:dd-MM-yy HH-mm}]/{PlayerSettings.productName}");
                    buildPath = Path.GetFullPath(buildPath);

                    break;
                case BuildTarget.iOS:
                    buildPath = Path.Combine(Application.dataPath,
                        $"../Builds/{EditorUserBuildSettings.activeBuildTarget}/{PlayerSettings.productName}{devOrProd}[{DateTime.Now:dd-MM-yy HH-mm}]");
                    buildPath = Path.GetFullPath(buildPath);

                    break;
                default:
                    buildPath = Path.Combine(firstPartOfBuildPath,
                        $"../Builds/{EditorUserBuildSettings.activeBuildTarget}/{PlayerSettings.productName}{devOrProd}[{DateTime.Now:dd-MM-yy HH-mm}]");
                    buildPath = Path.GetFullPath(buildPath);

                    break;
            }

            options.locationPathName = buildPath;
        }
    }
}

