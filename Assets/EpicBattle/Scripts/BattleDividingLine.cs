using UnityEngine;

/// <summary>
/// Đường kẻ phân chia 2 phe trên bản đồ.
/// Gắn vào 1 GameObject riêng — di chuyển GameObject đó = di chuyển đường kẻ.
/// Đường kẻ chạy theo trục Z (dọc map), tâm = position của GameObject này.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class BattleDividingLine : MonoBehaviour
{
    [Header("Line Settings")]
    [Tooltip("Độ dài của đường kẻ (đơn vị: m)")]
    public float lineLength = 20f;

    [Tooltip("Màu đường kẻ")]
    public Color lineColor = new Color(1f, 0.25f, 0.25f, 0.85f);

    [Tooltip("Độ dày đường kẻ")]
    public float lineWidth = 0.1f;

    [Tooltip("Trục đường kẻ chạy theo")]
    public LineAxis axis = LineAxis.Z;

    public enum LineAxis { X, Z }

    private LineRenderer _lr;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
    }

    void Start()
    {
        DrawLine();
    }

    // Vẽ lại ngay trong Editor khi thay đổi giá trị
    void OnValidate()
    {
        if (_lr == null) _lr = GetComponent<LineRenderer>();
        if (_lr != null) DrawLine();
    }

    void DrawLine()
    {
        _lr.useWorldSpace = false; 
        _lr.positionCount = 2;

        _lr.startColor = _lr.endColor = lineColor;
        _lr.startWidth = _lr.endWidth = lineWidth;

        if (_lr.material == null || _lr.material.name.Contains("Default-Line"))
            _lr.material = new Material(Shader.Find("Sprites/Default"));

        _lr.material.color = lineColor;

        float half = lineLength / 2f;

        if (axis == LineAxis.Z)
        {
            _lr.SetPosition(0, new Vector3(0f, 0.05f, -half));
            _lr.SetPosition(1, new Vector3(0f, 0.05f,  half));
        }
        else // LineAxis.X
        {
            _lr.SetPosition(0, new Vector3(-half, 0.05f, 0f));
            _lr.SetPosition(1, new Vector3( half, 0.05f, 0f));
        }
    }
}