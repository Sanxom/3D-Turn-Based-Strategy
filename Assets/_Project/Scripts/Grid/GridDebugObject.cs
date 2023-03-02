using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro gridDebugText;

    private GridObject gridObject;

    private void Update()
    {
        gridDebugText.text = gridObject.ToString();
    }

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }
}