using UnityEngine;
using System.Collections.Generic;
using RTS.Units;

namespace RTS.UI
{
    /// <summary>
    /// Manages unit selection and multi-select capabilities
    /// </summary>
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance { get; private set; }

        [SerializeField] private float selectionBoxMinSize = 10f;
        private HashSet<Unit> selectedUnits = new HashSet<Unit>();
        private Vector3 selectionStartPos;
        private bool isSelecting = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            HandleSelection();
            HandleCommand();
        }

        private void HandleSelection()
        {
            // Left click to select
            if (Input.GetMouseButtonDown(0))
            {
                selectionStartPos = Input.mousePosition;
                isSelecting = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (isSelecting)
                {
                    float selectionDistance = Vector3.Distance(Input.mousePosition, selectionStartPos);
                    
                    if (selectionDistance < selectionBoxMinSize)
                    {
                        // Single unit selection
                        SelectUnitAtMouse();
                    }
                    else
                    {
                        // Box selection
                        SelectUnitsInBox();
                    }
                }
                isSelecting = false;
            }
        }

        private void HandleCommand()
        {
            // Right click to command
            if (Input.GetMouseButtonDown(1))
            {
                if (selectedUnits.Count > 0)
                {
                    Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                    
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        // Check if hit is another unit
                        Unit targetUnit = hit.collider.GetComponent<Unit>();
                        if (targetUnit != null)
                        {
                            // Attack command
                            foreach (Unit unit in selectedUnits)
                            {
                                unit.Attack(targetUnit);
                            }
                        }
                        else
                        {
                            // Move command
                            foreach (Unit unit in selectedUnits)
                            {
                                unit.MoveTo(hit.point);
                            }
                        }
                    }
                }
            }
        }

        private void SelectUnitAtMouse()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            
            // Deselect all if not holding shift
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                DeselectAll();
            }

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Unit unit = hit.collider.GetComponent<Unit>();
                if (unit != null)
                {
                    SelectUnit(unit);
                }
            }
        }

        private void SelectUnitsInBox()
        {
            // TODO: Implement box selection
            // Create a screen-space box and select all units within it
            Debug.Log("Box selection not yet implemented");
        }

        public void SelectUnit(Unit unit)
        {
            if (unit == null) return;
            
            selectedUnits.Add(unit);
            unit.Select();
        }

        public void DeselectUnit(Unit unit)
        {
            if (unit == null) return;
            
            selectedUnits.Remove(unit);
            unit.Deselect();
        }

        public void DeselectAll()
        {
            foreach (Unit unit in selectedUnits)
            {
                unit.Deselect();
            }
            selectedUnits.Clear();
        }

        public int SelectedUnitCount => selectedUnits.Count;
        public IEnumerable<Unit> SelectedUnits => selectedUnits;
    }
}
