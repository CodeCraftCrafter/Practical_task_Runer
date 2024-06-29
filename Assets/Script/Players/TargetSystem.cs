using UnityEngine;
using System.Linq;

public class TargetSystem : MonoBehaviour
{
    public float range = 15f;
    public string[] targetTags = { "Target_1", "Target_2" };
    public Texture2D aim;
    public float aimSize = 50f;

    private Transform player;
    private GameObject currentTarget;
    private Collider[] colls = new Collider[0];

    private void OnEnable()
    {
        Generator.OnPlayerSpawned += SetPlayer;
    }

    private void OnDisable()
    {
        Generator.OnPlayerSpawned -= SetPlayer;
    }

    private void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    void Update()
    {
        if (player == null) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GetTargets();
            if (colls.Length > 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    NearTarget();
                }
                else if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    RandomTarget();
                }
                else
                {
                    NearTarget();
                }
            }

            if (colls.Length == 0)
            {
                currentTarget = null;
            }
            else
            {
                float curDist = Vector3.Distance(player.position, currentTarget.transform.position);
                if (curDist > range) currentTarget = null;
            }
        }

        PlayerRotate();
    }

    void OnGUI()
    {
        if (currentTarget)
        {
            Vector2 tmp = new Vector2(Camera.main.WorldToScreenPoint(currentTarget.transform.position).x,
                                      Screen.height - Camera.main.WorldToScreenPoint(currentTarget.transform.position).y);

            Vector2 offset = new Vector2(-aimSize / 2, -aimSize / 2);
            GUI.DrawTexture(new Rect(tmp.x + offset.x, tmp.y + offset.y, aimSize, aimSize), aim);
        }
    }

    void GetTargets()
    {
        colls = new Collider[0];
        colls = Physics.OverlapSphere(player.position, range).Where(coll => targetTags.Contains(coll.tag)).ToArray();

        if (currentTarget == null)
        {
            NearTarget();
        }
    }

    void NearTarget()
    {
        if (colls.Length > 0)
        {
            Collider currentCollider = null;
            float dist = Mathf.Infinity;

            foreach (Collider coll in colls)
            {
                float currentDist = Vector3.Distance(player.position, coll.transform.position);

                if (currentTarget)
                {
                    if (currentDist < dist && currentTarget != coll.gameObject)
                    {
                        currentCollider = coll;
                        dist = currentDist;
                    }
                }
                else
                {
                    if (currentDist < dist)
                    {
                        currentCollider = coll;
                        dist = currentDist;
                    }
                }
            }

            currentTarget = currentCollider?.gameObject;
            if (currentTarget)
            {
                PlayerRotate();
            }
        }
    }

    void RandomTarget()
    {
        if (colls.Length > 1)
        {
            GameObject[] tmp = colls.Select(coll => coll.gameObject).Where(go => go != currentTarget).ToArray();
            currentTarget = tmp[Random.Range(0, tmp.Length)];
            if (currentTarget)
            {
                PlayerRotate();
            }
        }
    }

    void PlayerRotate()
    {
        if (currentTarget)
        {
            Vector3 lookPos = currentTarget.transform.position - player.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            player.rotation = Quaternion.Lerp(player.rotation, rotation, 10 * Time.deltaTime);
        }
    }
}