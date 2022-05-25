using UnityEngine;

public class WorldStateController : MonoBehaviour
{
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