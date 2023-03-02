using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

    private const string IS_WALKING = "IsWalking";

    private Vector3 targetPosition;
    private GridPosition gridPosition;
    private float moveSpeed = 4f;
    private float rotateSpeed = 10f;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveSpeed * Time.deltaTime * moveDirection;
            
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            unitAnimator.SetBool(IS_WALKING, true);
        }
        else
            unitAnimator.SetBool(IS_WALKING, false);

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if(newGridPosition != gridPosition)
        {
            // Unit changed Grid Position
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}