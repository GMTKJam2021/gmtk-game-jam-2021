using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Heavily references this tutorial series: 
// https://www.raywenderlich.com/348-make-a-2d-grappling-hook-game-in-unity-part-1
// https://www.raywenderlich.com/312-make-a-2d-grappling-hook-game-in-unity-part-2
public class TetherSystem : MonoBehaviour
{
    public GameObject tetherHingeAnchor;
    public DistanceJoint2D tetherJoint;
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    private bool tetherAttached;
    private Vector2 playerPosition;
    private Rigidbody2D tetherHingeAnchorRb;
    private SpriteRenderer tetherHingeAnchorSprite;
    public LineRenderer tetherRenderer;
    public LayerMask tetherLayerMask;
    public LayerMask connectionLayerMask;
    [SerializeField]
    private float tetherMaxCastDistance = 20f;
    [SerializeField]
    private float tetherMaxLength = 30f;
    [SerializeField]
    private float tetherLength = 0f;
    private List<Vector2> tetherPositions = new List<Vector2>();
    private bool distanceSet;
    private Dictionary<(Vector2, int), int> wrapPointsLookup = new Dictionary<(Vector2, int), int>();
    private int wrapLoop = 0; // This is the number of times looped around, used with wrapPointsLookup to handle multiple loops around an object
    public float climbSpeed = 3f;
    private bool isColliding;
    public OxygenTank oxygenTank;

    void Awake()
    {
        tetherJoint.enabled = false;
        playerPosition = transform.position;
        tetherHingeAnchorRb = tetherHingeAnchor.GetComponent<Rigidbody2D>();
        tetherHingeAnchorSprite = tetherHingeAnchor.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        Vector3 facingDirection = worldMousePosition - transform.position;
        float aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

        // Check direction of angle
        //Debug.DrawRay(transform.position, aimDirection, Color.red);
        playerPosition = transform.position;

        if (!tetherAttached)
        {
            SetCrosshairPosition(aimAngle);
        }
        else
        {
            crosshairSprite.enabled = false;

            if (tetherPositions.Count > 0)
            {
                var lastRopePoint = tetherPositions.Last();
                var playerToCurrentNextHit = Physics2D.Raycast(playerPosition, (lastRopePoint - playerPosition).normalized, Vector2.Distance(playerPosition, lastRopePoint) - 0.1f, tetherLayerMask);

                if (playerToCurrentNextHit)
                {
                    var colliderWithVertices = playerToCurrentNextHit.collider as CompositeCollider2D;
                    if (colliderWithVertices != null)
                    {
                        try
                        {
                            var closestPointToHit = GetClosestColliderPointFromRaycastHit(playerToCurrentNextHit, colliderWithVertices);

                            if (wrapPointsLookup.ContainsKey((closestPointToHit, wrapLoop)))
                            {
                                // Removed to permit multiple loops
                                // ResetTether();
                                // return;

                                wrapLoop += 1;
                            }

                            tetherPositions.Add(closestPointToHit);
                            wrapPointsLookup.Add((closestPointToHit, wrapLoop), 0);
                            distanceSet = false;
                        }
                        catch (Exception e) // TO-DO: Hack to prevent crash on new module addition causing issues
                        {
                            ResetTether();
                            return;
                        }

                    }
                }
            }
        }

        HandleInput(aimDirection);
        UpdateTetherPositions();
        HandleTetherLength();
        HandleTetherUnwrap();
        HandleOxygenFlow();
    }

    private void HandleOxygenFlow()
    {
        if (tetherAttached)
        {
            oxygenTank.ReplenishOxygen(Time.deltaTime);
        }
    }

    private void SetCrosshairPosition(float aimAngle)
    {
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }

