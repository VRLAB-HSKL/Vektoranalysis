using System.Collections;
using System.Collections.Generic;
using Model;
using Model.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationControl : MonoBehaviour
{
    public ScalarFieldManager ScalarFieldManager;
    public PathManager PathManager;
    
    public Image FormulaImage;
    public TextMeshProUGUI NameValueText;
    
    // Start is called before the first frame update
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
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
