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
    
    /// <summary>
    /// Single awake in application to ensure init file was parsed
    /// ToDo: Alternatively, just place it at the front of the script execution order in unity editor settings
    /// </summary>
    private void Awake()
    {
        //Debug.Log("count: " + ScalarFieldManager.ScalarFields.Count);
        InitializeModel();
    }

    private void InitializeModel()
    {
        // Initialize global model
        ScalarFieldManager.ParseInitFile();
        
        InitializeViewControllers();
    }

    private void InitializeViewControllers()
    {
        ViewControllerManager.FieldViewController = 
            new FieldViewController(ScalarFieldManager, FieldMesh, FieldBoundingBox);
    }

    public void NextDataset()
    {
        SetNextValidIndex(true);

        ViewControllerManager.FieldViewController.UpdateViews(); //UpdateViewsDelegate();
        
        InfoWall.UpdateInformation();
        TableMap.SetTexture();
    }

    public void PreviousDataset()
    {
        SetNextValidIndex(false);

        // if (ScalarFieldManager.FieldViewController is null)
        // {
        //     Debug.Log("FieldViewController is null");
        // }
        //
        // if (ScalarFieldManager.FieldViewController.UpdateViewsDelegate is null)
        // {
        //     Debug.Log("FieldViewController.UpdateViewsDelegate is null");
        // }
        //
        ViewControllerManager.FieldViewController.UpdateViews();
        
        InfoWall.UpdateInformation();
        TableMap.SetTexture();
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