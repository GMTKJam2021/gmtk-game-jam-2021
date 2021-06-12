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
    public PlayerMovementController playerMovement;
    private bool tetherAttached;
    private Vector2 playerPosition;
    private Rigidbody2D tetherHingeAnchorRb;
    private SpriteRenderer tetherHingeAnchorSprite;
    public LineRenderer tetherRenderer;
    public LayerMask tetherLayerMask;
    private float tetherMaxCastDistance = 20f;
    private List<Vector2> tetherPositions = new List<Vector2>();
    private bool distanceSet;
    private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();
    public float climbSpeed = 3f;
    private bool isColliding;

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

        playerPosition = transform.position;

        if (!tetherAttached)
        {
            playerMovement.isSwinging = false;
            SetCrosshairPosition(aimAngle);
        }
        else
        {
            playerMovement.isSwinging = true;
            playerMovement.tetherHook = tetherPositions.Last();

            crosshairSprite.enabled = false;

            if (tetherPositions.Count > 0)
            {
                var lastRopePoint = tetherPositions.Last();
                var playerToCurrentNextHit = Physics2D.Raycast(playerPosition, (lastRopePoint - playerPosition).normalized, Vector2.Distance(playerPosition, lastRopePoint) - 0.1f, tetherLayerMask);

                if (playerToCurrentNextHit)
                {
                    var colliderWithVertices = playerToCurrentNextHit.collider as PolygonCollider2D;
                    if (colliderWithVertices != null)
                    {
                        var closestPointToHit = GetClosestColliderPointFromRaycastHit(playerToCurrentNextHit, colliderWithVertices);

                        if (wrapPointsLookup.ContainsKey(closestPointToHit))
                        {
                            ResetTether();
                            return;
                        }

                        tetherPositions.Add(closestPointToHit);
                        wrapPointsLookup.Add(closestPointToHit, 0);
                        distanceSet = false;
                    }
                }
            }
        }

        HandleInput(aimDirection);
        UpdateTetherPositions();
        HandleTetherLength();
        HandleTetherUnwrap();
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
        if (Input.GetMouseButton(0))
        {
            if (tetherAttached) return;
            tetherRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, aimDirection, tetherMaxCastDistance, tetherLayerMask);

            if (hit.collider != null)
            {
                tetherAttached = true;
                if (!tetherPositions.Contains(hit.point))
                {
                    // Jump slightly to distance the player a little from the ground after grappling to something.
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
                    tetherPositions.Add(hit.point);
                    tetherJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    tetherJoint.enabled = true;
                    tetherHingeAnchorSprite.enabled = true;
                }
            }
            else
            {
                tetherRenderer.enabled = false;
                tetherAttached = false;
                tetherJoint.enabled = false;
            }
        }

        if (Input.GetMouseButton(1))
        {
            ResetTether();
        }
    }

    private void ResetTether()
    {
        tetherJoint.enabled = false;
        tetherAttached = false;
        playerMovement.isSwinging = false;
        tetherRenderer.positionCount = 2;
        tetherRenderer.SetPosition(0, transform.position);
        tetherRenderer.SetPosition(1, transform.position);
        tetherPositions.Clear();
        tetherHingeAnchorSprite.enabled = false;
        wrapPointsLookup.Clear();
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

    private Vector2 GetClosestColliderPointFromRaycastHit(RaycastHit2D hit, PolygonCollider2D polyCollider)
    {
        // This converts the polygon collider's collection of points, into a dictionary of Vector2 positions 
        // (the value of each dictionary entry is the position itself), and the key of each entry, is set to 
        // the distance that this point is to the player's position (float value). Something else happens here: 
        // the resulting position is transformed into world space (by default a collider's vertex positions are 
        // stored in local space - i.e. local to the object the collider sits on, and we want the world space 
        // positions).
        var distanceDictionary = polyCollider.points.ToDictionary<Vector2, float, Vector2>(
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
        if (Input.GetButton("Tether Extend") && tetherAttached && !isColliding)
        {
            tetherJoint.distance -= Time.deltaTime * climbSpeed;
        }
        else if (Input.GetButton("Tether Retract") && tetherAttached)
        {
            tetherJoint.distance += Time.deltaTime * climbSpeed;
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

        if (!wrapPointsLookup.ContainsKey(hingePosition))
        {
            Debug.LogError("We were not tracking hingePosition (" + hingePosition + ") in the look up dictionary.");
            return;
        }

        if (playerAngle < hingeAngle)
        {
            if (wrapPointsLookup[hingePosition] == 1)
            {
                UnwrapTetherPosition(anchorIndex, hingeIndex);
                return;
            }

            wrapPointsLookup[hingePosition] = -1;
        }
        else
        {
            if (wrapPointsLookup[hingePosition] == -1)
            {
                UnwrapTetherPosition(anchorIndex, hingeIndex);
                return;
            }

            wrapPointsLookup[hingePosition] = 1;
        }
    }

    private void UnwrapTetherPosition(int anchorIndex, int hingeIndex)
    {
        var newAnchorPosition = tetherPositions[anchorIndex];
        wrapPointsLookup.Remove(tetherPositions[hingeIndex]);
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
