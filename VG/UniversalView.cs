using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VG
{
    public class UniversalView : MonoBehaviour
    {
        [SerializeField] private List<Button> buttons;
        [SerializeField] private List<Image> images;
        [SerializeField] private List<TextMeshProUGUI> texts;
        [SerializeField] private List<UniversalView> views;
        [SerializeField] private List<Object> objects;
        [SerializeField] private List<Material> materials;

        private ReadOnlyCollection<Button> buttonsWrapper;
        private ReadOnlyCollection<Image> imagesWrapper;
        private ReadOnlyCollection<Material> materialsWrapper;
        private ReadOnlyCollection<Object> objectsWrapper;
        private ReadOnlyCollection<TextMeshProUGUI> textsWrapper;
        private ReadOnlyCollection<UniversalView> viewsWrapper;

        public ReadOnlyCollection<Button> Buttons => buttonsWrapper ??= buttons.AsReadOnly();
        public ReadOnlyCollection<Image> Images => imagesWrapper ??= images.AsReadOnly();
        public ReadOnlyCollection<TextMeshProUGUI> Texts => textsWrapper ??= texts.AsReadOnly();
        public ReadOnlyCollection<UniversalView> Views => viewsWrapper ??= views.AsReadOnly();
        public ReadOnlyCollection<Object> Objects => objectsWrapper ??= objects.AsReadOnly();
        public ReadOnlyCollection<Material> Materials => materialsWrapper ??= materials.AsReadOnly();

        public Button button => Buttons[0];
        public Image image => Images[0];
        public Image secondImage => Images[1];
        public Image thirdImage => Images[2];

        public TextMeshProUGUI titleText => Texts[0];
        public TextMeshProUGUI descriptionText => Texts[1];

        public TextMeshProUGUI primaryText => Texts[0];
        public TextMeshProUGUI secondaryText => Texts[1];
        public TextMeshProUGUI tertiaryText => Texts[2];

        public Button.ButtonClickedEvent onClick => button.onClick;

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

        private void Reset()
        {
            (texts ??= new List<TextMeshProUGUI>()).Clear();
            (images ??= new List<Image>()).Clear();
            (views ??= new List<UniversalView>()).Clear();
            (buttons ??= new List<Button>()).Clear();
            (objects ??= new List<Object>()).Clear();

            var txt = GetComponent<TextMeshProUGUI>();
            if (txt) texts.Add(txt);
            var img = GetComponent<Image>();
            if (img) images.Add(img);
            var btn = GetComponent<Button>();
            if (btn) buttons.Add(btn);

            foreach (Transform item in transform) Collect(item);

            void Collect(Transform tfm)
            {
                var v = tfm.GetComponent<UniversalView>();
                if (v)
                {
                    views.Add(v);
                    return;
                }

                if (tfm.GetComponent<UniversalToggle>()) return;

                var t = tfm.GetComponent<TextMeshProUGUI>();
                if (t) texts.Add(t);
                var i = tfm.GetComponent<Image>();
                if (i) images.Add(i);
                var b = tfm.GetComponent<Button>();
                if (b) buttons.Add(b);

                foreach (Transform it in tfm) Collect(it);
            }
        }
    }
}