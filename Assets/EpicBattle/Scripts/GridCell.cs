using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header("Colors")]
    public Color normalColor   = new Color(1f, 1f, 1f, 0.15f);
    public Color hoverColor    = new Color(0.3f, 0.9f, 0.3f, 0.45f);
    public Color occupiedColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);
    public Color blockedColor  = new Color(1f, 0.6f, 0f, 0.3f); // cam = bị khoá bởi con khác

    // Tọa độ trong lưới — GridManager gán khi khởi tạo
    public int Row { get; set; }
    public int Col { get; set; }

    public bool IsOccupied { get; private set; }

    private Renderer _rend;
    private Material _mat;

    void Awake()
    {
        _rend = GetComponent<Renderer>();
        _mat  = _rend.material;
        SetColor(normalColor);
    }

    void OnMouseEnter()
    {
        if (IsOccupied) return;

        // Preview vùng footprint của unit đang chọn khi hover
        UnitPlacer.Instance?.PreviewFootprint(this, true);
    }

    void OnMouseExit()
    {
        if (IsOccupied) return;
        UnitPlacer.Instance?.PreviewFootprint(this, false);
    }

    void OnMouseDown()
    {
        if (IsOccupied) return;
        UnitPlacer.Instance?.TryPlaceUnit(this);
    }

    public void Occupy()   { IsOccupied = true;  SetColor(occupiedColor); }
    public void Vacate()   { IsOccupied = false; SetColor(normalColor); }
    public void Preview()  { if (!IsOccupied) SetColor(hoverColor); }
    public void Unpreview(){ if (!IsOccupied) SetColor(normalColor); }

    public void SetColor(Color c) => _mat.color = c;
}