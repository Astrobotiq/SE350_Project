using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshDynamicUpdate : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    float time = 0;

    private void Awake()
    {
        var Temp = GameObject.FindWithTag("NavMesh");
        surface = Temp.GetComponent<NavMeshSurface>();
    }

    void Update()
    {
        if (time >= 5)
        {
            surface.BuildNavMesh();
            time = 0;
        }

        time += Time.deltaTime;
        
    }

    


}
