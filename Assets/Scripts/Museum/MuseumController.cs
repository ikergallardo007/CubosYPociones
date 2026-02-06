using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MuseumController : MonoBehaviour
{
    // Public Attributes
    public CubeObject cubePrefab;

    // Private Properties
    private InputAction collectAction;
    private int i = 0;
    private int j = 0;

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
            CollectCube(i, j);
            j++;
            if (j == 16)
            {
                j = 0;
                i++;
                if (i == 11)
                {
                    i = 0;
                }
            }
        }
    }

    private void CollectCube(int i, int j)
    {
        Vector3 position = new Vector3(i * 2, 0, j * 2); // Example position for the cube
        Instantiate(cubePrefab, position, Quaternion.identity);
    }
}
