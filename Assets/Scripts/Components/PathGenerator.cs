using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap), typeof(NavMeshSurface))]
public class PathGenerator : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _extents;

    [SerializeField]
    private TileBase _solidTile;

    private Tilemap _tilemap;
    private NavMeshSurface _meshSurface;

    void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        _meshSurface = GetComponent<NavMeshSurface>();

        Regenerate();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F2))
        {
            Regenerate();
        }
    }

    void Regenerate()
    {
        _tilemap.CompressBounds();

        foreach (var child in transform.Cast<Transform>().ToArray())
        {
            DestroyImmediate(child.gameObject);
        }

        var position = new Vector3Int(-_extents.x, -_extents.y, 0);
        var size = new Vector3Int(_extents.x * 2, _extents.y * 2, 1);
        var bounds = new BoundsInt(position, size);

        var gridPositions = bounds.allPositionsWithin.Collect().Where(p => Random.Range(0f, 1f) < 0.5).ToArray();
        _tilemap.ClearAllTiles();
        _tilemap.SetTiles(gridPositions, Enumerable.Repeat(_solidTile, gridPositions.Length).ToArray());
        _meshSurface.BuildNavMesh();
    }
}
