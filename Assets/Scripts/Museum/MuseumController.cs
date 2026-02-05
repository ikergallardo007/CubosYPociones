using System.Collections;
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
        StartCoroutine(CollectCube());
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

    IEnumerator CollectCube()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                while (!collectAction.WasPressedThisFrame())
                {
                    yield return null; // Wait until the collect button is pressed
                }
                Vector3 position = new Vector3(i * 2, 0, j * 2); // Example position for the cube
                Instantiate(cubePrefab, position, Quaternion.identity);
            }
        }
    }
}
