using System.Collections.Generic;
using UnityEngine;

public class UnitsLayerManager : MonoBehaviour
{
    public List<UnitsMovement> units = new List<UnitsMovement>();
    public Canvas canvas; // Канвас, который должен быть выше
    public string canvasSortingLayer = "UI";
    public int canvasSortingOrder = 200; // Высокий порядок сортировки для канваса
    public ClickPoint clickPointController;

    void Start()
    {
        // Установка слоя рендеринга и порядка сортировки для канваса
        SetCanvasSortingLayer(canvas, canvasSortingLayer, canvasSortingOrder);
    }

    void Update()
    {
        // Обновление слоев при каждом кадре
        UpdateLayers();
    }

    public void UpdateLayers()
    {
        List<UnitsMovement> selectedUnits = new List<UnitsMovement>();
        List<UnitsMovement> unselectedUnits = new List<UnitsMovement>();

        // Разделение объектов на выбранные и невыбранные
        foreach (UnitsMovement unit in units)
        {
            if (unit._selected)
            {
                selectedUnits.Add(unit);
            }
            else
            {
                unselectedUnits.Add(unit);
            }
        }

        // Установка слоев для выбранных объектов
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            UnitsMovement unit = selectedUnits[i];
            SetSortingOrderRecursively(unit.transform, 100 + (selectedUnits.Count - i));
        }

        // Установка слоев для невыбранных объектов
        foreach (UnitsMovement unit in unselectedUnits)
        {
            SetSortingOrderRecursively(unit.transform, 0);
        }
    }

    private void SetSortingOrderRecursively(Transform obj, int order)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = order;
        }
        foreach (Transform child in obj)
        {
            SetSortingOrderRecursively(child, order);
        }
    }

    private void SetCanvasSortingLayer(Canvas canvas, string layer, int order)
    {
        if (canvas != null)
        {
            canvas.sortingLayerName = layer;
            canvas.sortingOrder = order;
        }
    }
}
