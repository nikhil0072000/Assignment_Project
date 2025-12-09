# Humanized Function Names - Assignment Project

## Overview
All scripts have been rewritten with beginner-friendly, humanized function names that read like natural English sentences. This makes the code much easier to understand for new game developers.

## Script Changes Summary

### 1. Player_Humanized.cs
**Original → Humanized**
- `InitializeUnit()` → `SetupMyCharacter()`
- `MoveAlongPath()` → `WalkToDestination()`
- `NotifyMovementComplete()` → `TellEveryoneIFinishedMoving()`

### 2. Enemy_Humanized.cs
**Original → Humanized**
- `InitializeUnit()` → `SetupMyCharacter()`
- `MoveAlongPath()` → `WalkToDestination()`
- `NotifyMovementComplete()` → `TellEveryoneIFinishedMoving()`
- `ExecuteAI()` → `MakeMyBrainThink()`
- `SetAITarget()` → `TellMyBrainWhoToChase()`
- `CanAct()` → `AmIAllowedToDoSomething()`
- `UpdatePositionToGrid()` → `MoveToMyGridSpot()`

### 3. EnemyAI_Humanized.cs
**Original → Humanized**
- `ExecuteAI()` → `ThinkAndDecideWhatToDo()`
- `FindBestAdjacentTile()` → `FindBestSpotNextToPlayer()`
- `IsValidTarget()` → `CanIWalkToThisSpot()`
- `HasPlayerMoved()` → `DidPlayerMoveToNewSpot()`
- `CanAct()` → `AmIAllowedToAct()`
- `SetTarget()` → `SetWhoIAmChasing()`
- `GetTarget()` → `GetWhoIAmChasing()`
- `ResetAI()` → `ResetMyBrainToStart()`
- `OnMovementComplete()` → `IFinishedMovingNow()`

**Variables:**
- `targetDistance` → `howCloseIWantToBe`
- `decisionDelay` → `howLongToWaitBeforeThinking`
- `currentTarget` → `whoAmIChasing`
- `enemy` → `myEnemyBody`
- `isProcessing` → `amICurrentlyThinking`
- `waitingForPlayerMove` → `amIWaitingForPlayerToMove`
- `lastPlayerPosition` → `whereWasPlayerLastTime`

### 4. CharacterMovement_Humanized.cs
**Original → Humanized**
- `InitializeUnit()` → `SetupMyCharacter()`
- `SetGridPosition()` → `PutMeOnTheGrid()`
- `MoveToPosition()` → `WalkToThisSpot()`
- `MoveAlongPath()` → `WalkToDestination()`
- `UpdateGridPositionSilent()` → `UpdateMyGridPositionQuietly()`
- `OnMovementComplete()` → `IFinishedWalkingNow()`
- `MoveToWorldPosition()` → `SmoothlyWalkToWorldPosition()`
- `CalculateWorldPosition()` → `CalculateWhereIShouldBeInTheWorld()`
- `GetTileSurfaceHeight()` → `FindGroundHeightAtThisSpot()`
- `IsMoving()` → `AmICurrentlyMoving()`
- `GetGridPosition()` → `WhereAmIOnTheGrid()`

**Variables:**
- `gridX/gridZ` → `myGridX/myGridZ`
- `moveSpeed` → `howFastIWalk`
- `unitHeight` → `howHighAboveGroundIAm`
- `currentTile` → `theTileImStandingOn`
- `isMoving` → `amICurrentlyWalking`
- `pathfinding` → `myPathFinder`

### 5. TurnManager_Humanized.cs
**Original → Humanized**
- `FindAllEnemys()` → `FindAllEnemiesInTheGame()`
- `SetupEnemyTargets()` → `TellAllEnemiesToChaseThePlayer()`
- `OnPlayerMoveComplete()` → `PlayerJustFinishedMoving()`
- `ExecuteEnemyTurn()` → `LetAllEnemiesTakeTheirTurn()`
- `CheckEnemyTurnComplete()` → `CheckIfAllEnemiesFinishedTheirTurn()`
- `OnEnemyTurnComplete()` → `AllEnemiesFinishedTheirTurn()`
- `IsPlayerTurn()` → `IsItThePlayersTurnRightNow()`
- `AddEnemy()` → `AddNewEnemyToTheGame()`
- `RemoveEnemy()` → `RemoveEnemyFromTheGame()`

**Variables:**
- `enemyTurnDelay` → `howLongToWaitBeforeEnemiesTakeTurn`
- `Enemys` → `allTheEnemiesInTheGame`
- `Player` → `thePlayer`
- `isPlayerTurn` → `isItThePlayersTurn`

### 6. GridManager_Humanized.cs
**Original → Humanized**
- `GenerateGrid()` → `CreateTheEntireGrid()`
- `CreateTileAtPosition()` → `MakeOneTileAtThisSpot()`
- `GetTile()` → `GetTileAtThisSpot()`
- `IsValidGridPosition()` → `IsThisSpotInsideMyGrid()`

