using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    [SerializeField]
    private int n;
    public GameObject star;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 2 * n - 1; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i < n)
                {
                    if (j < n - 1 - i)
                    {
                        
                        Debug.Log(' ');
                    }
                    else
                    {
                        SpawnStar(i, j);
                        Debug.Log('*');
                    }
                }
                else
                {
                    if (j < i - n + 1)
                    {

                        Debug.Log(' ');
                    }
                    else
                    {
                        SpawnStar(i,j);
                        Debug.Log('*');
                    }
                }
                
                
            }
        }
    }

   void SpawnStar(int i,int j)
    {
        Vector3 spawnPos = new Vector3(j,-i,0);
        Instantiate(star,spawnPos, Quaternion.identity);
    }
}
