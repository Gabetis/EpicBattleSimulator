using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitPlacer : MonoBehaviour
{
    public static UnitPlacer Instance { get; private set; }

    [Header("Unit Prefabs (Player side)")]
    public GameObject[] unitPrefabs;
    public int selectedIndex = 0;

    public bool battleStarted = false;
    private GridCell _lastPreviewCenter;

    void Awake() => Instance = this;

    void Update()
    {
        for (int i = 0; i < unitPrefabs.Length && i < 9; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectUnit(i);
    }

    public void SelectUnit(int index)
    {
        if (index < 0 || index >= unitPrefabs.Length) return;
        selectedIndex = index;
    }

    int GetCurrentFootprint()
    {
        if (unitPrefabs == null || selectedIndex >= unitPrefabs.Length) return 0;
        var data = unitPrefabs[selectedIndex]?.GetComponent<BattleUnitData>();
        return data != null ? data.footprintRadius : 0;
    }

    public void PreviewFootprint(GridCell center, bool show)
    {
        if (_lastPreviewCenter != null && GridManager.Instance != null)
        {
            foreach (var c in GridManager.Instance.GetFootprintCells(_lastPreviewCenter, GetCurrentFootprint()))
                c.Unpreview();
        }

        if (!show) { _lastPreviewCenter = null; return; }

        _lastPreviewCenter = center;
        int radius = GetCurrentFootprint();
        bool canPlace = GridManager.Instance.IsFootprintFree(center, radius);

        foreach (var c in GridManager.Instance.GetFootprintCells(center, radius))
        {
            if (canPlace) c.Preview();
            else          c.SetColor(c.blockedColor);
        }
    }

    public void TryPlaceUnit(GridCell cell)
    {
        if (battleStarted) return;
        if (unitPrefabs == null || unitPrefabs.Length == 0) return;

        int radius = GetCurrentFootprint();

        if (!GridManager.Instance.IsFootprintFree(cell, radius))
        {
            Debug.Log("[UnitPlacer] Không đặt được — vùng bị chặn!");
            return;
        }

        // Đọc thông số nhấc bổng từ Prefab
        float yOffset = 0f;
        var unitData = unitPrefabs[selectedIndex].GetComponent<BattleUnitData>();
        if (unitData != null)
        {
            yOffset = unitData.visualYOffset;
        }
        // Cộng thêm yOffset vào trục Y
        Vector3 spawnPos = new Vector3(cell.transform.position.x, yOffset, cell.transform.position.z);
        GameObject unit = Instantiate(unitPrefabs[selectedIndex], spawnPos, Quaternion.identity);
        unit.tag = "Player";

        Animator anim = unit.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.enabled = true;
 
            anim.Play("Idle"); 
        }

        AdjustUnitToGround(unit, spawnPos.y);

        GridManager.Instance.SetFootprintOccupied(cell, radius, true);
        _lastPreviewCenter = null;

        Debug.Log($"[UnitPlacer] Đặt {unitPrefabs[selectedIndex].name} thành công!");
    }

    public void StartBattle()
    {
        battleStarted = true;
        GridManager.Instance?.HideAllCells();
    }

    private void AdjustUnitToGround(GameObject unit, float groundY)
    {
        // 1. Cố gắng tìm giới hạn của Collider trước
        Collider[] colliders = unit.GetComponentsInChildren<Collider>();
        if (colliders.Length > 0)
        {
            Bounds bounds = colliders[0].bounds;
            for (int i = 1; i < colliders.Length; i++)
            {
                bounds.Encapsulate(colliders[i].bounds);
            }
            // Tính khoảng cách từ điểm thấp nhất của nhân vật đến mặt đất
            float offset = groundY - bounds.min.y;
            // Đẩy toàn bộ nhân vật lên
            unit.transform.position += new Vector3(0, offset, 0);
            return;
        }

        // 2. Nếu nhân vật không có Collider, tìm qua Mesh Renderer
        Renderer[] renderers = unit.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
            float offset = groundY - bounds.min.y;
            unit.transform.position += new Vector3(0, offset, 0);
        }
    }
}