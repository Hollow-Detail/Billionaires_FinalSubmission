using UnityEngine;

public class PowerupCrate : MonoBehaviour
{
    public float collectRadius = 2f;
    public int requiredSameColorCount = 2;
    public float checkInterval = 0.5f;

    private float checkTimer = 0f;
    private PowerupSpawner spawner;

    void Start()
    {
        spawner = FindObjectOfType<PowerupSpawner>();
        if (spawner == null)
        {
            Debug.LogWarning("PowerupSpawner not found in scene.");
        }
    }

    void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            checkTimer = checkInterval;
            TryCollect();
        }
    }

    void TryCollect()
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, collectRadius);
        int red = 0, blue = 0, green = 0, yellow = 0;

        foreach (Collider2D col in nearby)
        {
            if (col.CompareTag("Billion"))
            {
                ColorTag tag = col.GetComponent<ColorTag>();
                if (tag != null)
                {
                    switch (tag.colorTag)
                    {
                        case teamColor.ColorChoice.Red: red++; break;
                        case teamColor.ColorChoice.Blue: blue++; break;
                        case teamColor.ColorChoice.Green: green++; break;
                        case teamColor.ColorChoice.Yellow: yellow++; break;
                    }
                }
            }
        }

        if (red >= requiredSameColorCount && blue == 0 && green == 0 && yellow == 0)
        {
            Collect(teamColor.ColorChoice.Red);
        }
        else if (blue >= requiredSameColorCount && red == 0 && green == 0 && yellow == 0)
        {
            Collect(teamColor.ColorChoice.Blue);
        }
        else if (green >= requiredSameColorCount && red == 0 && blue == 0 && yellow == 0)
        {
            Collect(teamColor.ColorChoice.Green);
        }
        else if (yellow >= requiredSameColorCount && red == 0 && blue == 0 && green == 0)
        {
            Collect(teamColor.ColorChoice.Yellow);
        }
    }

    void Collect(teamColor.ColorChoice collectorColor)
    {
        if (spawner != null)
        {
            spawner.ClearPowerup(gameObject);
        }

        foreach (BillionSpawner spawner in FindObjectsOfType<BillionSpawner>())
        {
            ColorTag tag = spawner.GetComponent<ColorTag>();
            if (tag != null && tag.colorTag == collectorColor)
            {
                spawner.TriggerSpecialBillionPhase();
            }
        }

        Destroy(gameObject);
    }
}
