using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class WallPatterns {

    private List<int[,]> patterns;

    public WallPatterns()
    {
        patterns = new List<int[,]>();
        
        string[] files;

        if (Debug.isDebugBuild)
            files = Directory.GetFiles("Assets/Resources/", "*.txt");
        else
            files = Directory.GetFiles("3DGON_Data/Resources/Patterns/", "*.txt");

        StreamReader reader;
        
        foreach ( string file in files)
        {
            reader = new StreamReader(file);
            string text = reader.ReadToEnd();

            int[,] walls = new int[(text.Split('\n')[0].Length + 1) / 2, 6];

            int i = 0, j = 0;
            foreach (var row in text.Split('\n'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    walls[j, i] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }
            patterns.Add(walls);
            reader.Close();
        }
    }

    public int[,] ParseRandomPattern()
    {
        return patterns[Random.Range(0, patterns.Count)];
    }
}
