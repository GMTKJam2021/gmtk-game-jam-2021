using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    private StationModule[,] modules;
    public GameObject[] stationModulePrefabs;
    public Vector2Int maxStationDimensions = new Vector2Int(13, 13);
    public float moduleToGridScaleFactor = 1; // How large a modules array position is in square units
    public Vector3 gridOffset = new Vector3(-6.5f, -6.5f); // Offset for the grid
    public float moduleScaleInRate = 0.1f; // Percentage of module scaling per second; 0.1f = 10 seconds to fully appear

    private void Awake()
    {
        modules = new StationModule[maxStationDimensions.x, maxStationDimensions.y];

        // StartCoroutine(WaitABitThenAddAModule(1));
        // StartCoroutine(WaitABitThenAddAModule(2));
        // StartCoroutine(WaitABitThenAddAModule(3));
        // StartCoroutine(WaitABitThenAddAModule(4));
        // StartCoroutine(WaitABitThenAddAModule(5));
        // StartCoroutine(WaitABitThenAddAModule(6));

        for (var i = 0; i < 30; i++)
        {
            StartCoroutine(WaitABitThenAddAModule((i - 1) / 2 + 1));
        }
    }

    public IEnumerator WaitABitThenAddAModule(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Vector2Int spot = FindValidModuleSpot();

        yield return StartCoroutine(AddModule(stationModulePrefabs[Random.Range(0, stationModulePrefabs.Length)], spot.x, spot.y));
        Debug.Log("Module Done");
    }

    public Vector2Int FindValidModuleSpot()
    {
        // Make a list of all empty spots adjacent to existing modules and their neighbor count
        List<(int, int)> validSpots = new List<(int, int)>();

        // Check for 4 neighbor cells
        for (var y = 0; y < modules.GetLength(1); y++)
        {
            for (var x = 0; x < modules.GetLength(0); x++)
            {
                if (modules[x, y] != null)
                    continue;

                if (GetNeighborCount(x, y) < 4)
                    continue;

                validSpots.Add((x, y));
            }
        }

        // Check for 3 neighbor cells
        if (validSpots.Count < 1)
        {
            for (var y = 0; y < modules.GetLength(1); y++)
            {
                for (var x = 0; x < modules.GetLength(0); x++)
                {
                    if (modules[x, y] != null)
                        continue;

                    if (GetNeighborCount(x, y) < 3)
                        continue;

                    validSpots.Add((x, y));
                }
            }
        }

        // Check for 2 neighbor cells
        if (validSpots.Count < 1)
        {
            for (var y = 0; y < modules.GetLength(1); y++)
            {
                for (var x = 0; x < modules.GetLength(0); x++)
                {
                    if (modules[x, y] != null)
                        continue;

                    if (GetNeighborCount(x, y) < 2)
                        continue;

                    validSpots.Add((x, y));
                }
            }
        }

        // Check for 1 neighbor cells
        if (validSpots.Count < 1)
        {
            for (var y = 0; y < modules.GetLength(1); y++)
            {
                for (var x = 0; x < modules.GetLength(0); x++)
                {
                    if (modules[x, y] != null)
                        continue;

                    if (GetNeighborCount(x, y) < 1)
                        continue;

                    Debug.Log(GetNeighborCount(x, y));
                    validSpots.Add((x, y));
                }
            }

            foreach (var item in validSpots)
            {
                Debug.Log(item);
            }
        }

        // If all else fails, stick it in the middle
        if (validSpots.Count < 1)
        {
            Debug.Assert(modules[maxStationDimensions.x / 2, maxStationDimensions.y / 2] == null);

            // This is intentionally integer division
            validSpots.Add((maxStationDimensions.x / 2, maxStationDimensions.y / 2));
        }

        // Pick a random spot
        (int, int) selectedSpot = validSpots[Random.Range(0, validSpots.Count)];

        return new Vector2Int(selectedSpot.Item1, selectedSpot.Item2);
    }

    public int GetNeighborCount(int moduleX, int moduleY)
    {
        int count = 0;
        bool debug = false;
        for (var y = -1; y < 2; y++)
        {
            for (var x = -1; x < 2; x++)
            {
                // Debug.Log("X: " + (x + moduleX) + " | Y: " + (y + moduleY));
                if (x + moduleX < 0 || y + moduleY < 0 || x + moduleX >= maxStationDimensions.x || y + moduleY >= maxStationDimensions.y)
                {
                    continue; // don't draw outside the lines
                }
                // Debug.Log("X: " + (x + moduleX) + " | Y: " + (y + moduleY));

                if (x != 0 && y != 0) // ignore diagonals for now
                {
                    continue;
                }
                // Debug.Log("X: " + (x + moduleX) + " | Y: " + (y + moduleY));

                if (modules[x + moduleX, y + moduleY] == null) // no neighbor there
                {
                    continue;
                }
                // Debug.Log("X: " + (x + moduleX) + " | Y: " + (y + moduleY));

                if (!modules[x + moduleX, y + moduleY].CheckIfCanConnectTo(moduleX + x, moduleY + y, moduleX, moduleY)) // if no valid connector
                {
                    Debug.Log("fail");
                    continue;
                }
                Debug.Log("success");

                // Debug.Log("X: " + (x + moduleX) + " | Y: " + (y + moduleY));
                // Debug.Break();

                // if everything works, increase the neighbor count
                count++;
                Debug.Log("Count Up! " + count);
                debug = true;
            }
        }
        if (debug)
            Debug.Log("Done counting for this one.");
        return count;
    }

    public IEnumerator AddModule(GameObject module, int x, int y)
    {

        // Convert grid coords to real coords
        Vector3 spawnPosition = new Vector3(x * moduleToGridScaleFactor, y * moduleToGridScaleFactor, 0f) + gridOffset;

        // Spawn module
        GameObject newModule = Instantiate(module, spawnPosition, Quaternion.identity);

        // Add to modules[,]
        modules[x, y] = newModule.GetComponent<StationModule>();

        // Add as child to this
        newModule.transform.parent = transform;

        // Disable all physics
        Rigidbody2D newRb = newModule.GetComponent<Rigidbody2D>();
        Collider2D newCol = newModule.GetComponent<Collider2D>();
        newRb.simulated = false;
        newCol.enabled = false;

        // Shrink to 0
        newModule.transform.localScale = Vector3.zero;

        // Start coroutine to scale up and wait for it to finish
        yield return StartCoroutine(ScaleModuleIntoPlace(newModule.transform));

        // Add any connection visuals
        // TO-DO: Add any visuals for this?

        // Enable all physics
        newRb.simulated = true; // May not be needed?
        newCol.enabled = true;

        // Done?
    }

    public IEnumerator ScaleModuleIntoPlace(Transform module)
    {
        float newScale = 0f;
        while (module.localScale != Vector3.one)
        {
            newScale += moduleScaleInRate * Time.deltaTime;
            if (newScale > 1f)
            {
                newScale = 1f;
            }
            module.localScale = new Vector3(newScale, newScale, 1f);
            yield return null;
        }
    }
}
