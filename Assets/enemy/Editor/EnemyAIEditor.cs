using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// This script creates a custom button that you can use to create patrol points
[CustomEditor(typeof(EnemyAI))]
public class EnemyAIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyAI enemyAI = (EnemyAI)target;

        // Button to create a new patrol point
        if (GUILayout.Button("Create Patrol Point"))
        {
            CreatePatrolPoint(enemyAI);
        }

        // Button to update the colors of existing patrol points
        if (GUILayout.Button("Update Patrol Point Colors"))
        {
            enemyAI.UpdatePatrolPointColors();
        }
    }

    private void CreatePatrolPoint(EnemyAI enemyAI)
    {
        if (enemyAI.patrolPointPrefab != null)
        {
            // Remove null values from the list
            enemyAI.patrolPoints.RemoveAll(point => point == null);

            // If patrolPoints is null or empty, initialize the list
            if (enemyAI.patrolPoints == null || enemyAI.patrolPoints.Count == 0)
            {
                enemyAI.patrolPoints = new List<Transform>();
            }

            // Instantiate a linked prefab
            GameObject newPatrolPoint = (GameObject)PrefabUtility.InstantiatePrefab(enemyAI.patrolPointPrefab);

            // Set position, etc. as needed
            newPatrolPoint.transform.position = enemyAI.transform.position; // Adjust position based on your needs

            // Set the patrol point name
            int patrolPointNumber = enemyAI.patrolPoints.Count + 1;
            newPatrolPoint.name = $"Patrol Point ({enemyAI.gameObject.name}) ({patrolPointNumber})";

            // Set the patrol point time
            newPatrolPoint.GetComponent<PatrolPoint>().waitTime = enemyAI.waitTimeAtPatrolPointFallbackValue;

            // Add the new patrol point to the list in the EnemyAI script
            enemyAI.patrolPoints.Add(newPatrolPoint.transform);

            // Change the patrol point's color to the enemy's color variable
            Renderer patrolPointRenderer = newPatrolPoint.GetComponentInChildren<Renderer>();
            if (patrolPointRenderer != null)
            {
                patrolPointRenderer.sharedMaterial.color = enemyAI.patrolPointColor;
            }

            // Mark the enemyAI object as dirty to ensure changes are saved
            EditorUtility.SetDirty(enemyAI);
            // Mark the scene as dirty so Unity saves the changes
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(enemyAI.gameObject.scene);

            // Select the new patrol point in the hierarchy
            Selection.activeGameObject = newPatrolPoint;
        }
        else
        {
            Debug.LogWarning("Patrol Point Prefab not assigned!");
        }
    }
}
