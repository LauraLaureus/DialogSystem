using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogGUI : EditorWindow {

    public DialogData dialogRepo;

    private int viewIndex = 1;
    private Vector2 scrollView;
    

    [MenuItem("Window/Dialog Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(DialogGUI));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            dialogRepo = AssetDatabase.LoadAssetAtPath(objectPath, typeof(DialogData)) as DialogData;
        }

    }

    void OnGUI()
    {

        scrollView = EditorGUILayout.BeginScrollView(scrollView);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Dialog Editor", EditorStyles.boldLabel);
        if (dialogRepo != null)
        {
            if (GUILayout.Button("Show Dialog List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = dialogRepo;
            }
        }
        if (GUILayout.Button("Open Dialog List"))
        {
            OpenDialogRepo();
        }
        
        GUILayout.EndHorizontal();

        if (dialogRepo == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Dialog List", GUILayout.ExpandWidth(false)))
            {
                CreateNewRepo();
            }
            if (GUILayout.Button("Open Existing Dialog List", GUILayout.ExpandWidth(false)))
            {
                OpenDialogRepo();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        if (dialogRepo != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < dialogRepo.dialogSystem.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(120);

            if (GUILayout.Button("Add Dialog", GUILayout.ExpandWidth(false)))
            {
                AddItem();
            }
            if (GUILayout.Button("Delete Dialog", GUILayout.ExpandWidth(false)))
            {
                DeleteItem(viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (dialogRepo.dialogSystem == null)
                Debug.Log("wtf");
            if (dialogRepo.dialogSystem.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Dialog", viewIndex, GUILayout.ExpandWidth(false)), 1, dialogRepo.dialogSystem.Count);
                
                EditorGUILayout.LabelField("of   " + dialogRepo.dialogSystem.Count.ToString() + "  dialog ", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                dialogRepo.dialogSystem[viewIndex - 1].key = EditorGUILayout.TextField("Dialog Key", dialogRepo.dialogSystem[viewIndex - 1].key);
                
                GUILayout.Label("Dialog", EditorStyles.boldLabel);
                dialogRepo.dialogSystem[viewIndex - 1].dialog = EditorGUILayout.TextArea(dialogRepo.dialogSystem[viewIndex - 1].dialog, GUILayout.Height(120));
                GUILayout.Space(10);
                

                dialogRepo.dialogSystem[viewIndex - 1].character = EditorGUILayout.TextField("Character", dialogRepo.dialogSystem[viewIndex - 1].character);
                dialogRepo.dialogSystem[viewIndex - 1].details_prevKey = EditorGUILayout.TextField("Previous dialog key", dialogRepo.dialogSystem[viewIndex - 1].details_prevKey);
                dialogRepo.dialogSystem[viewIndex - 1].details_nextKey = EditorGUILayout.TextField("Next dialog Key", dialogRepo.dialogSystem[viewIndex - 1].details_nextKey);

                GUILayout.Label("Notes", EditorStyles.boldLabel);
                dialogRepo.dialogSystem[viewIndex - 1].notes = EditorGUILayout.TextArea(dialogRepo.dialogSystem[viewIndex - 1].notes, GUILayout.Height(120));
                

                GUILayout.Space(10);
                
            }
            else
            {
                GUILayout.Label("This Dialog List is Empty.");
            }

            EditorGUILayout.EndScrollView();
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(dialogRepo);
        }
    }

    void CreateNewRepo()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 1;
        dialogRepo = CreateDialogData.Create();
        if (dialogRepo)
        {
            dialogRepo.dialogSystem = new List<Dialog>();
            string relPath = AssetDatabase.GetAssetPath(dialogRepo);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenDialogRepo()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Dialog Item List", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            dialogRepo = AssetDatabase.LoadAssetAtPath(relPath, typeof(DialogData)) as DialogData;
            if (dialogRepo.dialogSystem == null)
                dialogRepo.dialogSystem = new List<Dialog>();
            if (dialogRepo)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddItem()
    {
        Dialog newItem = new Dialog();
        newItem.key = "New Key";
        dialogRepo.dialogSystem.Add(newItem);
        viewIndex = dialogRepo.dialogSystem.Count;
    }

    void DeleteItem(int index)
    {
        dialogRepo.dialogSystem.RemoveAt(index);
    }
}


