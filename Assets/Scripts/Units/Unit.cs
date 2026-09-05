using UnityEngine;
using UnityEngine.AI;

namespace RTS.Units
{
    /// <summary>
    /// Base unit class - represents a single controllable unit
    /// </summary>
    public class Unit : MonoBehaviour
    {
        [Header("Unit Stats")]
        [SerializeField] private int unitId;
        [SerializeField] private string unitName = "Unit";
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackRange = 5f;
        [SerializeField] private float moveSpeed = 5f;

        [Header("Components")]
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Renderer unitRenderer;

        private float currentHealth;
        private bool isSelected = false;

        private void Start()
        {
            currentHealth = maxHealth;
            
            if (navMeshAgent == null)
            {
                navMeshAgent = GetComponent<NavMeshAgent>();
            }
            
            if (unitRenderer == null)
            {
                unitRenderer = GetComponent<Renderer>();
            }

            if (navMeshAgent != null)
            {
                navMeshAgent.speed = moveSpeed;
            }
        }

        private void Update()
        {
            // Update animations, check if reached destination, etc.
        }

        /// <summary>
        /// Move unit to target position
        /// </summary>
        public void MoveTo(Vector3 targetPosition)
        {
            if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(targetPosition);
            }
        }

        /// <summary>
        /// Attack a target unit
        /// </summary>
        public void Attack(Unit targetUnit)
        {
            if (targetUnit == null) return;
            
            float distanceToTarget = Vector3.Distance(transform.position, targetUnit.transform.position);
            
            if (distanceToTarget <= attackRange)
            {
                targetUnit.TakeDamage(attackDamage);
            }
            else
            {
                // Move to attack range first
                MoveTo(targetUnit.transform.position);
            }
        }

        /// <summary>
        /// Deal damage to this unit
        /// </summary>
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Select this unit visually
        /// </summary>
        public void Select()
        {
            isSelected = true;
            SetSelectionHighlight(true);
        }

        /// <summary>
        /// Deselect this unit
        /// </summary>
        public void Deselect()
        {
            isSelected = false;
            SetSelectionHighlight(false);
        }

        private void SetSelectionHighlight(bool highlighted)
        {
            if (unitRenderer != null)
            {
                // TODO: Implement visual selection feedback
                // Options: Change color, add outline, add glow, etc.
            }
        }

        private void Die()
        {
            Debug.Log($"{unitName} died!");
            Destroy(gameObject);
        }

        public bool IsSelected => isSelected;
        public float Health => currentHealth;
        public float MaxHealth => maxHealth;
        public float AttackRange => attackRange;
        public string UnitName => unitName;
    }
}
