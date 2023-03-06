using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#endif

public class FyberPostProcessBuild : MonoBehaviour
{
    #if UNITY_IOS
    [PostProcessBuild(101)]
    private static void OnPostProcessBuildPlayer(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.iOS)
        {
            return;
        }

        UnityEngine.Debug.Log("OfferWall: started post-build script");

        XcodeProject xcodeProject = new XcodeProject(pathToBuiltProject);
        FyberPostProcessBuild.UpdateProjectSettings(xcodeProject);
        xcodeProject.Save();

        UnityEngine.Debug.Log("OfferWall: finished post-build script");
    }

    private static void UpdateProjectSettings(XcodeProject xcodeProject)
    {
        xcodeProject.AddBuildProperty("OTHER_LDFLAGS", "-ObjC");
        xcodeProject.SetBuildProperty("CLANG_ENABLE_MODULES", "YES");
        xcodeProject.SetBuildProperty("VALIDATE_WORKSPACE", "YES");
    }
    #endif
}

class XcodeProject
{
    #if UNITY_IOS
    private PBXProject pbxProject;
    private string xcodeProjectPath;
    private string mainTargetGUID;
    private string unityFrameworkGUID;

    public XcodeProject(string projectPath)
    {
        xcodeProjectPath = projectPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
        Open();
    }

    private void Open()
    {
        pbxProject = new PBXProject();
        pbxProject.ReadFromFile(xcodeProjectPath);
        mainTargetGUID = pbxProject.GetUnityMainTargetGuid();
        unityFrameworkGUID = pbxProject.GetUnityFrameworkTargetGuid();
    }

    public void Save()
    {
        pbxProject.WriteToFile(xcodeProjectPath);
    }

    public void AddBuildProperty(string name, string value)
    {
        pbxProject.AddBuildProperty(mainTargetGUID, name, value);
        pbxProject.AddBuildProperty(unityFrameworkGUID, name, value);
    }

    public void SetBuildProperty(string name, string value)
    {
        pbxProject.SetBuildProperty(mainTargetGUID, name, value);
        pbxProject.SetBuildProperty(unityFrameworkGUID, name, value);
    }

    public void EmbedFramework(string name, string path)
    {
        string framework = Path.Combine(path, name);
        string fileGuid = pbxProject.FindFileGuidByProjectPath(framework);
        pbxProject.AddFileToEmbedFrameworks(mainTargetGUID, fileGuid);
    }

    public void InsertShellScriptBuildPhase(string name, string shellScript, int index)
    {
        string shellPath = "/bin/sh";
        pbxProject.InsertShellScriptBuildPhase(index, mainTargetGUID, name, shellPath, shellScript);
    }
    #endif
}