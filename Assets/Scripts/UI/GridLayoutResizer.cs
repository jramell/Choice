using UnityEngine;
using UnityEngine.UI;

public class GridLayoutResizer : MonoBehaviour {

	[Header("Grid Settings")]
	public int numberOfRows = 5;
	public int numberOfCols = 3;

	[Tooltip("Prefab of a cell to be contained by the grid")]
	public GameObject cellPrefab;

	[Tooltip("Rect transform the cells will belong to")]
	public RectTransform parentRect;

	[Tooltip("GridLayoutGroup the cells will belong to")]
	public GridLayoutGroup gridLayoutGroup;

	[Header("Spacing Settings")]
	/// <summary>
	/// Whether the script should automatically calculate GridLayout spacing or not
	/// </summary>
	[Tooltip("Whether the script should automatically calculate GridLayout spacing or not")]
	public bool calculateSpacing = true;

	/// <summary>
	/// Percent of the container's width that will be used for horizontal spacing
	/// </summary>
	[Tooltip("Percent of the container's width that will be used for horizontal spacing")]
	public float widthSpacingPercent = 0.1f;

	/// <summary>
	/// Percent of the container's height that will be used for vertical spacing
	/// </summary>
	[Tooltip("Percent of the container's height that will be used for vertical spacing")]
	public float heightSpacingPercent = 0.1f;

	public GameObject[,] gridCells;

	void Start() {
		gridCells = new GameObject[numberOfRows, numberOfCols];
		if(calculateSpacing) {
			CalculateSpacing(parentRect, gridLayoutGroup);
		}
		CalculateCellSize(parentRect, gridLayoutGroup);
		DictionaryWindowManager.Instance.InitializeWordSlots(gridCells);
	}

	private void CalculateSpacing(RectTransform parentRect, GridLayoutGroup gridLayout) {
		float spacingX = parentRect.rect.width * widthSpacingPercent / numberOfCols;
		float spacingY = parentRect.rect.height * heightSpacingPercent / numberOfRows;
		gridLayout.spacing = new Vector2(spacingX, spacingY);
	}

	private void CalculateCellSize(RectTransform parentRect, GridLayoutGroup gridLayout) {
		float cellWidth = parentRect.rect.width / numberOfCols - gridLayout.spacing.x;
		float cellHeight = parentRect.rect.height / numberOfRows - gridLayout.spacing.y;
		gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
		for (int i = 0; i < numberOfRows; i++) {
			for (int j = 0; j < numberOfCols; j++) {
				GameObject wordSlot = Instantiate(cellPrefab);
				wordSlot.transform.SetParent(parentRect.gameObject.transform, false);
				gridCells[i,j] = wordSlot;
			}
		}
	}
}
