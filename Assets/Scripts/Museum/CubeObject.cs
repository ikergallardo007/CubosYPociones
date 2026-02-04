using UnityEngine;

public class CubeObject : MonoBehaviour
{
    // Public attributes
    [System.NonSerialized]
    public MeshRenderer cubeMeshRenderer;

    // Awake method.
    private void Awake()
    {
        cubeMeshRenderer = GetComponent<MeshRenderer>();
    }
}
