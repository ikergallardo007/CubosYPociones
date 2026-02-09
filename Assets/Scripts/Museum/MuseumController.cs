using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class MuseumController : MonoBehaviour
{
    // Struct
    [System.Serializable]
    public struct CubeData
    {
        public string name; // Name of the cube
        public Vector3 position; // Position of the cube in the scene
        public Color color; // Color of the cube

        public CubeData(string name, Vector3 position, Color color)
        {
            this.name = name;
            this.position = position;
            this.color = color;
        }

        public void Save(string filePath)
        {
            string jsonData = JsonUtility.ToJson(this); // Convert the CubeData object to JSON format
            File.WriteAllText(filePath, jsonData); // Write the JSON data to a file
        }
    }

    // Public Attributes
    public CubeObject cubePrefab; // Reference to the cube prefab to be instantiated

    // Private Properties
    private InputAction collectAction; // Input action for collecting cubes
    private int i = 0; // Counter for the x-axis position of the cube
    private int j = 0; // Counter for the z-axis position of the cube
    private int counter = 0; // Counter for the number of cubes collected
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
    [SerializeField]
    private List<CubeObject> collectedCubes = new List<CubeObject>(); // List to keep track of collected cubes

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
            counter++;
            CollectCube(i, j, counter);
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

    private void CollectCube(int i, int j, int counter)
    {
        Vector3 position = new Vector3(i * 2, 0, j * 2); // Example position for the cube
        CubeObject cube = Instantiate(cubePrefab, position, Quaternion.identity); // Instantiate the cube at the specified position with no rotation
        cube.cubeMeshRenderer.material.color = cubeColors[Random.Range(0, cubeColors.Length)]; // Assign a random color to the cube from the predefined array
        cube.name = "Cube_" + counter; // Name the cube with a unique identifier
        collectedCubes.Add(cube); // Add the collected cube to the list
    }

    private void OnDestroy()
    {
        // Clean up the collected cubes when the MuseumController is destroyed
        foreach (CubeObject cube in collectedCubes)
        {
            print("Destroying cube: " + cube.name); // Log the name of the cube being destroyed for debugging purposes
            if (cube != null)
            {
                CubeData cubeData = new CubeData(cube.name, cube.transform.position, cube.cubeMeshRenderer.material.color); // Create a CubeData object with the cube's name, position, and color
                string filePath = Application.dataPath + "/Data/" + cube.name + ".json"; // Define the file path for saving the cube data
                print("Saving cube data to: " + filePath); // Log the file path for debugging purposes
                cubeData.Save(filePath); // Save the cube data to a JSON file
            }
        }
    }
}
