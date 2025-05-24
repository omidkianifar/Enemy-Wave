using UnityEditor;
using UnityEngine;

public class DocumentationMenu
{
    // Path to the HTML files in the Assets folder
    public const string CodeStructurePath = "Assets/Project Guidelines/Docs/CodeStructure.html";
    public const string FolderStructurePath = "Assets/Project Guidelines/Docs/FolderStructure.html";
    public const string GitHubFlowPath = "Assets/Project Guidelines/Docs/GitHubFlow.html";

    [MenuItem("Documentation/Project Guidelines/Code Structure")]
    public static void OpenCodeStructure()
    {
        OpenDocumentation(CodeStructurePath);
    }

    [MenuItem("Documentation/Project Guidelines/Folder Structure")]
    public static void OpenFolderStructure()
    {
        OpenDocumentation(FolderStructurePath);
    }

    [MenuItem("Documentation/Project Guidelines/GitHub Flow")]
    public static void OpenGitHubFlow()
    {
        OpenDocumentation(GitHubFlowPath);
    }

    private static void OpenDocumentation(string filePath)
    {
        if (System.IO.File.Exists(filePath))
        {
            Application.OpenURL("file://" + System.IO.Path.GetFullPath(filePath));
        }
        else
        {
            Debug.LogError("Documentation file not found: " + filePath);
        }
    }
}