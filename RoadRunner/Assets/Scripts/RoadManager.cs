using System.Collections;
using System.Collections.Generic; //for list
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject[] roadPrefabs; //array of prefabs

    private Transform playerTransform; //player 
    private float spawnZ = -3.0f; // spawining in Z direction value (-3 for starting on road not in ground plane)
    private float tileLenght = 56.0f; //z value of road prefab
    private int tilesOnScreen = 4; //4 tiles are shown on scrren (initial prefabs on screen)
    private float safeZone = 100.0f; //remove previous tile when it is in safe Zone 
    private int lastPrefabIndex = 0;

    private List<GameObject> activeTile;

    // Start is called before the first frame update
    void Start()
    {
        activeTile = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //Initial road prefabs on screen
        for(int i=0; i <tilesOnScreen;i++)
        {
            if(i<2)
            {
                spawnTile(0);
            }
            else {
                spawnTile();
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        //spawning the roads with calling function
        if((playerTransform.position.z - safeZone) > (spawnZ - tilesOnScreen * tileLenght) )
        {
            spawnTile();
            deleteTile();
        }
    }

    private void spawnTile(int prefabIndex =-1)
    {
        GameObject go;
        if (prefabIndex == - 1)
        {
            //random prefab other than 0
            go = Instantiate(roadPrefabs[RandomPrefabRoadGeneration()]) as GameObject;
        }
        else
        {
            //First 2 prefab are normal one  = 0
            go = Instantiate(roadPrefabs[prefabIndex]) as GameObject;
        }
        go.transform.SetParent(transform); //main gameObject cha child (RoadManager) 
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLenght;
        activeTile.Add (go);
    }
    private void deleteTile() // stop extended memory usage(Delete Previous tiles) 
    {
        Destroy(activeTile[0]);
        activeTile.RemoveAt(0);
    }

    private int RandomPrefabRoadGeneration()
    {
        if (roadPrefabs.Length <= 1)
            return 0;
        int randomIndex = lastPrefabIndex;
        while(randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, roadPrefabs.Length);
        }
        lastPrefabIndex = randomIndex;
        return randomIndex;
    }
}
