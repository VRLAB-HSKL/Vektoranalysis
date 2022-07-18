using Controller;
using Model;
using Travel;
using UnityEngine;

public class WorldStateController : MonoBehaviour
{
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
        InitializeModel();
    }

    private void InitializeModel()
    {
        // Initialize global model
        GlobalDataModel.InitializeData();
        
        InitializeViewControllers();
    }

    private void InitializeViewControllers()
    {
        GlobalDataModel.FieldViewController = new FieldViewController(FieldMesh, FieldBoundingBox);
    }

    public void NextDataset()
    {
        SetNextValidIndex(true);

        GlobalDataModel.FieldViewController.UpdateViews(); //UpdateViewsDelegate();
        
        InfoWall.UpdateInformation();
        TableMap.SetTexture();
    }

    public void PreviousDataset()
    {
        SetNextValidIndex(false);

        // if (GlobalDataModel.FieldViewController is null)
        // {
        //     Debug.Log("FieldViewController is null");
        // }
        //
        // if (GlobalDataModel.FieldViewController.UpdateViewsDelegate is null)
        // {
        //     Debug.Log("FieldViewController.UpdateViewsDelegate is null");
        // }
        //
        GlobalDataModel.FieldViewController.UpdateViews();
        
        InfoWall.UpdateInformation();
        TableMap.SetTexture();
    }


    private void SetNextValidIndex(bool isIncrement)
    {
        var oldIndex = GlobalDataModel.CurrentFieldIndex;
        if (oldIndex == 0)
        {
            GlobalDataModel.CurrentFieldIndex = GlobalDataModel.ScalarFields.Count - 1;
        }
        else if (oldIndex == GlobalDataModel.ScalarFields.Count - 1)
        {
            GlobalDataModel.CurrentFieldIndex = 0;
        }
        else
        {
            if (isIncrement) 
                ++GlobalDataModel.CurrentFieldIndex;
            else 
                --GlobalDataModel.CurrentFieldIndex;
        }
    }
    
}