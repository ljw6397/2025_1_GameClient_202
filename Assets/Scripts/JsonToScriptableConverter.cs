#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
public class JsonToScriptableConverter : EditorWindow
{

    private string jsonFilePath = "";                                //JSON 파일 경로 문자열 값
    private string outputFolder = "Assets/ScriptableObjects/items";  //출력 SO 파일을 경로 값
    private bool createDatabase = true;                               //데이터 베이스를 사용 할 것인지에 대한 bool 값


    [MenuItem("Tools/JSON to Scriptable Objects")]

    public static void ShowWindow()
    {
        GetWindow<JsonToScriptableConverter>("JSON to Scriptable Objects");
    }


    private void OnGUI()
    {
        GUILayout.Label("JSON to Scriptable Object Converter", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if(GUILayout.Button("Select JSON File"))
        {
            jsonFilePath = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
        }

        EditorGUILayout.LabelField("Selected File : ", jsonFilePath);
        EditorGUILayout.Space();
        outputFolder = EditorGUILayout.TextField("Output Folder : ", outputFolder);
        createDatabase = EditorGUILayout.Toggle("Create Database Asset", createDatabase);
        EditorGUILayout.Space();

        if(GUILayout.Button("Convert to Scriptable Objects"))
        {
            if(string.IsNullOrEmpty(jsonFilePath))
            {
                EditorUtility.DisplayDialog("Error", "Please select a JSON file firest!", "OK");
                return;
            }
            ConvertJsonToScriptableObjects();
        }
    }

    private void ConvertJsonToScriptableObjects()     //JSON 파일을 ScriptableObject 파일로 변환 시켜주는 함수
    {
        //폴더 생성
        if(!Directory.Exists(outputFolder))    //폴더 위치를 확인하고 없으면 생성 한다
        {
            Directory.CreateDirectory(outputFolder);
        }
        //JSON 파일 읽
        string jsonText = File.ReadAllText(jsonFilePath);  //Json 파일을 읽는다.



        try
        {
            List<ItemData> itemDataList = JsonConvert.DeserializeObject<List<ItemData>>(jsonText);

            List<ItemSO> createdItems = new List<ItemSO>();

            foreach (var itemData in itemDataList)
            {

                ItemSO itemSO = ScriptableObject.CreateInstance<ItemSO>();  //ItemSO 파일을 생성

                itemSO.id = itemData.id;
                itemSO.itemName = itemData.itemName;
                itemSO.nameEng = itemData.nameEng;
                itemSO.description = itemData.description;

                //열거혀여 변환
                if(System.Enum.TryParse(itemData.itemTypeString, out ItemType parsedType))
                {
                    itemSO.itemType = parsedType;
                }
                else
                {
                    Debug.LogWarning($"아이템 '{itemData.itemName}'의 유효하지 않은 타입 : {itemData.itemTypeString}");
                }

                itemSO.price = itemData.price;
                itemSO.power = itemData.power;
                itemSO.level = itemData.level;
                itemSO.isStackable = itemData.isStackable;

                //아이콘 로드 (경로가 있는 경우)
                if(!string.IsNullOrEmpty(itemData.iconPath))
                {
                    itemSO.icon = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Resources/{itemData.iconPath}.png");

                    if(itemSO.icon == null)
                    {
                        Debug.LogWarning($"아이템 ' {itemData.nameEng}'의; 아이콘을 찾을 수 없습니다 : {itemData.iconPath}");
                    }
                }
                //스크립터블 오브젝트 저장 - ID를 4자리 숫자로 포맷팅
                string assetPath = $"{outputFolder}/item_{itemData.id.ToString("D4")}_{itemData.nameEng}.asset";
                AssetDatabase.CreateAsset(itemSO, assetPath);

                //에셋 이름 저장
                itemSO.name = $"Item_{itemData.id.ToString("D4")}+{itemData.nameEng}";
                createdItems.Add(itemSO);
                EditorUtility.SetDirty(itemSO);

            }

            //데이터베이스 생성
            if(createDatabase && createdItems.Count > 0)
            {
                ItemDatabaseSO database = ScriptableObject.CreateInstance<ItemDatabaseSO>();
                database.items = createdItems;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/ItemDatabase.asset");
                EditorUtility.SetDirty(database);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Success", $"Created {createdItems.Count} scriptable objects!", "OK");
            
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Convert JSON: { e.Message}","OK");
                Debug.LogError($"JSON 변환 오류: {e}");
        }
    }

    
    
}

#endif
