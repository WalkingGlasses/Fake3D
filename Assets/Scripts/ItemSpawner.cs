
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float speed = 40f;

    public List<Item> items = new List<Item>();
    public Transform parent;

    public float spawnDistance = 200f;

    [Header("Player Collision")]
    public EndlessRunnerPlayerMovement player;
    public SurvivorPlayerHealth playerHealth;

    public float hitDistance = 2.5f;

    private void Start()
    {
        InvokeRepeating(nameof(BeginSpawn), 1f, 1f);
    }

    private void BeginSpawn()
    {
        Item spawnedItem = Instantiate(itemToSpawn, parent);

        int lane = Random.Range(0, 3);

        float xPosition = (lane - 1) * 2f;

        spawnedItem.lane = lane;

        spawnedItem.itemPosition = new Vector3(
            xPosition,
            0f,
            spawnDistance
        );

        items.Add(spawnedItem);
    }

    private void Update()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            Item item = items[i];

            if (item == null)
            {
                items.RemoveAt(i);
                continue;
            }

            ItemMover(item);

            CheckCollision(item);

            if (item.itemPosition.z <= -10f)
            {
                items.RemoveAt(i);
                Destroy(item.gameObject);
            }
        }
    }

    private void ItemMover(Item item)
    {
        item.itemPosition.z -= speed * Time.deltaTime;
    }

    private void CheckCollision(Item item)
    {
        if (item.collisionResolved) return;

        if (player == null || playerHealth == null) return;

        float distance = Mathf.Abs(
            item.itemPosition.z - player.transform.position.z
        );

        if (distance <= hitDistance)
        {
            item.collisionResolved = true;

            if (item.lane == player.CurrentLane)
            {
                if (player.GetJumpHeight() <
                    item.requiredJumpHeight)
                {
                    playerHealth.TakeDamage(5f);
                }
            }
        }
    }
}
