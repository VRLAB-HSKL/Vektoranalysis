using UnityEngine;

public class WorldStateController : MonoBehaviour
{
    /// <summary>
    /// Single awake in application to ensure init file was parsed
    /// </summary>
    private void Awake()
    {
        InitializeModel();
    }

    private void InitializeModel()
    {
        // Initialize global model
        GlobalDataModel.InitializeData();
    }
}