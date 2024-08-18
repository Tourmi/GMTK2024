using System.Linq;
using UnityEngine;

public class TowerPlacementComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject _ghostTower;

    [SerializeField]
    private GameObject _towerPrefab;

    private GameObject _ghostTowerInstance;

    private void Awake()
    {
        _ghostTowerInstance = Instantiate(_ghostTower);
        _ghostTowerInstance.gameObject.SetActive(false);
    }

    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics.RaycastAll(ray, 100f, LayerMask.GetMask("UI"));
        foreach (var hit in hits.Reverse())
        {
            if (!hit.collider.gameObject.TryGetComponent<TowerAnchorComponent>(out var anchor))
            {
                continue;
            }

            if (anchor._occupiedObject != null)
            {
                continue;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var tower = Instantiate(_towerPrefab, anchor.transform);
                tower.transform.position = anchor.transform.position;
                anchor._occupiedObject = tower;
                break;
            }

            _ghostTowerInstance.SetActive(true);
            _ghostTowerInstance.transform.position = anchor.transform.position;
            return;
        }

        _ghostTowerInstance.SetActive(false);
    }
}
