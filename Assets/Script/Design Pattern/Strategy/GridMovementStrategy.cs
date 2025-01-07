using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementStrategy : IMovementStrategy
{
    private readonly MonoBehaviour owner;
    private readonly IPathFinder pathfinder;
    private readonly float moveSpeed;

    private List<Vector2Int> currentPath;
    private int currentPathIndex;
    private Coroutine moveCoroutine;

    public bool IsMoving => moveCoroutine != null;
    public event System.Action OnDestinationReached;

    public GridMovementStrategy(MonoBehaviour owner, IPathFinder pathfinder, float moveSpeed)
    {
        this.owner = owner;
        this.pathfinder = pathfinder;
        this.moveSpeed = moveSpeed;
    }


    public void MoveTo(Vector2Int target)
    {
        Debug.Log("Start Move To");
        if (IsMoving)
        {
            owner.StopCoroutine(moveCoroutine);
        }

        /*var start = new Vector2Int(
            Mathf.RoundToInt(owner.transform.position.x),
            Mathf.RoundToInt(owner.transform.position.y)
        );*/
        var start = GridManager.Instance.GetGrid().GetXY(owner.transform.position);
        bool[,] matrix = GridManager.Instance.GetGridValueMatrix();

        //currentPath = pathfinder.FindPath(start, target);
        Debug.Log("begin path finding");
        try
        {
            currentPath = PathFindingDll.FindPath(start, target, matrix);
        }
        catch (Exception ex)
        {
            Debug.LogError("PathFindingDll " + ex.Message);
        }
        
        if (currentPath != null)
        {
            currentPathIndex = 0;
            moveCoroutine = owner.StartCoroutine(FollowPath());
        }
    }
    private IEnumerator FollowPath()
    {
        while (currentPathIndex < currentPath.Count)
        {
            var targetPos = GridManager.Instance.GetGrid().GetWorldPosition(
                currentPath[currentPathIndex].x,
                currentPath[currentPathIndex].y
                );

            while (Vector3.Distance(owner.transform.position, targetPos) > 0.001f)
            {
                owner.gameObject.GetComponent<SpriteRenderer>().flipX =
                    targetPos.x <= owner.transform.position.x ? false : true;

                owner.transform.position = Vector3.MoveTowards(
                    owner.transform.position,
                    targetPos,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }

            currentPathIndex++;
        }

        moveCoroutine = null;
        OnDestinationReached?.Invoke();
    }
}
