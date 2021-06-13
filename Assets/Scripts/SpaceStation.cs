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

        StartCoroutine(WaitABitThenAddAModule(0.5f));
        StartCoroutine(WaitABitThenAddAModule(1f));
        StartCoroutine(WaitABitThenAddAModule(1.5f));
        StartCoroutine(WaitABitThenAddAModule(2f));
        StartCoroutine(WaitABitThenAddAModule(2.5f));
        StartCoroutine(WaitABitThenAddAModule(3f));
        StartCoroutine(WaitABitThenAddAModule(3.5f));
        StartCoroutine(WaitABitThenAddAModule(4f));
        StartCoroutine(WaitABitThenAddAModule(4.5f));

        for (var i = 0; i < 30; i++)
        {
            StartCoroutine(WaitABitThenAddAModule(6 * i));
        }
    }

    public IEnumerator WaitABitThenAddAModule(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Vector2Int spot = FindValidModuleSpot();

        // GameObject newModule = stationModulePrefabs[Random.Range(0, stationModulePrefabs.Length)];
        GameObject newModule = PickModuleForSpot(spot);

        yield return StartCoroutine(AddModule(newModule, spot.x, spot.y));
    }

    public GameObject PickModuleForSpot(Vector2Int spot)
    {
        // If center, return first item on list(core)
        if (spot.x == maxStationDimensions.x / 2 && spot.y == maxStationDimensions.y / 2)
        {
            return stationModulePrefabs[0];
        }

        // Gather list of all possible modules
        List<StationModule> moduleList = new List<StationModule>();
        foreach (var go in stationModulePrefabs)
        {
            moduleList.Add(go.GetComponent<StationModule>());
        }

        // First, check if it has core neighbors (including diagonals)
        // TO-DO: Consider discarding diagonals and restarting spot search?
        StationModule.CoreSection coreSection = CheckIfValidCoreSection(spot);

        // If core, figure out which core piece fits and return that
        if (coreSection != StationModule.CoreSection.NotCore)
        {
            foreach (var module in moduleList)
            {
                if (module.coreSection == coreSection)
                {
                    return module.gameObject;
                }
            }
        }
        else
        {
            moduleList.RemoveAll(x => x.coreSection != StationModule.CoreSection.NotCore);
        }

        // Check if it is connecting to a brace connection
        // If brace, narrow list to braces; otherwise, remove braces
        // Count that spot's neighbors and return a module with that many sides if possible; continue with a narrowed-down list
        // Check the type of module that the player has the least of; return one of those if possible
        // If still undecided, pick randomly from remaining list
        return moduleList[Random.Range(0, moduleList.Count)].gameObject;
    }



    public StationModule.CoreSection CheckIfValidCoreSection(Vector2Int spot)
    {
        int x = -1;
        int y = -1;
        if (!(spot.x + x < 0 || spot.y + y < 0 || spot.x + x >= maxStationDimensions.x || spot.y + y >= maxStationDimensions.y) && modules[spot.x + x, spot.y + y] != null)
        {
            if (modules[spot.x + x, spot.y + y].isCenterCore)
            {
                return StationModule.CoreSection.TopRight;
            }
        }

        x = 0;
        if (!(spot.x + x < 0 || spot.y + y < 0 || spot.x + x >= maxStationDimensions.x || spot.y + y >= maxStationDimensions.y) && modules[spot.x + x, spot.y + y] != null)
        {
            if (modules[spot.x + x, spot.y + y].isCenterCore)
            {
                return StationModule.CoreSection.TopCenter;
            }
        }

        x = 1;
        if (!(spot.x + x < 0 || spot.y + y < 0 || spot.x + x >= maxStationDimensions.x || spot.y + y >= maxStationDimensions.y) && modules[spot.x + x, spot.y + y] != null)
        {
            if (modules[spot.x + x, spot.y + y].isCenterCore)
            {
                return StationModule.CoreSection.TopLeft;
            }
        }

        x = -1;
        y = 0;
        if (!(spot.x + x < 0 || spot.y + y < 0 || spot.x + x >= maxStationDimensions.x || spot.y + y >= maxStationDimensions.y) && modules[spot.x + x, spot.y + y] != null)
        {
            if (modules[spot.x + x, spot.y + y].isCenterCore)
            {
                return StationModule.CoreSection.CenterRight;
            }
        }

        x = 0;
        if (!(spot.x + x < 0 || spot.y + y < 0 || spot.x + x >= maxStationDimensions.x || spot.y + y >= maxStationDimensions.y) && modules[spot.x + x, spot.y + y] != null)
        {
            if (modules[spot.x + x, spot.y + y].isCenterCore)
            {
                Debug.LogError("Something has gone horribly wrong if you reached this error. The game should probably be over because the core center needed replacing.");
                return StationModule.CoreSection.Center;
            }
        }

        x = 1;
        if (!(spot.x + x < 0 || spot.y + y < 0 || spot.x + x >= maxStationDimensions.x || spot.y + y >= maxStationDimensions.y) && modules[spot.x + x, spot.y + y] != null)
        {
            if (modules[spot.x + x, spot.y + y].isCenterCore)
            {
                return StationModule.CoreSection.CenterLeft;
            }
        }

        x = -1;
        y = 1;
        if (!(spot.x + x < 0 || spot.y + y < 0 || spot.x + x >= maxStationDimensions.x || spot.y + y >= maxStationDimensions.y) && modules[spot.x + x, spot.y + y] != null)
        {
            if (modules[spot.x + x, spot.y + y].isCenterCore)
            {
                return StationModule.CoreSection.BottomRight;
            }
        }

        x = 0;
        if (!(spot.x + x < 0 || spot.y + y < 0 || spot.x + x >= maxStationDimensions.x || spot.y + y >= maxStationDimensions.y) && modules[spot.x + x, spot.y + y] != null)
        {
            if (modules[spot.x + x, spot.y + y].isCenterCore)
            {
                return StationModule.CoreSection.BottomCenter;
            }
        }

        x = 1;
        if (!(spot.x + x < 0 || spot.y + y < 0 || spot.x + x >= maxStationDimensions.x || spot.y + y >= maxStationDimensions.y) && modules[spot.x + x, spot.y + y] != null)
        {
            if (modules[spot.x + x, spot.y + y].isCenterCore)
            {
                return StationModule.CoreSection.BottomLeft;
            }
        }

        return StationModule.CoreSection.NotCore;
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

                    // Debug.Log(GetNeighborCount(x, y));
                    validSpots.Add((x, y));
                }
            }

            foreach (var item in validSpots)
            {
                // Debug.Log(item);
            }
        }

        // If all else fails, stick it in the middle
        if (validSpots.Count < 1)
        {
            Debug.Assert(modules[maxStationDimensions.x / 2, maxStationDimensions.y / 2] == null);

            // This is intentionally integer division
            validSpots.Add((maxStationDimensions.x / 2, maxStationDimensions.y / 2));
        }

        // Remove spots that are not reciprocal


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

                if (!modules[moduleX + x, moduleY + y].CheckIfCanConnectTo(moduleX, moduleY, moduleX + x, moduleY + y)) // if no valid connector
                {
                    // Debug.Log("fail");
                    continue;
                }

                // Debug.Log("success");

                // Debug.Log("X: " + (x + moduleX) + " | Y: " + (y + moduleY));
                // Debug.Break();

                // if everything works, increase the neighbor count
                count++;
                // Debug.Log("Count Up! " + count);
                debug = true;
            }
        }
        // if (debug)
        // Debug.Log("Done counting for this one.");
        return count;
    }

    public List<(int, int)> GetNeighborsWithConnections(int moduleX, int moduleY, List<StationConnection> connections)
    {
        List<(int, int)> validNeighbors = new List<(int, int)>();
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

                if (!modules[moduleX + x, moduleY + y].CheckIfCanConnectTo(moduleX, moduleY, moduleX + x, moduleY + y)) // if no valid connector
                {
                    continue;
                }
                Debug.Log("X: " + (x + moduleX) + " | Y: " + (y + moduleY));

                foreach (var c in connections)
                {
                    if (c.connectedModuleGridLocation.x + moduleX == x && c.connectedModuleGridLocation.y + moduleY == y)
                    {
                        // Debug.Log("X: " + (x + moduleX) + " | Y: " + (y + moduleY));
                        validNeighbors.Add((x + moduleX, y + moduleY));
                    }
                    // Debug.Log("nope");
                }
                validNeighbors.Add((x + moduleX, y + moduleY));
                // Debug.Log("nope over");
            }
        }

        return validNeighbors;
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

        // Rotate to match random connection to connection target
        // but not if core section
        if (modules[x, y].coreSection == StationModule.CoreSection.NotCore)
        {
            List<float> thisAngle = new List<float>();
            Debug.Log("outangles");
            foreach (var connection in modules[x, y].connections)
            {
                thisAngle.Add(connection.outDirectionAngle);
                // Debug.Log(connection.outDirectionAngle);
            }
            // StationConnection randomConnection = modules[x, y].connections[Random.Range(0, modules[x, y].connections.Count)];
            List<(int, int)> validNeighbors = GetNeighborsWithConnections(x, y, modules[x, y].connections);
            // float thisAngle = randomConnection.outDirectionAngle;
            Debug.Log("neigh");
            foreach (var n in validNeighbors)
            {
                Debug.Log(n);
            }
            Debug.Log("end neigh");

            // This list will contain all the directions where a connection is valid
            List<float> targetAngle = new List<float>();
            foreach (var neighbor in validNeighbors)
            {
                if (neighbor.Item1 < x)
                {
                    targetAngle.Add(180);
                    Debug.Log(180);
                }
                else if (neighbor.Item1 > x)
                {
                    targetAngle.Add(0);
                    Debug.Log(0);
                }
                else if (neighbor.Item2 < y)
                {
                    targetAngle.Add(270);
                    Debug.Log(270);
                }
                else if (neighbor.Item2 > y)
                {
                    targetAngle.Add(90);
                    Debug.Log(90);
                }
            }

            int indexOfBestRotation = 0;
            int highestCount = 0;
            List<int> additionalBestIndicies = new List<int>();
            for (var i = 0; i < 4; i++)
            {
                int count = 0;
                // Debug.Log(i + ", Count: " + count);
                foreach (var angle in thisAngle)
                {
                    foreach (var other in targetAngle)
                    {
                        // Debug.Log("Angle: " + angle + " | Other: " + other);
                        if ((angle + i * 90) % 360 == other)
                        {
                            // Debug.Log("true A " + angle + i * 90);
                            count++;
                        }
                    }
                }

                if (count > highestCount)
                {
                    // Debug.Log("true B " + count);
                    indexOfBestRotation = i;
                    highestCount = count;
                    additionalBestIndicies.Clear();
                }
                else if (count == highestCount)
                {
                    additionalBestIndicies.Add(i);
                }
            }
            additionalBestIndicies.Add(indexOfBestRotation);
            // Debug.Assert(highestCount > 0); // If this doesn't pass, the logic is horribly broken somewhere (Narrator from the future: it was horribly broken)
            // Debug.Log("Rotation should be " + indexOfBestRotation * )
            // Rotate to make indexOfBestRotation
            indexOfBestRotation = additionalBestIndicies[Random.Range(0, additionalBestIndicies.Count)];
            newModule.transform.eulerAngles = new Vector3(0f, 0f, 90f * indexOfBestRotation);

            // Rotate the connection points
            int a = 90 * indexOfBestRotation;
            foreach (var connection in modules[x, y].connections)
            {
                Vector2 temp = Helper.RotateVector2ByDegree(connection.connectedModuleGridLocation, a);
                connection.connectedModuleGridLocation = new Vector2Int((int)temp.x, (int)temp.y);
                connection.outDirectionAngle = (connection.outDirectionAngle + a) % 360;
            }
        }



        // Disable all physics
        //Rigidbody2D newRb = newModule.GetComponent<Rigidbody2D>();
        Collider2D newCol = newModule.GetComponent<Collider2D>();
        //newRb.simulated = false;
        newCol.enabled = false;
        newCol.usedByComposite = false;

        // Shrink to 0
        newModule.transform.localScale = Vector3.zero;

        // Start coroutine to scale up and wait for it to finish
        yield return StartCoroutine(ScaleModuleIntoPlace(newModule.transform));

        // Add any connection visuals
        // TO-DO: Add any visuals for this?

        // Enable all physics
        //newRb.simulated = true; // May not be needed?
        newCol.usedByComposite = true;
        newCol.enabled = true;

        GetComponent<CompositeCollider2D>().GenerateGeometry();

        // Done?
        Debug.Log("Module Done");

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
