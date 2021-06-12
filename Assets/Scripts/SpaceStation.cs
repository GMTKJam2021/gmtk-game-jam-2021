using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    [SerializeField] // Serialized for debugging
    private StationModule[,] modules;
    public Vector2Int maxStationDimensions = new Vector2Int(13, 13);
    public float moduleToGridScaleFactor = 1; // How large a modules array position is in square units

    private void Awake()
    {
        modules = new StationModule[maxStationDimensions.x, maxStationDimensions.y];
    }

    public IEnumerator AddModule(GameObject module, int x, int y)
    {
        // Convert grid coords to real coords
        Vector3 spawnPosition = new Vector3(x * moduleToGridScaleFactor, y * moduleToGridScaleFactor, 0f);

        // Spawn module
        // Add as child to this
        // Disable all physics
        // Shrink to 0
        // Start coroutine to scale up and wait for it to finish
        yield return null; // put coroutine here
        // Add any connection visuals
        // Enable all physics
        // 
    }

    public List<StationModule> GetModules(bool onlyModulesWithOpenConnections = false)
    {
        List<StationModule> fullList = GetComponentsInChildren<StationModule>(false).ToList();
        if (!onlyModulesWithOpenConnections)
        {
            return fullList;
        }

        List<StationModule> trimmedList = new List<StationModule>();
        foreach (var module in fullList)
        {
            if (module.connections != null && module.connections.Count > 0)
            {
                foreach (var connection in module.connections)
                {
                    if (connection.otherConnection == null) // If the module connection does not have a paired connection
                    {
                        trimmedList.Add(module);
                    }
                }
            }
        }

        return trimmedList;
    }
}