        var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
        var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }

    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetButtonDown("Oxygen Toggle"))
        {
            // If the tether is attached, reset it
            if (tetherAttached)
            {
                Debug.Log("Detach tether");
                ResetTether();
                return;
            }
            // Otherwise, attach it if possible
            Debug.Log("Attaching tether");
            tetherRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, aimDirection, tetherMaxCastDistance, connectionLayerMask);

            if (hit.collider != null)
            {
                Debug.Log("Tether attached");
                tetherAttached = true;
                if (!tetherPositions.Contains(hit.point))
                {
                    tetherPositions.Add(hit.point);
                    tetherJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    tetherJoint.enabled = true;
                    tetherHingeAnchorSprite.enabled = true;
                    tetherLength = tetherJoint.distance;
                }
            }
            else
            {
                tetherRenderer.enabled = false;
                tetherAttached = false;
                tetherJoint.enabled = false;
            }
        }
    }

    private void ResetTether()
    {
        tetherJoint.enabled = false;
        tetherAttached = false;
        tetherRenderer.positionCount = 2;
        tetherRenderer.SetPosition(0, transform.position);
        tetherRenderer.SetPosition(1, transform.position);
        tetherPositions.Clear();
        tetherHingeAnchorSprite.enabled = false;
        wrapPointsLookup.Clear();
        wrapLoop = 0;
    }

    private void UpdateTetherPositions()
    {
        if (!tetherAttached)
        {
            return;
        }

        tetherRenderer.positionCount = tetherPositions.Count + 1;

        for (var i = tetherRenderer.positionCount - 1; i >= 0; i--)
        {
            if (i != tetherRenderer.positionCount - 1) // if not the Last point of line renderer
            {
                tetherRenderer.SetPosition(i, tetherPositions[i]);

                if (i == tetherPositions.Count - 1 || tetherPositions.Count == 1)
                {
                    var tetherPosition = tetherPositions[tetherPositions.Count - 1];
                    if (tetherPositions.Count == 1)
                    {
                        tetherHingeAnchorRb.transform.position = tetherPosition;
                        if (!distanceSet)
                        {
                            tetherJoint.distance = Vector2.Distance(transform.position, tetherPosition);
                            distanceSet = true;
                        }
                    }
                    else
                    {
                        tetherHingeAnchorRb.transform.position = tetherPosition;
                        if (!distanceSet)
                        {
                            tetherJoint.distance = Vector2.Distance(transform.position, tetherPosition);
                            distanceSet = true;
                        }
                    }
                }
                else if (i - 1 == tetherPositions.IndexOf(tetherPositions.Last()))
                {
                    var tetherPosition = tetherPositions.Last();
                    tetherHingeAnchorRb.transform.position = tetherPosition;
                    if (!distanceSet)
                    {
                        tetherJoint.distance = Vector2.Distance(transform.position, tetherPosition);
                        distanceSet = true;
                    }
                }
            }
            else
            {
                tetherRenderer.SetPosition(i, transform.position);
            }
        }
    }

    private Vector2 GetClosestColliderPointFromRaycastHit(RaycastHit2D hit, CompositeCollider2D polyCollider)
    {
        // This converts the polygon collider's collection of points, into a dictionary of Vector2 positions 
        // (the value of each dictionary entry is the position itself), and the key of each entry, is set to 
        // the distance that this point is to the player's position (float value). Something else happens here: 
        // the resulting position is transformed into world space (by default a collider's vertex positions are 
        // stored in local space - i.e. local to the object the collider sits on, and we want the world space 
        // positions).

        // TO-DO: ensure this actually covers all sub-areas; may need to iterate over all paths in composite collider
        Vector2[] points = new Vector2[polyCollider.GetPathPointCount(0)];
        polyCollider.GetPath(0, points);

        var distanceDictionary = points.ToDictionary<Vector2, float, Vector2>(
            position => Vector2.Distance(hit.point, polyCollider.transform.TransformPoint(position)),
            position => polyCollider.transform.TransformPoint(position));

        // The dictionary is ordered by key. In other words, the distance closest to the player's current 
        // position, and the closest one is returned, meaning that whichever point is returned from this method, 
        // is the point on the collider between the player and the current hinge point on the tether.
        var orderedDictionary = distanceDictionary.OrderBy(e => e.Key);
        return orderedDictionary.Any() ? orderedDictionary.First().Value : Vector2.zero;
    }

    private void HandleTetherLength()
    {
        if (Input.GetButton("Tether Retract") && tetherAttached && !isColliding)
        {
            tetherJoint.distance -= Time.deltaTime * climbSpeed;
            tetherLength -= Time.deltaTime * climbSpeed;
        }
        else if (Input.GetButton("Tether Extend") && tetherAttached && tetherLength < tetherMaxLength)
        {
            tetherJoint.distance += Time.deltaTime * climbSpeed;
            tetherLength += Time.deltaTime * climbSpeed;

        }
    }

    // May be needed?
    /*void OnTriggerStay2D(Collider2D colliderStay)
    {
        isColliding = true;
    }*/

    private void OnCollisionStay2D(Collision2D other)
    {
        isColliding = true;
    }

    // May be needed?
    /*private void OnTriggerExit2D(Collider2D colliderOnExit)
    {
        isColliding = false;
    }*/

    private void OnCollisionExit2D(Collision2D other)
    {
        isColliding = false;
    }

    private void HandleTetherUnwrap()
    {
        if (tetherPositions.Count <= 1)
        {
            return;
        }

        // Hinge = next point up from the player position
        // Anchor = next point up from the Hinge
        // Hinge Angle = Angle between anchor and hinge
        // Player Angle = Angle between anchor and player

        var anchorIndex = tetherPositions.Count - 2;
        var hingeIndex = tetherPositions.Count - 1;
        var anchorPosition = tetherPositions[anchorIndex];
        var hingePosition = tetherPositions[hingeIndex];
        var hingeDir = hingePosition - anchorPosition;
        var hingeAngle = Vector2.Angle(anchorPosition, hingeDir);
        var playerDir = playerPosition - anchorPosition;
        var playerAngle = Vector2.Angle(anchorPosition, playerDir);

        if (!wrapPointsLookup.ContainsKey((hingePosition, wrapLoop)))
        {
            Debug.LogError("We were not tracking hingePosition (" + hingePosition + ") in the look up dictionary.");
            return;
        }

        if (playerAngle < hingeAngle)
        {
            if (wrapPointsLookup[(hingePosition, wrapLoop)] == 1)
            {
                UnwrapTetherPosition(anchorIndex, hingeIndex);
                return;
            }

            wrapPointsLookup[(hingePosition, wrapLoop)] = -1;
        }
        else
        {
            if (wrapPointsLookup[(hingePosition, wrapLoop)] == -1)
            {
                UnwrapTetherPosition(anchorIndex, hingeIndex);
                return;
            }

            wrapPointsLookup[(hingePosition, wrapLoop)] = 1;
        }
    }

    private void UnwrapTetherPosition(int anchorIndex, int hingeIndex)
    {
        var newAnchorPosition = tetherPositions[anchorIndex];
        wrapPointsLookup.Remove((tetherPositions[hingeIndex], wrapLoop));

        // iterate through lookup and find highest wrapLoop
        int highestLoop = 0;
        foreach (var keyValuePair in wrapPointsLookup)
        {
            if (keyValuePair.Key.Item2 > highestLoop)
            {
                highestLoop = keyValuePair.Key.Item2;
            }
        }
        wrapLoop = highestLoop;
        // TO-DO: see if the above can be done more efficiently or less often

        tetherPositions.RemoveAt(hingeIndex);

        tetherHingeAnchorRb.transform.position = newAnchorPosition;
        distanceSet = false;

        // Set new rope distance joint distance for anchor position if not yet set.
        if (distanceSet)
        {
            return;
        }
        tetherJoint.distance = Vector2.Distance(transform.position, newAnchorPosition);
        distanceSet = true;
    }
}
