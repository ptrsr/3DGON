using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallScript : MonoBehaviour {

    public float _width;
    public float _distance;
    public float _speed;
    public int _side;

    public float maxHeight;
    public float heightSmoothing;

    public MusicAnalyser musicAnalyser;
    public Material material;

    private float _currentHeight;
    private Mesh mesh;
    private MeshFilter mf;
    private MeshRenderer mr;
    private MeshCollider mc;

    private List<Vector3> originalVertices;
    private List<Vector3> vertices;

    private delegate void WallDelegate();
    private WallDelegate wallDelegate;

    public delegate void StopDelegate();
    public static StopDelegate stopDelegate;

	void Start () {

        mf = gameObject.AddComponent<MeshFilter>();
        mr = gameObject.AddComponent<MeshRenderer>();
        mc = gameObject.AddComponent<MeshCollider>();

        mr.material = material;

        mesh = new Mesh();
        
        UpdateMesh();
        UpdateTriangles();

        mesh.RecalculateNormals();
        mesh.MarkDynamic();

        mc.convex = true;

        wallDelegate += UpdateMesh;
        wallDelegate += step;

        stopDelegate += Stop;
	}
	
	void Update () {

        _currentHeight += (musicAnalyser.RmsValue * maxHeight - _currentHeight) * 0.4f;

        if (wallDelegate != null)
            wallDelegate();
	}

    void UpdateMesh()
    {
        Vector3 angledVector = new Vector3(Mathf.Cos(60 * Mathf.Deg2Rad), 0, Mathf.Sin(60 * Mathf.Deg2Rad));

        float frontDistance = Mathf.Clamp(_distance, 0, Mathf.Infinity);
        float rearDistance = Mathf.Clamp(_distance + _width, 0, Mathf.Infinity);

        if (rearDistance == 0)
            Destroy(gameObject);

        originalVertices = new List<Vector3> { new Vector3(frontDistance, 0, 0), new Vector3(rearDistance, 0, 0), angledVector * frontDistance, angledVector * rearDistance };

        for (int i = 0; i < 4; i++)
            originalVertices.Add(originalVertices[i] + new Vector3(0, _currentHeight, 0));

        vertices = new List<Vector3>();

        AddTriangle(5, 6, 7);
        AddTriangle(4, 6, 5);
        AddTriangle(1, 0, 4);
        AddTriangle(1, 4, 5);
        AddTriangle(2, 3, 6);
        AddTriangle(3, 7, 6);
        AddTriangle(0, 2, 6);
        AddTriangle(0, 6, 4);
        AddTriangle(3, 1, 5);
        AddTriangle(3, 5, 7);

        mesh.vertices = vertices.ToArray();
        mesh.RecalculateBounds();

        mf.mesh = mesh;

        if (rearDistance > 0)
            mc.sharedMesh = mesh;
    }

    private void AddTriangle(int point1, int point2, int point3)
    {
        vertices.Add(originalVertices[point1]);
        vertices.Add(originalVertices[point2]);
        vertices.Add(originalVertices[point3]);
    }

    private void UpdateTriangles()
    {
        int[] triangles = new int[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
            triangles[i] = i;

        mesh.triangles = triangles;
    }

    private void step()
    {
        _distance -= _speed;
    }

    public void Hit()
    {
        Main.s.MoveNext(Command.Stop);
        StartCoroutine(Blink());
    }

    private void Stop()
    {
        StartCoroutine(BackOff());
    }

    private void Reverse()
    {
        if (_distance < 50)
            _speed += _speed / 15;
        else
            Destroy(gameObject);

    }

    private IEnumerator BackOff()
    {
        _speed = 0;
        yield return new WaitForSeconds(1);
        _speed = -0.01f;
        wallDelegate += Reverse;
    }

    private IEnumerator Blink()
    {
        Material newMaterial = new Material(mr.material);
        mr.material = newMaterial;

        bool red = true;

        while (true)
        {
            if (red)
                mr.material.SetColor("_EmissionColor", Color.red);
            else
                mr.material.SetColor("_EmissionColor", Color.white);

            red ^= true;
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OnDestroy()
    {
        stopDelegate -= Stop;
    }
}
