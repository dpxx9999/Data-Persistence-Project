using UnityEngine;
using UnityEngine.UI; // 버튼 관련 using
using System.Collections.Generic;

public class MainManager : MonoBehaviour
{
    public MeshRenderer cubeMesh;
    public Button[] colorButtons; // 버튼 배열

    private Dictionary<string, Color> colorDictionary = new Dictionary<string, Color>();

    // 색상 이름 배열
    private string[] colorNames = new string[] { "Yellow", "Green", "Red" };

    void Start()
    {
        // JSON 파일에서 색상 데이터 불러오기
        LoadColorData();

        // 버튼 클릭 이벤트 연결
        for (int i = 0; i < colorButtons.Length; i++)
        {
            int index = i; // 이벤트에서 인덱스를 로컬로 저장
            colorButtons[i].onClick.AddListener(() => ChangeColor(index));
        }

        // 저장된 색상 불러오기
        LoadSavedColor();
    }

    void LoadColorData()
    {
        // JSON 파일에서 색상 데이터를 읽음
        TextAsset colorDataJson = Resources.Load<TextAsset>("ColorData"); // Resources 폴더 안에 ColorData.json 파일이 있어야 함
        if (colorDataJson != null)
        {
            ColorData colorData = JsonUtility.FromJson<ColorData>(colorDataJson.text);

            foreach (var colorItem in colorData.colors)
            {
                // Hex 값을 Color로 변환하여 Dictionary에 추가
                Color color;
                if (ColorUtility.TryParseHtmlString(colorItem.value, out color))
                {
                    colorDictionary.Add(colorItem.name, color);
                }
                else
                {
                    Debug.LogWarning($"Invalid color code: {colorItem.value} for {colorItem.name}");
                }
            }
        }
        else
        {
            Debug.LogError("ColorData.json not found in Resources folder.");
        }
    }

    void ChangeColor(int index)
    {
        // 버튼을 통해 선택된 색상 이름을 가져와서 색상 적용
        string colorName = colorNames[index]; // colorNames 배열은 색상 이름을 저장
        if (colorDictionary.ContainsKey(colorName))
        {
            cubeMesh.material.color = colorDictionary[colorName];
            SaveColor(colorName);
        }
    }

    void SaveColor(string colorName)
    {
        PlayerPrefs.SetString("SelectedColorName", colorName);
        PlayerPrefs.Save();
    }

    void LoadSavedColor()
    {
        if (PlayerPrefs.HasKey("SelectedColorName"))
        {
            string savedColorName = PlayerPrefs.GetString("SelectedColorName");
            if (colorDictionary.ContainsKey(savedColorName))
            {
                cubeMesh.material.color = colorDictionary[savedColorName];
            }
        }
    }
}

[System.Serializable]
public class ColorData
{
    public ColorItem[] colors; // 색상 데이터를 담을 배열
}

[System.Serializable]
public class ColorItem
{
    public string name;  // 색상 이름
    public string value; // 색상 값 (Hex 코드)
}
