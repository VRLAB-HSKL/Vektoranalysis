using Controller;
using Model;
using Model.ScriptableObjects;
using Travel;
using UnityEngine;

public class WorldStateController : MonoBehaviour
{
    public ViewControllerManager ViewControllerManager;
    public ScalarFieldManager ScalarFieldManager;
    
    public GameObject FieldMesh;
    public GameObject FieldBoundingBox;
    
    public MapPlacement TableMap;
    public InformationControl InfoWall;
    public CreateColorScale ColorScale;
    
    /// <summary>
    /// Single awake in application to ensure init file was parsed
    /// ToDo: Alternatively, just place it at the front of the script execution order in unity editor settings
    /// </summary>
    private void Awake()
    {
        InitializeModel();
    }

    private void InitializeModel()
    {
        // Parse scalar fields data
        ScalarFieldManager.ParseInitFile();
        
        InitializeViewControllers();
    }

    private void InitializeViewControllers()
    {
        ViewControllerManager.FieldViewController = 
            new FieldViewController(ScalarFieldManager, FieldMesh, FieldBoundingBox);
    }

    private void UpdateRoom()
    {
        InfoWall.UpdateInformation();
        TableMap.SetTexture();
        ColorScale.UpdateScale();
    }

    public void NextDataset()
    {
        SetNextValidIndex(true);

        ViewControllerManager.FieldViewController.UpdateViews(); //UpdateViewsDelegate();
        
        UpdateRoom();
    }

    public void PreviousDataset()
    {
        SetNextValidIndex(false);

        ViewControllerManager.FieldViewController.UpdateViews();
        
        UpdateRoom();
    }


    private void SetNextValidIndex(bool isIncrement)
    {
        var oldIndex = ScalarFieldManager.CurrentFieldIndex;
        if (oldIndex == 0)
        {
            ScalarFieldManager.CurrentFieldIndex = ScalarFieldManager.ScalarFields.Count - 1;
        }
        else if (oldIndex == ScalarFieldManager.ScalarFields.Count - 1)
        {
            ScalarFieldManager.CurrentFieldIndex = 0;
        }
        else
        {
            if (isIncrement) 
                ++ScalarFieldManager.CurrentFieldIndex;
            else 
                --ScalarFieldManager.CurrentFieldIndex;
        }
    }
    
}