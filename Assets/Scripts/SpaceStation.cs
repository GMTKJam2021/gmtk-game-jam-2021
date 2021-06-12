using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
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
