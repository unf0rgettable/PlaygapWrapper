#if UNITY_EDITOR && UNITY_IOS
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace Playgap.Editor
{
    public static class PostBuildiOS
    {
        private const string PLAYGAP_SKADNETWORK_ID = "wbmgtnm2cz.skadnetwork";

        [PostProcessBuild]
        public static void SetupPlaygapFramework(BuildTarget buildTarget, string buildPath)
        {
            if (buildTarget != BuildTarget.iOS)
            {
                return;
            }

            var projectPath = PBXProject.GetPBXProjectPath(buildPath);

            var project = new PBXProject();
            project.ReadFromFile(projectPath);

            string unityTargetGuid = project.GetUnityFrameworkTargetGuid();

            SetupUnityFrameworkModulemap(project, unityTargetGuid, buildPath);
            AddSkAdNetworkIdentifier(buildPath);

            project.WriteToFile(projectPath);
        }

        private static void SetupUnityFrameworkModulemap(PBXProject project, string targetGiud, string buildPath)
        {
            // Modulemap
            project.AddBuildProperty(targetGiud, "DEFINES_MODULE", "YES");

            var moduleFile = buildPath + "/UnityFramework/UnityFramework.modulemap";
            if (!File.Exists(moduleFile))
            {
                FileUtil.CopyFileOrDirectory("Assets/Playgap/Plugins/iOS/UnityFramework.modulemap", moduleFile);
                project.AddFile(moduleFile, "UnityFramework/UnityFramework.modulemap");
                project.AddBuildProperty(targetGiud, "MODULEMAP_FILE", "$(SRCROOT)/UnityFramework/UnityFramework.modulemap");
            }

            // Headers
            string unityInterfaceGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnityInterface.h");
            project.AddPublicHeaderToBuild(targetGiud, unityInterfaceGuid);

            string unityForwardDeclsGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnityForwardDecls.h");
            project.AddPublicHeaderToBuild(targetGiud, unityForwardDeclsGuid);

            string unityRenderingGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnityRendering.h");
            project.AddPublicHeaderToBuild(targetGiud, unityRenderingGuid);

            string unitySharedDeclsGuid = project.FindFileGuidByProjectPath("Classes/Unity/UnitySharedDecls.h");
            project.AddPublicHeaderToBuild(targetGiud, unitySharedDeclsGuid);
        }

        private static void AddSkAdNetworkIdentifier(string buildPath)
        {
            string plistPath = Path.Combine(buildPath, "Info.plist");
            string plistContent = File.ReadAllText(plistPath);
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(plistContent);

            // Get root
            PlistElementDict rootDict = plist.root;

            // Check if SKAdNetworkItems already exists
            PlistElementArray skAdNetworkItems = null;
            if (rootDict.values.ContainsKey("SKAdNetworkItems"))
            {
                try
                {
                    skAdNetworkItems = rootDict.values["SKAdNetworkItems"] as PlistElementArray;
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Could not obtain SKAdNetworkItems PlistElementArray: {e.Message}");
                }
            }

            // If not exists, create it
            if (skAdNetworkItems == null)
            {
                skAdNetworkItems = rootDict.CreateArray("SKAdNetworkItems");
            }

            // Add SKAdNetwork ID
            if (!plistContent.Contains(PLAYGAP_SKADNETWORK_ID))
            {
                PlistElementDict skAdNetworkIdentifierDict = skAdNetworkItems.AddDict();
                skAdNetworkIdentifierDict.SetString("SKAdNetworkIdentifier", PLAYGAP_SKADNETWORK_ID);
            }

            File.WriteAllText(plistPath, plist.WriteToString());
        }

    }
}
#endif