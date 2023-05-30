using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VG
{
    public class UniversalToggle : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private List<Image> images;
        [SerializeField] private List<TextMeshProUGUI> texts;
        [SerializeField] private List<UniversalView> views;
        private ReadOnlyCollection<Image> imagesWrapper;
        private ReadOnlyCollection<TextMeshProUGUI> textsWrapper;

        private ReadOnlyCollection<Toggle> togglesWrapper;
        private ReadOnlyCollection<UniversalView> viewsWrapper;

        public ReadOnlyCollection<Image> Images => imagesWrapper ??= images.AsReadOnly();
        public ReadOnlyCollection<TextMeshProUGUI> Texts => textsWrapper ??= texts.AsReadOnly();
        public ReadOnlyCollection<UniversalView> Views => viewsWrapper ??= views.AsReadOnly();

        public Toggle toggle => _toggle;
        public Image image => Images[0];
        public Image firstImage => Images[0];
        public Image secondImage => Images[1];
        public Image thirdImage => Images[2];

        public TextMeshProUGUI titleText => Texts[0];
        public TextMeshProUGUI descriptionText => Texts[1];

        public TextMeshProUGUI primaryText => Texts[0];
        public TextMeshProUGUI secondaryText => Texts[1];
        public TextMeshProUGUI tertiaryText => Texts[2];

        public Toggle.ToggleEvent onValueChanged => toggle.onValueChanged;

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

        public bool isOn
        {
            get => toggle.isOn;
            set => toggle.isOn = value;
        }

        private void Reset()
        {
            (texts ??= new List<TextMeshProUGUI>()).Clear();
            (images ??= new List<Image>()).Clear();
            (views ??= new List<UniversalView>()).Clear();

            _toggle = null;

            var txt = GetComponent<TextMeshProUGUI>();
            if (txt) texts.Add(txt);
            var img = GetComponent<Image>();
            if (img) images.Add(img);
            var tgl = GetComponent<Toggle>();
            if (tgl) _toggle = tgl;

            foreach (Transform item in transform) Collect(item);

            void Collect(Transform tfm)
            {
                var v = tfm.GetComponent<UniversalView>();
                if (v)
                {
                    views.Add(v);
                    return;
                }

                var t = tfm.GetComponent<TextMeshProUGUI>();
                if (t) texts.Add(t);
                var i = tfm.GetComponent<Image>();
                if (i) images.Add(i);

                foreach (Transform it in tfm) Collect(it);
            }
        }

        public void SimpleViewDeactivation(bool value)
        {
            if (views?.Count > 0)
            {
                if (views.Count > 1)
                {
                    views[0].gameObject.SetActive(!value);
                    foreach (var t in views[0].Texts)
                        t.gameObject.SetActive(!value);
                    foreach (var i in views[0].Images)
                        i.gameObject.SetActive(!value);
                    foreach (var v in views[0].Views)
                        v.gameObject.SetActive(!value);

                    views[1].gameObject.SetActive(value);
                    foreach (var t in views[1].Texts)
                        t.gameObject.SetActive(value);
                    foreach (var i in views[1].Images)
                        i.gameObject.SetActive(value);
                    foreach (var v in views[1].Views)
                        v.gameObject.SetActive(value);
                }
                else
                {
                    views[0].gameObject.SetActive(false);
                    foreach (var t in views[0].Texts)
                        t.gameObject.SetActive(value);
                    foreach (var i in views[0].Images)
                        i.gameObject.SetActive(value);
                    foreach (var v in views[0].Views)
                        v.gameObject.SetActive(value);
                }
            }
        }
    }
}