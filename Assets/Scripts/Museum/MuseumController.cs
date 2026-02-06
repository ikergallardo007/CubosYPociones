using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MuseumController : MonoBehaviour
{
    // Public Attributes
    public CubeObject cubePrefab; // Reference to the cube prefab to be instantiated

    // Private Properties
    private InputAction collectAction; // Input action for collecting cubes
    private int i = 0; // Counter for the x-axis position of the cube
    private int j = 0; // Counter for the z-axis position of the cube
    private Color[] cubeColors = new Color[] {
                Color.black,
                Color.blue,
                Color.brown,
                Color.gray,
                Color.green,
                Color.orange,
                Color.pink,
                Color.purple,
                Color.red,
                Color.white,
                Color.yellow
    };

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
        CubeObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
        cube.cubeMeshRenderer.material.color = cubeColors[Random.Range(0, cubeColors.Length)];
    }
}