**Variables:**
- `gridWidth/gridHeight` → `howManyTilesWide/howManyTilesTall`
- `tileSize` → `howBigEachTileIs`
- `tileSpacing` → `spaceBetweenTiles`
- `tilePrefab` → `whatTileLooksLike`
- `gridParent` → `whereToputAllTiles`
- `gridTiles` → `allMyTiles`

### 7. MouseRaycast_Humanized.cs
**Original → Humanized**
- `HandleMouseRaycast()` → `CheckWhatMyMouseIsPointingAt()`
- `HandleMouseClick()` → `CheckIfPlayerClickedOnSomething()`
- `OnTileClicked()` → `PlayerClickedOnThisTile()`
- `HandleTileHover()` → `MyMouseIsHoveringOverThisTile()`
- `HandleNoTileHover()` → `MyMouseIsNotHoveringOverAnyTile()`
- `UpdateTileInfoUI()` → `ShowTileInfoOnScreen()`
- `ClearTileInfoUI()` → `ClearTileInfoFromScreen()`
- `GetCurrentHoveredTile()` → `GetTileMyMouseIsOverRightNow()`

**Variables:**
- `gameCamera` → `myCameraToUse`
- `tileLayerMask` → `whatLayersCanIClickOn`
- `raycastDistance` → `howFarCanILook`
- `showDebugRay` → `shouldIShowDebugLines`
- `currentHoveredTile` → `tileMyMouseIsOverRightNow`
- `uiManager` → `myUIController`

### 8. GridPathfinding_Humanized.cs
**Original → Humanized**
- `InitializePathfindingGrid()` → `SetupMyPathfindingGrid()`
- `UpdateWalkabilityFromTiles()` → `CheckWhichTilesICanWalkOn()`
- `FindPath()` → `FindPathFromHereToThere()`
- `GetLowestFCostNode()` → `GetNodeWithLowestCost()`
- `GetNeighbors()` → `GetNeighborsOfThisNode()`
- `CalculateDistance()` → `CalculateDistanceBetweenNodes()`
- `RetracePath()` → `BuildPathFromStartToEnd()`
- `IsValidPosition()` → `IsThisSpotValid()`

**Variables:**
- `pathNodes` → `allMyPathNodes`
- `gCost/hCost/fCost` → `costFromStart/costToEnd/totalCost`
- `parent` → `whereICameFrom`
- `isWalkable` → `canIWalkOnThis`

### 9. TileData_Humanized.cs
**Original → Humanized**
- `InitializeTile()` → `SetupThisTile()`
- `OnMouseHover()` → `MouseIsHoveringOverMe()`
- `OnMouseExit()` → `MouseStoppedHoveringOverMe()`
- `IsWalkable()` → `CanIWalkOnThis()`
- `SetOccupied()` → `SetSomeoneIsStandingOnMe()`
- `SetWalkable()` → `SetIfSomeoneCanWalkOnMe()`
- `GetGridX()/GetGridZ()` → `GetMyGridX()/GetMyGridZ()`
- `GetGridPosition()` → `GetMyGridPosition()`
- `GetPositionString()` → `GetMyPositionAsText()`

**Variables:**
- `defaultMaterial` → `whatILookLikeNormally`
- `hoverMaterial` → `whatILookLikeWhenMouseIsOverMe`
- `isWalkable` → `canSomeoneWalkOnMe`
- `isOccupied` → `isSomeoneStandingOnMe`

### 10. UIManager_Humanized.cs
**Original → Humanized**
- `DisplayTileInfo()` → `ShowTileInfoOnScreen()`
- `ClearTileInfo()` → `HideTileInfoFromScreen()`
- `UpdateGameStatus()` → `ShowGameStatusOnScreen()`

**Variables:**
- `tileInfoText` → `whereToShowTileInfo`
- `gameStatusText` → `whereToShowGameStatus`

## Key Principles Used

1. **Natural Language**: Functions read like sentences ("AmICurrentlyMoving" instead of "IsMoving")
2. **First Person Perspective**: Variables use "my" and "I" ("myGridX" instead of "gridX")
3. **Descriptive Names**: Clear purpose ("howFastIWalk" instead of "moveSpeed")
4. **Question Format**: Boolean functions as questions ("CanIWalkOnThis" instead of "IsWalkable")
5. **Action Descriptions**: Functions describe what they do ("TellEveryoneIFinishedMoving")

## Benefits for Noob Developers

- **Easier to Read**: Code reads like natural English
- **Self-Documenting**: Function names explain their purpose
- **Reduced Confusion**: Clear variable ownership with "my" prefix
- **Better Understanding**: Logical flow is more apparent
- **Learning Friendly**: Concepts are easier to grasp for beginners

## Usage

Replace the original scripts with these humanized versions to make your codebase more beginner-friendly. The functionality remains identical, only the naming has been improved for clarity and readability.
