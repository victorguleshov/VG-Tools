using UnityEngine;

namespace VG
{
    [ExecuteInEditMode]
    public class RandomSprite : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] spriteCollection;

        private void OnEnable()
        {
            if (spriteRenderer && spriteCollection.Length > 0)
                spriteRenderer.sprite = spriteCollection[Random.Range(0, spriteCollection.Length)];
        }

        private void OnValidate()
        {
            if (spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }
}