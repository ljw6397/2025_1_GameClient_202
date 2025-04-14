using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public class ItemDataLoader : MonoBehaviour
{
    [SerializeField]
    private string jsonFileName = "Items";

    private List<ItemData> itemList;

    private string EncodeKorean(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        byte[] bytes = Encoding.Default.GetBytes(text);
        return Encoding .UTF8.GetString(bytes);

    }
    void Start()
    {
        LoaditemData();
    }

    void LoaditemData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        if (jsonFile != null)
        {
            byte[] bytes = Encoding.Default.GetBytes (jsonFile.text);
            string correntText = Encoding .UTF8.GetString (bytes);

            itemList = JsonConvert.DeserializeObject<List<ItemData>>(correntText);

            Debug.Log($"�ε�� ������ �� : {itemList.Count}");

            foreach(var item in itemList)
            {
                Debug.Log($"������ : {EncodeKorean(item.itemName)}, ���� : {EncodeKorean(item.description)}");
                
            }
        }
        else
        {
            Debug.LogError($"JSON ������ ã�� �� �����ϴ�. : {jsonFileName}");
        }
    }
    // Start is called before the first frame update
    

    
}
