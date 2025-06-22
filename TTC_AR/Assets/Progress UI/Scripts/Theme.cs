
using EasyUI.Progress;
using System.Collections.Generic;
using UnityEngine;


namespace EasyUI.Helpers
{

    [System.Serializable]
    public class CustomColor
    {
        public ProgressColor color;
        public Color value;
    }

    [CreateAssetMenu(fileName = "Theme", menuName = "Easy UI/Progress UI/Theme")]
    public class Theme : ScriptableObject
    {
        [Header("name (key) :")]
        public string Name;

        [Space]
        [Header("Colors :")]
        [NonReorderable]
        public List<CustomColor> ProgressColors = new List<CustomColor>(){
            new CustomColor{ color=ProgressColor.Default, value=Color.black },
            new CustomColor{ color=ProgressColor.Red,     value=Color.red },
            new CustomColor{ color=ProgressColor.Purple,  value=Color.magenta },
            new CustomColor{ color=ProgressColor.Magenta, value=Color.magenta },
            new CustomColor{ color=ProgressColor.Blue,    value=Color.blue },
            new CustomColor{ color=ProgressColor.Green,   value=Color.green },
            new CustomColor{ color=ProgressColor.Yellow,  value=Color.yellow },
            new CustomColor{ color=ProgressColor.Orange,  value=Color.yellow }
        };

        [Space]
        [Header("Overlay color :")]
        public Color OverlayColor = new Color32(3, 144, 98, 255);
        public Color BackgroundColor = new Color32(103, 103, 103, 84);
        [Space]
        public Color TitleTextColor = Color.white;
        public Color DetailsTextColor = Color.white;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Name))
                Debug.LogError("Theme's Name must be specified");
        }
    }

}