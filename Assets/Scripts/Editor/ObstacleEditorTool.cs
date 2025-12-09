using UnityEngine;
using UnityEditor;

public class ObstacleEditorTool : EditorWindow
{
    
    private ObstacleData obstacleData;
    
    // Size of each button in the grid interface
    private const float BUTTON_SIZE = 30f;
    
    // Colors for visual feedback
    private Color obstacleColor = Color.red;
    private Color emptyColor = Color.white;
    
   
   
    /// Found under Tools > ObstacleTool > Obstacle Editor
    
    [MenuItem("Tools/ObstacleTool/Obstacle Editor")]
    public static void ShowWindow()
    {
        // Create and show the editor window
        ObstacleEditorTool window = GetWindow<ObstacleEditorTool>();
        window.titleContent = new GUIContent("Obstacle Editor");
        window.minSize = new Vector2(400, 500);
    }
    
    
    void OnGUI()
    {
        // Title and instructions
        EditorGUILayout.LabelField("Obstacle Editor Tool", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Select an ObstacleData asset and use the grid below to place obstacles. Red buttons = obstacles, White buttons = empty tiles.", MessageType.Info);
        EditorGUILayout.Space();
        
        // Object field to select the ObstacleData asset
        ObstacleData newObstacleData = (ObstacleData)EditorGUILayout.ObjectField("Obstacle Data", obstacleData, typeof(ObstacleData), false);
        
        // Check if user selected a different asset
        if (newObstacleData != obstacleData)
        {
            obstacleData = newObstacleData;
        }
        
        EditorGUILayout.Space();
        
        // Only show the grid if we have a valid ObstacleData asset
        if (obstacleData != null)
        {
            DrawObstacleGrid();
            EditorGUILayout.Space();
            DrawgridButtons();
        }
        else
        {
            EditorGUILayout.HelpBox("Please select an ObstacleData asset to begin editing obstacles.", MessageType.Warning);
        }
    }
    
    
    void DrawObstacleGrid()
    {
        EditorGUILayout.LabelField("Grid Layout (10x10)", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Click buttons to toggle obstacles on/off");
        EditorGUILayout.Space();
        
        // Start a vertical layout for the grid rows
        EditorGUILayout.BeginVertical();
        
        // Draw each row of the grid (Z coordinates from top to bottom)
        for (int z = obstacleData.gridHeight - 1; z >= 0; z--)
        {
            // Start horizontal layout for this row
            EditorGUILayout.BeginHorizontal();
            
            // Draw each column in this row (X coordinates from left to right)
            for (int x = 0; x < obstacleData.gridWidth; x++)
            {
                DrawTileButton(x, z);
            }
            
            // End horizontal layout for this row
            EditorGUILayout.EndHorizontal();
        }
        
        // End vertical layout for the grid
        EditorGUILayout.EndVertical();
    }
    
   
    void DrawTileButton(int x, int z)
    {
        // Check current obstacle state for this tile
        bool hasObstacle = obstacleData.HasObstacle(x, z);
        
        // Set button color based on obstacle state
        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = hasObstacle ? obstacleColor : emptyColor;
        
        // Create button content with coordinates for easy identification
        string buttonText = $"{x},{z}";
        
        // Draw the button with fixed size
        if (GUILayout.Button(buttonText, GUILayout.Width(BUTTON_SIZE), GUILayout.Height(BUTTON_SIZE)))
        {
            // Toggle obstacle state when button is clicked
            obstacleData.SetObstacle(x, z, !hasObstacle);
            
            // Force Unity to repaint the scene view to show changes
            SceneView.RepaintAll();
        }
        
        // Restore original button color
        GUI.backgroundColor = originalColor;
    }
    
   
    void DrawgridButtons()
    {
        EditorGUILayout.LabelField("Utility Functions", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        
        // Clear all obstacles button
        if (GUILayout.Button("Clear All Obstacles"))
        {
            if (EditorUtility.DisplayDialog("Clear All Obstacles", 
                "Are you sure you want to remove all obstacles from the grid?", 
                "Yes", "Cancel"))
            {
                obstacleData.ClearAllObstacles();
                SceneView.RepaintAll();
            }
        }
        
        // Show obstacle count
        int obstacleCount = obstacleData.GetObstacleCount();
        EditorGUILayout.LabelField($"Total Obstacles: {obstacleCount}");
        
        EditorGUILayout.EndHorizontal();
    }
}
