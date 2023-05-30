#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace VG.Editor.Features
{
    public class SnapSceneCamToGameCamFeature
    {
        [MenuItem("Tools/VG/Snap SceneCam To GameCam")]
        public static void SnapSceneCamToGameCam()
        {
            var sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                var sceneCam = sceneView.camera;
                var gameCam = Camera.main;

                if (sceneCam != null && gameCam != null)
                {
                    sceneView.AlignViewToObject(gameCam.transform);
                    sceneView.orthographic = gameCam.orthographic;

                    // sceneCam.CopyFrom (gameCam);

                    // Making extra sure cam is the same...
                    // sceneCam.fieldOfView = gameCam.fieldOfView;
                    // sceneCam.nearClipPlane = gameCam.nearClipPlane;
                    // sceneCam.farClipPlane = gameCam.farClipPlane;
                    // sceneCam.aspect = gameCam.aspect;
                    // sceneCam.orthographic = gameCam.orthographic;
                    // sceneCam.depth = gameCam.depth;
                    // sceneCam.pixelRect = gameCam.pixelRect;
                    // sceneCam.cameraType = gameCam.cameraType;
                    // sceneCam.projectionMatrix = gameCam.projectionMatrix;
                    // sceneCam.pro
                    // sceneView.Repaint ();
                }
            }
        }
    }
}

#endif