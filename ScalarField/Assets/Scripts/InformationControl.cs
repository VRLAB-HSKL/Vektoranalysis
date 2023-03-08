using Model.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationControl : MonoBehaviour
{
    [Header("Data")]
    public ScalarFieldManager ScalarFieldManager;
    public PathManager PathManager;
    
    [Header("Dependencies")]
    public Image FormulaImage;
    public TextMeshProUGUI NameValueText;
    
    void Start()
    {
        UpdateInformation();
    }

    public void UpdateInformation()
    {
        var initFieldId = ScalarFieldManager.CurrentField.ID;
        NameValueText.text = initFieldId;
        
        var path = PathManager.FormulaImageResourcePath + initFieldId;
        var formulaSprite = Resources.Load<Sprite>(path);
        if (formulaSprite != null)
        {
            FormulaImage.sprite = formulaSprite;
        }
    }
}
