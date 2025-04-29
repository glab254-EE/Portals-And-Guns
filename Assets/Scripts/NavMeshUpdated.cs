using Unity.AI.Navigation;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject InstanceObject;
    [SerializeField] private NavMeshSurface navMesh;
    void SpawnObject()
    {
        Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit pos1;
        bool succ = Physics.Raycast(ray1,out pos1);
        if (!succ) return;
        Instantiate(InstanceObject,pos1.point,Quaternion.identity,null);
        navMesh.RemoveData();
        navMesh.BuildNavMesh();
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SpawnObject();
        }
    }
}
