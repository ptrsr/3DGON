using UnityEngine;
using System.Collections;

public class WallSpawnerScript : MonoBehaviour
{

    public float spawnDistance = 15;
    public float spawnWidth = 3;
    public float spawnSpeed = 0.06f;
    public float maxHeight = 10;
    public float heightSmoothing = 0.4f;


    public MusicAnalyser musicAnalyser;
    public GameObject wallPrefab;

    private delegate void SpawnerDelegate();
    private SpawnerDelegate spawnerDelegate;

    private WallPatterns p;

    private int[,] currentPattern;
    private int wave = 0;

    float timer = 0;

    void Start()
    {
        p = new WallPatterns();
        currentPattern = p.ParseRandomPattern();
    }

    // Update is called once per frame
    void Update()
    {

        if (spawnerDelegate != null)
            spawnerDelegate();
    }

    public void StartSpawning()
    {
        if (spawnerDelegate == null)
            spawnerDelegate += StartSpawning;

        if (timer > spawnWidth)
        {
            if (wave >= currentPattern.GetLength(0))
            {
                currentPattern = p.ParseRandomPattern();
                wave = 0;
            }
            else
            {
                for (int i = 0; i <= 5; i++)
                {
                    if (currentPattern[wave, i] == 1)
                    {
                        GameObject wall = GameObject.Instantiate(wallPrefab);
                        WallScript wallScript = wall.GetComponent<WallScript>();

                        wallScript._side = i;
                        wall.transform.Rotate(new Vector3(0, 60 * i, 0));
                        
                        wallScript._distance = spawnDistance - (timer - spawnWidth);
                        wallScript._width = spawnWidth;
                        wallScript._speed = spawnSpeed;
                        wallScript.maxHeight = maxHeight;
                        wallScript.heightSmoothing = heightSmoothing;

                        wallScript.musicAnalyser = musicAnalyser;
                    }
                }
            }
            wave++;
            timer = 0;
        }
        timer += spawnSpeed;
    }

    public void StopSpawning()
    {
        spawnerDelegate -= StartSpawning;
    }

    private void SpawnWall(int side)
    {
        GameObject wall = (GameObject)Instantiate(wallPrefab);
        wall.transform.Rotate(0, side * 60, 0);
        WallScript wallScript = wall.GetComponent<WallScript>();

        wallScript._distance = 6;
    }
}
