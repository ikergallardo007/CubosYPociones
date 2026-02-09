using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class MuseumController : MonoBehaviour
{
    // Struct with the necessary information to save about each cube, including its name, position, and color. This struct will be used to create a list of cubes that can be saved to a file.
    [System.Serializable]
    public struct CubeData
    {
        public string name; // Name of the cube
        public Vector3 position; // Position of the cube in the scene
        public Color color; // Color of the cube

        // Constructor to initialize the CubeData struct 
        public CubeData(string name, Vector3 position, Color color)
        {
            this.name = name;
            this.position = position;
            this.color = color;
        }
    }

    // Struct to hold a list of CubeData objects, which can be saved to a file. This struct will allow us to easily manage and save the information of multiple cubes at once.
    [System.Serializable]
    public struct CubeList 
    {
        public List<CubeData> cubes; // List of CubeData objects

        public CubeList(List<CubeData> cubes)
        {
            this.cubes = cubes;
        }

        public void Save(string filePath)
        {
            File.WriteAllText(filePath, string.Empty); // Clear the file before writing new data
            foreach (CubeData cube in cubes)
            {
                string cubeInfo = $"Name: {cube.name}, Position: {cube.position}, Color: {cube.color}";
                File.AppendAllText(filePath, cubeInfo + "\n");
            }
        }
    }

    // Public Attributes
    public CubeObject cubePrefab; // Reference to the cube prefab to be instantiated
    public CubeData cubeData; // Instance of CubeData to hold the information of a single cube
    public CubeList cubeList; // Instance of CubeList to hold a list of CubeData objects for saving multiple cubes

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
        cubeData = new CubeData(cube.name, cube.transform.position, cube.cubeMeshRenderer.material.color); // Create a CubeData instance with the cube's information
        cubeList.cubes.Add(cubeData); // Add the CubeData instance to the CubeList
    }

    private void OnDestroy()
    {
        cubeList.Save(Application.dataPath + "/Data/cube_data.txt"); // Save the cube data to a file when the MuseumController is destroyed
    }

}
