using UnityEngine;
using UnityEngine.InputSystem;

public class MuseumController : MonoBehaviour
{
    // Public Attributes
    public CubeObject cubePrefab;

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
        // Space button pressed.
        bool collectPress = collectAction.WasPressedThisFrame();
        if (collectPress)
        {
            print("Collect Button Pressed");
        }
    }

    void CollectCube(Vector3 position)
    {
        // Instantiation of cube prefab.
        Instantiate(cubePrefab, position, Quaternion.identity);
    }
}
