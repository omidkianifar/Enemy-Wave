using System;
using UnityEditor;
using UnityEngine;

public class GuidelinesPopup : EditorWindow
{
    private const string ShowPopupPrefKey = "ShowGuidelinesPopup";
    private const string ShowSessionPrefKey = "ShowInSessionGuidelinesPopup";
    private bool dontShowAgain = false;

    private const string HeaderImagePath = "Assets/Project Guidelines/Docs/HeaderImage.png";

    [MenuItem("Documentation/Project Guidelines/Guidelines Summary")]
    public static void ShowWindow()
    {
        GuidelinesPopup window = GetWindow<GuidelinesPopup>("Guidelines Summary");
        window.Show();
    }

    private void OnGUI()
    {
        // Load header image
        Texture2D headerImage = AssetDatabase.LoadAssetAtPath<Texture2D>(HeaderImagePath);
        if (headerImage != null)
        {
            GUILayout.Label(headerImage, GUILayout.Width(position.width), GUILayout.Height(100));
        }
        else
        {
            GUILayout.Label("Project Guidelines Summary", EditorStyles.boldLabel);
        }

        EditorGUILayout.Space();

        // Add the summary of your guidelines here
        GUILayout.Label("• Follow the Model-View-Controller (MVC) pattern.", EditorStyles.wordWrappedLabel);
        GUILayout.Label("• Use VContainer for dependency injection.", EditorStyles.wordWrappedLabel);
        GUILayout.Label("• Separate code into appropriate folders (e.g., Models, Views, Controllers).", EditorStyles.wordWrappedLabel);
        GUILayout.Label("• Avoid using Unity's Instantiate for objects in the DI graph.", EditorStyles.wordWrappedLabel);
        GUILayout.Label("• Use Interfaces to decouple components.", EditorStyles.wordWrappedLabel);
        GUILayout.Label("• Adhere to naming conventions and best practices.", EditorStyles.wordWrappedLabel);

        EditorGUILayout.Space();

        // Buttons to open HTML files
        if (GUILayout.Button("Open Code Structure", GUILayout.Height(30)))
        {
            DocumentationMenu.OpenCodeStructure();
        }

        if (GUILayout.Button("Open Folder Structure", GUILayout.Height(30)))
        {
            DocumentationMenu.OpenFolderStructure();
        }

        if (GUILayout.Button("Open Github Flow", GUILayout.Height(30)))
        {
            DocumentationMenu.OpenGitHubFlow();
        }

        EditorGUILayout.Space();

        // Don't show again checkbox
        dontShowAgain = EditorGUILayout.Toggle("Don't show again", dontShowAgain);
        if (dontShowAgain)
        {
            EditorPrefs.SetBool(ShowPopupPrefKey, !dontShowAgain);
        }
    }

    [InitializeOnLoadMethod]
    private static void InitializeOnLoad()
    {
        EditorApplication.delayCall += ShowDelayedPopup;
        EditorApplication.wantsToQuit += HandleQuit;
    }

    private static void ShowDelayedPopup()
    {
        if (EditorPrefs.HasKey(ShowPopupPrefKey) && !EditorPrefs.GetBool(ShowPopupPrefKey, true))
        {
            return;
        }

        if (EditorPrefs.HasKey(ShowSessionPrefKey) && EditorPrefs.GetBool(ShowSessionPrefKey))
        {
            return;
        }

        EditorPrefs.SetBool(ShowSessionPrefKey, true);

        ShowWindow();
    }

    private static bool HandleQuit()
    {
        EditorPrefs.DeleteKey(ShowSessionPrefKey);  

        return true;
    }
}
