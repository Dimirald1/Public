using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitsMovement : MonoBehaviour
{
    public List<UnitsMovement> moveToMice = new List<UnitsMovement>();
    public GameObject circle;
    public UnitsLayerManager layerManager;

    public NavMeshAgent agent;
    public bool _selected;
    private SpriteRenderer _spriteRenderer;
    public static List<UnitsMovement> selectedUnits = new List<UnitsMovement>();

    public Collider2D unitCollider;

    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public GameObject positionController;
    public Canvas baseMode;
    public Canvas attackMode;

    void Start()
    {
        moveToMice.Add(this);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (layerManager.clickPointController != null)
        {
            layerManager.clickPointController.targetObject.SetActive(true);
        }
    }

    void Update()
    {
        // Check if baseMode is active; if so, do nothing.
        if (baseMode.isActiveAndEnabled)
        {
            return;
        }

        // Ensure the code only runs when attackMode is active
        if (!attackMode.isActiveAndEnabled)
        {
            return;
        }

        if (_spriteRenderer.color.a < 1f)
        {
            return;
        }

        // Handle right-click movement
        if (Input.GetMouseButton(1) && _selected)
        {
            HandleRightClickMovement();
        }

        // Handle left-click deselection
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            HandleLeftClickDeselection();
        }

    }

    private void HandleRightClickMovement()
    {
        Vector3 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPoint.z = transform.position.z;

        if (unitCollider.OverlapPoint(clickPoint))
        {
            // Move units to the click point if it overlaps with the unit's collider
            MoveSelectedUnits(clickPoint);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            // Handle case when C is pressed and the click is not on the unit collider
            RaycastHit2D hit = Physics2D.Raycast(clickPoint, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject != gameObject)
            {
                Transform sideController = hit.collider.transform.Find("SideController");
                if (sideController != null && sideController.CompareTag("Enemy"))
                {
                    MoveSelectedUnits(clickPoint, 0.2f); // Move units slightly short of the click point
                }
            }
        }
    }

    private void MoveSelectedUnits(Vector3 clickPoint, float stopDistance = 0f)
    {
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(clickPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            return;
        }

        float distance = 0.15f;
        if (selectedUnits.Count == 3)
        {
            distance *= 2;
        }

        // Move the selected units to the target position, adjusting for stopDistance
        if (selectedUnits.Count == 1)
        {
            if (selectedUnits[0].agent.isOnNavMesh)
            {
                selectedUnits[0].agent.SetDestination(clickPoint - new Vector3(0, 0, stopDistance));
                positionController.tag = "InTheAttack";
            }
        }
        else if (selectedUnits.Count == 2)
        {
            if (selectedUnits[0].agent.isOnNavMesh)
            {
                selectedUnits[0].agent.SetDestination(clickPoint + new Vector3(-distance, 0, 0) - new Vector3(0, 0, stopDistance));
            }
            if (selectedUnits[1].agent.isOnNavMesh)
            {
                selectedUnits[1].agent.SetDestination(clickPoint + new Vector3(distance, 0, 0) - new Vector3(0, 0, stopDistance));
            }
            positionController.tag = "InTheAttack";
        }
        else if (selectedUnits.Count == 3)
        {
            if (selectedUnits[0].agent.isOnNavMesh)
            {
                selectedUnits[0].agent.SetDestination(clickPoint + new Vector3(-distance, 0, 0) - new Vector3(0, 0, stopDistance));
            }
            if (selectedUnits[1].agent.isOnNavMesh)
            {
                selectedUnits[1].agent.SetDestination(clickPoint - new Vector3(0, 0, stopDistance));
            }
            if (selectedUnits[2].agent.isOnNavMesh)
            {
                selectedUnits[2].agent.SetDestination(clickPoint + new Vector3(distance, 0, 0) - new Vector3(0, 0, stopDistance));
            }
            positionController.tag = "InTheAttack";
        }

        if (layerManager.clickPointController != null)
        {
            layerManager.clickPointController.HandleClick(clickPoint);
        }
    }


    private void HandleLeftClickDeselection()
    {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

        if (hit.collider == null || hit.collider.gameObject != gameObject)
        {
            _selected = false;
            circle.SetActive(false);

            if (selectedUnits.Contains(this))
            {
                selectedUnits.Remove(this);
                layerManager.UpdateLayers();
            }
        }
    }

    private void OnMouseDown()
    {
        // Check if baseMode is active; if so, do nothing.
        if (baseMode.isActiveAndEnabled)
        {
            return;
        }

        // Ensure the code only runs when attackMode is active
        if (!attackMode.isActiveAndEnabled)
        {
            return;
        }

        if (_spriteRenderer.color.a < 1f)
        {
            return;
        }

        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0;

        if (unitCollider.OverlapPoint(clickPosition))
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                if (!selectedUnits.Contains(this))
                {
                    if (selectedUnits.Count >= 3)
                    {
                        UnitsMovement firstSelected = selectedUnits[0];
                        firstSelected._selected = false;
                        firstSelected.circle.SetActive(false);
                        selectedUnits.RemoveAt(0);
                    }

                    if (selectedUnits.Count < 3)
                    {
                        selectedUnits.Add(this);
                        _selected = true;
                        circle.SetActive(true);
                        layerManager.UpdateLayers();
                    }
                }
                else
                {
                    selectedUnits.Remove(this);
                    _selected = false;
                    circle.SetActive(false);
                    layerManager.UpdateLayers();
                }
            }
            else
            {
                foreach (UnitsMovement obj in selectedUnits)
                {
                    obj._selected = false;
                    obj.circle.SetActive(false);
                }
                selectedUnits.Clear();

                selectedUnits.Add(this);
                _selected = true;
                circle.SetActive(true);
                layerManager.UpdateLayers();
            }
        }
    }

    public void UpdateSelectionState()
    {
        if (_selected)
        {
            if (!selectedUnits.Contains(this) && selectedUnits.Count < 3)
            {
                selectedUnits.Add(this);
            }
            circle.SetActive(true);
        }
        else
        {
            selectedUnits.Remove(this);
            circle.SetActive(false);
        }
        layerManager.UpdateLayers();
    }

    public bool IsSelected()
    {
        return _selected;
    }
}
