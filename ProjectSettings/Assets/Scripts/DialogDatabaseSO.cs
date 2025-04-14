using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogDatebase", menuName = "Dialog System/Datebase")]
public class DialogDatabaseSO : ScriptableObject
{
    public List<DialogSO>dialogs = new List<DialogSO>();
    private Dictionary<int, DialogSO> dialogsById;

    public void Initailize()
    {
        dialogsById = new Dictionary<int, DialogSO>();

        foreach (var dialog in dialogs)
        {
            if (dialog != null)
            {
                dialogsById[dialog.id] = dialog;
            }
        }
    }
    public DialogSO GetDialogById(int id)
    {
        if(dialogsById == null)
            Initailize();
        if(dialogsById.TryGetValue(id, out DialogSO dialog))
            return dialog;

        return null;
    }

    internal DialogSO GetDialogById(int? nextld)
    {
        throw new NotImplementedException();
    }
}
