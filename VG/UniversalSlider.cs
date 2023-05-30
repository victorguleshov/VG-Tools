using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VG
{
    public class UniversalSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private List<Image> images = new();
        [SerializeField] private List<TextMeshProUGUI> texts = new();

        private ReadOnlyCollection<Image> imagesWrapper;
        private ReadOnlyCollection<TextMeshProUGUI> textsWrapper;

        public Slider Slider => slider;
        public ReadOnlyCollection<Image> Images => imagesWrapper ??= images.AsReadOnly();
        public ReadOnlyCollection<TextMeshProUGUI> Texts => textsWrapper ??= texts.AsReadOnly();

        public Image image => Images[0];
        public Image secondImage => Images[1];
        public Image thirdImage => Images[2];

        public TextMeshProUGUI titleText => Texts[0];
        public TextMeshProUGUI descriptionText => Texts[1];

        public TextMeshProUGUI primaryText => Texts[0];
        public TextMeshProUGUI secondaryText => Texts[1];
        public TextMeshProUGUI tertiaryText => Texts[2];

        public string text
        {
            get => primaryText.text;
            set => primaryText.text = value;
        }

        public Sprite sprite
        {
            get => image.sprite;
            set => image.sprite = value;
        }

        public float value
        {
            get => slider.value;
            set => slider.value = value;
        }

        private void Reset()
        {
            slider = GetComponentInChildren<Slider>();
            images = GetComponentsInChildren<Image>().ToList();
            texts = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        }
    }
}