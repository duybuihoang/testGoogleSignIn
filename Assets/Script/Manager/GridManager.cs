using UnityEngine;

public class GridManager : BaseMonobehavior
{
    [SerializeField] private float targetAspectRatio = 0.5625f; // 9:16
    [SerializeField] private int preferredGridWidth = 20; // Adjust this based on your game design

    private Grid<IGridObject> gameGrid; 
    private int gridWidth;
    private int gridHeight;
    private float cellSize;
    private Vector3 originPosition;

    private static GridManager instance;
    public static GridManager Instance => instance;

    protected override void Awake()
    {
        base.Awake();

        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
            return;
        }

        CalculateGridDimensions();
        InitializeGrid();
    }

    private void CalculateGridDimensions()
    {
        // Get screen dimensions in world units
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        // Calculate cell size based on preferred grid width
        cellSize = screenWidth / preferredGridWidth;

        // Calculate grid dimensions
        gridWidth = preferredGridWidth;
        gridHeight = Mathf.FloorToInt(screenHeight / cellSize);
        Debug.Log(gridWidth);
        Debug.Log(gridHeight);

        // Calculate origin position to center the grid
        originPosition = new Vector3(
            -screenWidth / 2f + cellSize/2,
            (-screenHeight/ 2f) + cellSize * 3,
            0f
        );
        Debug.Log(originPosition);
    }
    private void InitializeGrid()
    {
        gameGrid = new Grid<IGridObject>(gridWidth, gridHeight, cellSize, originPosition);
    }

    public bool[,] GetGridValueMatrix()
    {
        int height = gridHeight;
        int width = gridWidth;

        bool[,] matrix = new bool[width + 1, height + 1];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                matrix[i, j] = gameGrid.GetValue(i, j) == null ? false : true;
            }
        }
        return matrix;
    }

    public Grid<IGridObject> GetGrid() => gameGrid;
    public float GetCellSize() => cellSize;
    public Vector3 GetOriginPosition() => originPosition;
    public int GetGridWidth() => gridWidth;
    public int GetGridHeight() => gridHeight;
}
