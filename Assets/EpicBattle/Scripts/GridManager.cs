using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Grid Settings")]
    public int columns  = 10;
    public int rows     = 8;
    public float cellSize = 1.5f;
    public float cellGap  = 0.1f;

    [Header("Map Info")]
    public Vector3 mapCenter = Vector3.zero;

    [Header("Prefabs & Materials")]
    public GameObject cellPrefab;
    public Material   cellMaterial;

    [Header("Dividing Line")]
    public Color divideLineColor = new Color(1f, 0.3f, 0.3f, 0.8f);
    public float divideLineWidth = 0.08f;

    // Mảng 2D để tra ô nhanh theo tọa độ [row, col]
    private GridCell[,] _grid;

    void Awake() => Instance = this;

    void Start()
    {
        _grid = new GridCell[rows, columns];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        float totalWidth = columns * (cellSize + cellGap) - cellGap;
        float startX = mapCenter.x - totalWidth / 2f + cellSize / 2f;
        // Lưới nằm bên phía -Z (phía Player)
        float startZ = mapCenter.z - (cellSize / 2f) - (rows - 1) * (cellSize + cellGap);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float x = startX + col * (cellSize + cellGap);
                float z = startZ + row * (cellSize + cellGap);
                Vector3 pos = new Vector3(x, mapCenter.y + 0.02f, z);

                GameObject go = Instantiate(cellPrefab, pos, Quaternion.Euler(90f, 0f, 0f), transform);
                go.name = $"Cell_{row}_{col}";
                go.transform.localScale = new Vector3(cellSize, cellSize, 1f);

                if (cellMaterial != null)
                    go.GetComponent<Renderer>().material = Instantiate(cellMaterial);

                GridCell cell = go.GetComponent<GridCell>();
                if (cell == null) cell = go.AddComponent<GridCell>();

                // Lưu tọa độ lưới vào cell để sau tra ngược
                cell.Row = row;
                cell.Col = col;

                _grid[row, col] = cell;
            }
        }
    }

    /// <summary>
    /// Lấy tất cả ô trong vùng footprintRadius xung quanh ô trung tâm.
    /// </summary>
    public List<GridCell> GetFootprintCells(GridCell center, int radius)
    {
        var result = new List<GridCell>();
        for (int dr = -radius; dr <= radius; dr++)
        {
            for (int dc = -radius; dc <= radius; dc++)
            {
                int r = center.Row + dr;
                int c = center.Col + dc;
                if (r >= 0 && r < rows && c >= 0 && c < columns)
                    result.Add(_grid[r, c]);
            }
        }
        return result;
    }

    /// <summary>
    /// Kiểm tra toàn bộ vùng footprint có trống không.
    /// </summary>
    public bool IsFootprintFree(GridCell center, int radius)
    {
        foreach (var cell in GetFootprintCells(center, radius))
        {
            if (cell.IsOccupied) return false;
        }
        return true;
    }

    /// <summary>
    /// Khoá hoặc mở toàn bộ vùng footprint.
    /// </summary>
    public void SetFootprintOccupied(GridCell center, int radius, bool occupied)
    {
        foreach (var cell in GetFootprintCells(center, radius))
        {
            if (occupied) cell.Occupy();
            else          cell.Vacate();
        }
    }

    public void HideAllCells()
    {
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < columns; c++)
                _grid[r, c].gameObject.SetActive(false);
    }
}