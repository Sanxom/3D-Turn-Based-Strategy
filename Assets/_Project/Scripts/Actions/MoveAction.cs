using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;

    private const string IS_WALKING = "IsWalking";

    private Vector3 targetPosition;
    private float moveSpeed = 4f;
    private float rotateSpeed = 10f;
    private float stoppingDistance = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive)
            return;

        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveSpeed * Time.deltaTime * moveDirection;

            unitAnimator.SetBool(IS_WALKING, true);
        }
        else
        {
            unitAnimator.SetBool(IS_WALKING, false);
            isActive = false;
            OnActionComplete();
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // Outside of Grid bounds
                    continue;
                }

                if(unitGridPosition == testGridPosition)
                {
                    // Same Grid Position where unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Has a unit on this Grid Position
                    continue;
                }

                // Found a valid Grid Position
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete)
    {
        this.OnActionComplete = OnActionComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }
}