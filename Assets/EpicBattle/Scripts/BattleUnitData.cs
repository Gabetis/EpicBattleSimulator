using UnityEngine;

/// <summary>
/// Gắn component này vào từng Prefab nhân vật để định nghĩa
/// khoảng không gian mà nó chiếm trên lưới.
/// </summary>
public class BattleUnitData : MonoBehaviour
{
    [Tooltip("Bán kính ô bị khoá xung quanh (0=chỉ ô đứng, 1=3x3, 2=5x5)")]
    public int footprintRadius = 0;
    //   Peasant / Archer  → footprintRadius = 0  (1x1)
    //   Knight / Warrior  → footprintRadius = 1  (3x3)
    //   Beast / Boss      → footprintRadius = 2  (5x5)

    
    [Tooltip("Thông số nhấc nhân vật lên mặt đất (ví dụ: 1.0 hoặc 1.2)")]
    public float visualYOffset = 0f; // 
}