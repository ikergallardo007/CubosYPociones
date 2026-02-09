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

    public struct IndexData
    {
        public int i; // Index for the x-axis position of the cube
        public int j; // Index for the z-axis position of the cube
        public int counter; // Counter for the number of cubes collected

        // Constructor to initialize the IndexData struct
        public IndexData(int i, int j, int counter)
        {
            this.i = i;
            this.j = j;
            this.counter = counter;
        }

        public void Save(string filePath)
        {
            string indexInfo = JsonUtility.ToJson(this); // Convert the index information to JSON format for easier parsing when loading
            File.WriteAllText(filePath, indexInfo); // Write the index information to the file
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                string indexInfo = File.ReadAllText(filePath); // Read the index information from the file
                IndexData loadedIndexData = JsonUtility.FromJson<IndexData>(indexInfo); // Convert the JSON string back to an IndexData object
                this.i = loadedIndexData.i; // Assign the loaded i value to the current instance
                this.j = loadedIndexData.j; // Assign the loaded j value to the current instance
                this.counter = loadedIndexData.counter; // Assign the loaded counter value to the current instance
            }
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

        // Method to save the list of CubeData objects to a file. This method will write the information of each cube to a text file in a readable format.
        public void Save(string filePath)
        {
            File.WriteAllText(filePath, string.Empty); // Clear the file before writing new data
            foreach (CubeData cube in cubes)
            {
                string cubeInfo = JsonUtility.ToJson(cube); // Convert the cube information to JSON format for easier parsing when loading
                File.AppendAllText(filePath, cubeInfo + "\n"); // Append the cube information to the file, adding a new line after each cube's data
            }
        }

        public void Load(string filePath)
        {
            cubes = new List<CubeData>(); // Initialize the list of cubes
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath); // Read all lines from the file
                foreach (string line in lines)
                {
                    CubeData cube = JsonUtility.FromJson<CubeData>(line); // Convert the JSON string back to a CubeData object
                    cubes.Add(cube); // Add the CubeData object to the list
                }
            }
        }
    }

    // Public Attributes
    public CubeObject cubePrefab; // Reference to the cube prefab to be instantiated
    public CubeData cubeData; // Instance of CubeData to hold the information of a single cube
    public CubeList cubeList; // Instance of CubeList to hold a list of CubeData objects for saving multiple cubes
    public IndexData indexData; // Instance of IndexData to hold the current index and counter values for saving and loading

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

    private void Awake()
    {
        cubeList.Load(Application.dataPath + "/Data/cube_data.txt"); // Load the cube data from a file when the MuseumController is created
        foreach (CubeData cubeData in cubeList.cubes)
        {
            Vector3 position = cubeData.position; // Get the position of the cube from the loaded data
            CubeObject cube = Instantiate(cubePrefab, position, Quaternion.identity); // Instantiate the cube at the specified position with no rotation
            cube.cubeMeshRenderer.material.color = cubeData.color; // Assign the color to the cube from the loaded data
            cube.name = cubeData.name; // Name the cube with the name from the loaded data
        }
        indexData.Load(Application.dataPath + "/Data/index_data.txt"); // Load the index data from a file when the MuseumController is created
        print("Data loaded successfully. You have " + indexData.counter + " cubes stored."); // Print a message to the console indicating that the data has been loaded and how many cubes are stored)
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Get the collect action from the Input System
        collectAction = InputSystem.actions.FindAction("Collect");
        counter = indexData.counter; // Set the counter to the loaded value from the index data
        i = indexData.i; // Set the i index to the loaded value from the index data
        j = indexData.j; // Set the j index to the loaded value from the index data
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
        cubeData = new CubeData(cube.name, cube.transform.position, cube.cubeMeshRenderer.material.color); // Create a CubeData instance with the cube's information
        cubeList.cubes.Add(cubeData); // Add the CubeData instance to the CubeList
    }

    private void OnDestroy()
    {
        IndexData indexData = new IndexData(i, j, counter); // Create an IndexData instance with the current index and counter values
        indexData.Save(Application.dataPath + "/Data/index_data.txt"); // Save the index data to a file when the MuseumController is destroyed
        cubeList.Save(Application.dataPath + "/Data/cube_data.txt"); // Save the cube data to a file when the MuseumController is destroyed
        print("Data saved successfully. You stored " + counter + " cubes."); // Print a message to the console indicating that the data has been saved
    }

}
