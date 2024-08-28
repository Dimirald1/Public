using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameHighlighting : MonoBehaviour
{
    public Canvas targetCanvas;
    public Image frame;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;
    private RectTransform canvasRectTransform;

    void Start()
    {
        canvasRectTransform = targetCanvas.GetComponent<RectTransform>();
        frame.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartFrameSelection();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateFrameSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndFrameSelection();
        }
    }

    private void StartFrameSelection()
    {
        _frameStart = GetMousePositionInCanvas();
    }

    private void UpdateFrameSelection()
    {
        _frameEnd = GetMousePositionInCanvas();

        Vector2 min = Vector2.Min(_frameStart, _frameEnd);
        Vector2 max = Vector2.Max(_frameStart, _frameEnd);

        frame.rectTransform.anchoredPosition = min;
        Vector2 size = max - min;
        if (size.magnitude > 10)
        {
            frame.enabled = true;
            frame.rectTransform.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));

            Rect rect = new Rect(min, size);

            UnitsMovement[] allUnits = FindObjectsOfType<UnitsMovement>();
            int selectedCount = 0;
            foreach (var unit in allUnits)
            {
                if (unit.SpriteRenderer.color.a < 1f)
                {
                    continue; // Пропускаем прозрачные объекты
                }

                Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(targetCanvas.worldCamera, unit.transform.position);
                Vector2 localPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera, out localPosition);

                if (rect.Contains(localPosition) && selectedCount < 3)
                {
                    unit._selected = true;
                    selectedCount++;
                }
                else
                {
                    unit._selected = false;
                }
                unit.UpdateSelectionState();
            }
        }
    }

    private void EndFrameSelection()
    {
        frame.enabled = false;
    }

    private Vector2 GetMousePositionInCanvas()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera, out mousePos);
        return mousePos;
    }
}
