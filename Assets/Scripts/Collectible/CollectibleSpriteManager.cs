using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpriteManager : MonoBehaviour
{
    public static CollectibleSpriteManager Instance { get; private set; }

    [SerializeField] private CollectibleSpriteData collectibleSpriteData;

    private Dictionary<Collectible.CollectibleType, Sprite> collectibleSpriteDictionary;

    private void Awake()
    {
        // Implement the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitializeSpriteDictionary();
    }

    private void InitializeSpriteDictionary()
    {
        collectibleSpriteDictionary = new Dictionary<Collectible.CollectibleType, Sprite>();

        foreach (var pair in collectibleSpriteData.collectibleSprites)
        {
            if (!collectibleSpriteDictionary.ContainsKey(pair.type))
            {
                collectibleSpriteDictionary.Add(pair.type, pair.sprite);
            }
            else
            {
                Debug.LogWarning("Duplicate collectible type found: " + pair.type);
            }
        }

        Debug.Log("Collectible sprite dictionary initialized with " + collectibleSpriteDictionary.Count + " entries.");
    }

    public Sprite GetSpriteForType(Collectible.CollectibleType type)
    {
        if (collectibleSpriteDictionary.TryGetValue(type, out Sprite sprite))
        {
            return sprite;
        }
        else
        {
            Debug.LogWarning("Sprite not found for type: " + type);
            return null;
        }
    }
}
