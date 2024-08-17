using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap), typeof(NavMeshSurface))]
public class PathGenerator : MonoBehaviour
{
    [SerializeField]
    [Range(1, 50)]
    private int _extents;

    [SerializeField]
    [Range(1, 50)]
    private int _excludeFrontRange;
    [SerializeField]
    [Range(1, 50)]
    private int _excludeBackRange;
    [SerializeField]
    [Range(1, 50)]
    private int _excludeRightRange;
    [SerializeField]
    [Range(1, 50)]
    private int _excludeLeftRange;

    [SerializeField]
    private TileBase _solidTile;

    [SerializeField]
    private GameObject _targetObject;

    [SerializeField]
    private EnemySpawner _enemySpawner;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLineStrip(new Vector3[]
        {
            transform.position + new Vector3(-_extents, 0, -_extents),
            transform.position + new Vector3(-_extents, 0, _extents),
            transform.position + new Vector3(_extents, 0, _extents),
            transform.position + new Vector3(_extents, 0, -_extents),
        }, true);
        Gizmos.DrawLineStrip(new Vector3[]
        {
            transform.position + new Vector3(-_extents, 1, -_extents),
            transform.position + new Vector3(-_extents, 1, _extents),
            transform.position + new Vector3(_extents, 1, _extents),
            transform.position + new Vector3(_extents, 1, -_extents),
        }, true);
        Gizmos.DrawLine(transform.position + new Vector3(-_extents, 0, -_extents), transform.position + new Vector3(-_extents, 1, -_extents));
        Gizmos.DrawLine(transform.position + new Vector3(-_extents, 0, _extents), transform.position + new Vector3(-_extents, 1, _extents));
        Gizmos.DrawLine(transform.position + new Vector3(_extents, 0, _extents), transform.position + new Vector3(_extents, 1, _extents));
        Gizmos.DrawLine(transform.position + new Vector3(_extents, 0, -_extents), transform.position + new Vector3(_extents, 1, -_extents));

        Gizmos.color = new Color(1, 0.5f, 0);
        var frontSigns = new Vector2Int(1, -1);
        var backSigns = new Vector2Int(-1, 1);
        var rightSigns = new Vector2Int(1, 1);
        var leftSigns = new Vector2Int(-1, -1);
        DrawExclusionCubes(_excludeFrontRange, frontSigns);
        DrawExclusionCubes(_excludeBackRange, backSigns);
        DrawExclusionCubes(_excludeRightRange, rightSigns);
        DrawExclusionCubes(_excludeLeftRange, leftSigns);
    }

    void DrawExclusionCubes(int range, Vector2Int signs)
    {
        var centerX = signs.x * range + 0.5f;
        var centerY = signs.y * range + 0.5f;
        DrawExclusionCube(centerX, centerY);
        for (var i = 1; i <= _extents - range; i++)
        {
            DrawExclusionCube(centerX - i * signs.x, centerY + i * signs.y);
            DrawExclusionCube(centerX + (i * signs.x), centerY - i * signs.y);
        }
    }

    void DrawExclusionCube(float x, float z)
    {
        Gizmos.DrawCube(transform.position + new Vector3(x, 0.5f, z), Vector3.one);
    }

    void Regenerate()
    {
        foreach (var child in transform.Cast<Transform>().ToArray())
        {
            DestroyImmediate(child.gameObject);
        }

        _tilemap.ClearAllTiles();

        var position = new Vector3Int(-_extents, -_extents, 0);
        var size = new Vector3Int(_extents * 2 + 1, _extents * 2 + 1, 1);
        var bounds = new BoundsInt(position, size);

        var gridPositions = bounds.allPositionsWithin
            .Collect()
            .Where(IsInsideInclusionZone)
            .Where(p => Random.Range(0f, 1f) < 0.25)
            .ToArray();
        var outsidePositions = bounds.allPositionsWithin
            .Collect()
            .Where(p => !IsInsideInclusionZone(p))
            .ToArray();
        _tilemap.SetTiles(gridPositions, Enumerable.Repeat(_solidTile, gridPositions.Length).ToArray());
        _tilemap.SetTiles(outsidePositions, Enumerable.Repeat(_solidTile, outsidePositions.Length).ToArray());

        var startingPoint = GenerateValidPoint();
        _tilemap.SetTile(startingPoint, null);
        var targetPositionTile = GenerateValidPoint();
        _tilemap.SetTile(targetPositionTile, null);

        var targetPosition = _tilemap.CellToLocal(targetPositionTile) + new Vector3(0.5f, 0.5f, 0.5f);
        var target = Instantiate(_targetObject, transform);
        target.transform.position = targetPosition;

        var startPosition = _tilemap.CellToLocal(startingPoint) + new Vector3(0.5f, 0.5f, 0.5f);
        var enemySpawner = Instantiate(_enemySpawner, transform);
        enemySpawner.transform.position = startPosition;
        enemySpawner.Target = target.transform;

        _meshSurface.BuildNavMesh();
    }

    private bool IsInsideInclusionZone(Vector3Int p)
        => (p.x + p.y) > -(_excludeLeftRange * 2) && (p.x + p.y) < (_excludeRightRange * 2) && (p.x - p.y) > -(_excludeBackRange * 2) && (p.x - p.y) < (_excludeFrontRange * 2);

    private Vector3Int GenerateValidPoint()
    {
        Vector3Int p;
        do
        {
            p = new Vector3Int(Random.Range(-_extents, _extents + 1), Random.Range(-_extents, _extents + 1), 0);
        } while (!IsInsideInclusionZone(p));

        return p;
    }
}
