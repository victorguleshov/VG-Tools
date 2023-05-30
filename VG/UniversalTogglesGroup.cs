using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VG
{
    public class UniversalTogglesGroup : ToggleGroup
    {
        [SerializeField] private List<UniversalToggleData> toggles = new();
        [SerializeField] private List<Image> images = new();
        [SerializeField] private List<TextMeshProUGUI> texts = new();
        private ReadOnlyCollection<Image> imagesWrapper;
        private ReadOnlyCollection<TextMeshProUGUI> textsWrapper;

        private ReadOnlyCollection<UniversalToggleData> togglesWrapper;

        public ReadOnlyCollection<UniversalToggleData> Toggles => togglesWrapper ??= toggles.AsReadOnly();
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

        protected new void Reset()
        {
            //base.Reset ();
            toggles.Clear();
            images.Clear();
            texts.Clear();

            var txt = GetComponent<TextMeshProUGUI>();
            if (txt) texts.Add(txt);
            var img = GetComponent<Image>();
            if (img) images.Add(img);

            foreach (Transform item in transform) Collect(item);

            void Collect(Transform tfm)
            {
                var x = tfm.GetComponent<Toggle>();
                if (x)
                {
                    x.group = this;
                    toggles.Add(new UniversalToggleData(x));
                    return;
                }

                var t = tfm.GetComponent<TextMeshProUGUI>();
                if (t) texts.Add(t);
                var i = tfm.GetComponent<Image>();
                if (i) images.Add(i);

                foreach (Transform it in tfm) Collect(it);
            }
        }

        [Serializable]
        public class UniversalToggleData
        {
            [SerializeField] private Toggle toggle;
            [SerializeField] private List<Image> images;
            [SerializeField] private List<TextMeshProUGUI> texts;

            private ReadOnlyCollection<Image> imagesWrapper;
            private ReadOnlyCollection<TextMeshProUGUI> textsWrapper;

            public UniversalToggleData(Toggle toggle)
            {
                this.toggle = toggle;
                images = toggle.GetComponentsInChildren<Image>().ToList();
                texts = toggle.GetComponentsInChildren<TextMeshProUGUI>().ToList();
            }

            public Toggle Toggle => toggle;

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

            public bool isOn
            {
                get => toggle.isOn;
                set => toggle.isOn = value;
            }

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
        }
    }
}