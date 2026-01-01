using UnityEngine;

public class M3_SafeAreaEnforcer : MonoBehaviour
{
    public void ApplySafeArea()
    {
        RectTransform TargetRect = GetComponent<RectTransform>();
        if (TargetRect == null)
            return;

        Rect SafeArea = Screen.safeArea;

        Vector2 AnchorMin = SafeArea.position;
        Vector2 AnchorMax = SafeArea.position + SafeArea.size;

        AnchorMin.x /= Screen.width;
        AnchorMin.y /= Screen.height;
        AnchorMax.x /= Screen.width;
        AnchorMax.y /= Screen.height;

        TargetRect.anchorMin = AnchorMin;
        TargetRect.anchorMax = AnchorMax;

        TargetRect.offsetMin = Vector2.zero;
        TargetRect.offsetMax = Vector2.zero;
    }
}