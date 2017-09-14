using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class DialogData : ScriptableObject {


    public List<Dialog> dialogSystem;


    public Dialog GetDialogByKey(string key) {

        return this.dialogSystem.Where<Dialog>(d => d.key == key).First();
       
    }
}

public class CreateDialogData
{
    [MenuItem("Assets/Create/Inventory Item List")]
    public static DialogData Create()
    {
        DialogData asset = ScriptableObject.CreateInstance<DialogData>();

        AssetDatabase.CreateAsset(asset, "Assets/LaureusDialogSystem/DialogData.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
