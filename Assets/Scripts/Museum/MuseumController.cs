using UnityEngine;
using UnityEngine.InputSystem;

public class MuseumController : MonoBehaviour
{
    // Public Attributes
    public GameObject cubePrefab;

    // Private Properties
    private InputAction collectAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Get the collect action from the Input System
        collectAction = InputSystem.actions.FindAction("Collect");
    }

    // Update is called once per frame
    private void Update()
    {
        bool collectPress = collectAction.WasPressedThisFrame();
        if (collectPress)
        {
            print("Collect Button Pressed");
        }
    }

    void CollectCube(Vector3 position)
    {
        Instantiate(cubePrefab, position, Quaternion.identity);
    }
}
