using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour
{
    [SerializeField]
    private Material darkMaterial;
    [SerializeField]
    private Material lightMaterial;

    [SerializeField]
    private float triangleDistance = 50;

    private GameObject[] triangles;

    void Start()
    {
        CreateBackground();
    }

    private void CreateBackground()
    {
        float w = triangleDistance * Mathf.Tan(30 * (Mathf.PI / 180));

        triangles = new GameObject[6];

        for (int i = 0; i < 6; i++)
        {
            GameObject backgroundTriangle = new GameObject();
            backgroundTriangle.name = "triangle " + i;
            triangles[i] = backgroundTriangle;

            MeshFilter mf = backgroundTriangle.AddComponent<MeshFilter>();
            MeshRenderer mr = backgroundTriangle.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(triangleDistance, 0, w), new Vector3(triangleDistance, 0, -w) };
            mesh.normals = new Vector3[] { new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(0, 1, 0) };
            mesh.triangles = new int[] { 0, 1, 2 };
            mf.mesh = mesh;

            mr.material = i % 2 == 0 ? darkMaterial : lightMaterial;

            backgroundTriangle.transform.Rotate(new Vector3(0, i * 60 + 30, 0));
            backgroundTriangle.transform.SetParent(this.transform);
        }
    }
}
