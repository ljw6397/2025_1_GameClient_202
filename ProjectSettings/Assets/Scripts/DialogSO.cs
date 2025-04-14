using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Dialog", menuName = "Dialog System/Dialog")]

public class DialogSO : ScriptableObject
{
    public int id;
    public string characterName;
    public string text;
    public int nextld;
    public List<DialogChoiceSO> choices = new List<DialogChoiceSO>();
    public Sprite portrait;

    [Tooltip("초상화 리소스 경로 (Resources 폴더 내의 경로)")]
    public string portraitPath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
