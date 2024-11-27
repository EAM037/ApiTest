using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGrid : MonoBehaviour
{
    private Grid Grid;
    public int rows;
    public int columns;
    [SerializeField] GameObject plantPrefab;
    public int cellSpace;
    // Start is called before the first frame update
    void Start()
    {
        Grid = GetComponent<Grid>();
        for (int i = 0; i < rows;)
        {
            for (int j = 0; j < columns;)
            {
                Vector3 cellPosition = Grid.GetCellCenterLocal(new Vector3Int(i, j));
                Instantiate(plantPrefab, cellPosition, Quaternion.identity);
                j = j + cellSpace;
            }
            i = i + cellSpace;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 bottomLeftPosition = new Vector3(transform.position.x + rows / 1, 0, transform.position.y + columns / 1);
        Gizmos.DrawWireCube(bottomLeftPosition, new Vector3(rows * 2, 0, columns * 2));
    }
}
