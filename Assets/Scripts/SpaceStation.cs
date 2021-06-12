using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    [SerializeField] // Serialized for debugging
    private StationModule[,] modules;
    public GameObject testPrefab;
    public Vector2Int maxStationDimensions = new Vector2Int(13, 13);
    public float moduleToGridScaleFactor = 1; // How large a modules array position is in square units
    public float moduleScaleInRate = 0.1f; // Percentage of module scaling per second; 0.1f = 10 seconds to fully appear

    private void Awake()
    {
        modules = new StationModule[maxStationDimensions.x, maxStationDimensions.y];

        StartCoroutine(WaitABitThenAddAModule(1));
        StartCoroutine(WaitABitThenAddAModule(2));
        StartCoroutine(WaitABitThenAddAModule(3));
        StartCoroutine(WaitABitThenAddAModule(4));
        StartCoroutine(WaitABitThenAddAModule(5));
        StartCoroutine(WaitABitThenAddAModule(6));
    }

    public IEnumerator WaitABitThenAddAModule(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        StartCoroutine(AddModule(testPrefab, (int)seconds - 1, 0));
    }

    public IEnumerator AddModule(GameObject module, int x, int y)
    {
        // Convert grid coords to real coords
        Vector3 spawnPosition = new Vector3(x * moduleToGridScaleFactor, y * moduleToGridScaleFactor, 0f);

        // Spawn module
        GameObject newModule = Instantiate(module, spawnPosition, Quaternion.identity);

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
