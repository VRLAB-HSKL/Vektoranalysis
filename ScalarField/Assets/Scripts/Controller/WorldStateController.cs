using Controller;
using Model;
using UnityEngine;

public class WorldStateController : MonoBehaviour
{
    public GameObject FieldMesh;
    public GameObject FieldBoundingBox;
    
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
    
}