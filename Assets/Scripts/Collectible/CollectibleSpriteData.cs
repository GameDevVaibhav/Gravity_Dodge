using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleSpriteData", menuName = "ScriptableObjects/CollectibleSpriteData", order = 1)]
public class CollectibleSpriteData : ScriptableObject
{
    [System.Serializable]
    public struct CollectibleSpritePair
    {
        public Collectible.CollectibleType type;
        public Sprite sprite;
    }

    public CollectibleSpritePair[] collectibleSprites;

    public Sprite GetSpriteForType(Collectible.CollectibleType type)
    {
        foreach (var pair in collectibleSprites)
        {
            if (pair.type == type)
            {
                return pair.sprite;
            }
        }

        Debug.LogWarning("Sprite not found for type: " + type);
        return null;
    }
}
