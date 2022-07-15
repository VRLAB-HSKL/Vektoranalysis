using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationControl : MonoBehaviour
{
    public Image FormulaImage;
    public TextMeshProUGUI NameValueText;
    
    // Start is called before the first frame update
    void Start()
    {
        var initFieldId = GlobalDataModel.CurrentField.ID;
        NameValueText.text = initFieldId;
        
        var path = GlobalDataModel.FormulaImageResourcePath + initFieldId;
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
