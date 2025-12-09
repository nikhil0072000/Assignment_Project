using UnityEngine;

public class MouseRaycast : MonoBehaviour
{
    public Camera gameCamera;
    public LayerMask tileLayerMask = -1;
    public float raycastDistance = 100f;
    public bool showDebugRay = true;

    private TileData currentHoveredTile = null;
    private UIManager uiManager;
    private Player Player;

    void Start()
    {
        if (gameCamera == null)
            gameCamera = Camera.main;

        uiManager = FindFirstObjectByType<UIManager>();
        Player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        HandleMouseRaycast();
        HandleMouseClick();
    }

    // Check what mouse is pointing at
    private void HandleMouseRaycast()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = gameCamera.ScreenPointToRay(mousePosition);

       

        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastDistance, tileLayerMask))
        {
            TileData hitTile = hitInfo.collider.GetComponent<TileData>();

            if (hitTile != null)
                HandleTileHover(hitTile);
            else
                HandleNoTileHover();
        }
        else
        {
            HandleNoTileHover();
        }
    }

    // Handle mouse clicks for moving player
    private void HandleMouseClick()
    {
        if (Player != null && Player.IsMoving())
            return;

        TurnManager turnManager = FindFirstObjectByType<TurnManager>();
        if (turnManager != null && !turnManager.IsPlayerTurn())
            return;

        if (Input.GetMouseButtonDown(0) && currentHoveredTile != null)
            OnTileClicked(currentHoveredTile);
    }

    // When player clicks on a tile
    private void OnTileClicked(TileData clickedTile)
    {
        if (Player == null)
            return;

        int targetX = clickedTile.GetGridX();
        int targetZ = clickedTile.GetGridZ();

        if (!clickedTile.IsWalkable())
            return;

        Vector2Int currentPos = Player.GetGridPosition();

        if (targetX == currentPos.x && targetZ == currentPos.y)
            return;

        Player.MoveToPosition(targetX, targetZ);
    }

    // When mouse hovers over a tile
    private void HandleTileHover(TileData hoveredTile)
    {
        if (currentHoveredTile != hoveredTile)
        {
            if (currentHoveredTile != null)
                currentHoveredTile.OnMouseExit();

            currentHoveredTile = hoveredTile;
            currentHoveredTile.OnMouseHover();

            UpdateTileInfoUI(hoveredTile);
        }
    }

    // When mouse stops hovering
    private void HandleNoTileHover()
    {
        if (currentHoveredTile != null)
        {
            currentHoveredTile.OnMouseExit();
            currentHoveredTile = null;
            ClearTileInfoUI();
        }
    }

    // Show tile info on screen
    private void UpdateTileInfoUI(TileData tileData)
    {
        if (uiManager != null)
        {
            string positionInfo = tileData.GetPositionString();
            string tileState = "";

            if (!tileData.IsWalkable())
                tileState = "Blocked";
            else if (Player != null && !Player.IsMoving())
                tileState = " tap to move";
            else if (Player != null && Player.IsMoving())
                tileState = " moving";

            uiManager.DisplayTileInfo( tileState,positionInfo);
        }
    }

    // Clear tile info from screen
    private void ClearTileInfoUI()
    {
        if (uiManager != null)
            uiManager.ClearTileInfo();
    }

    public TileData GetCurrentHoveredTile() => currentHoveredTile;
}
