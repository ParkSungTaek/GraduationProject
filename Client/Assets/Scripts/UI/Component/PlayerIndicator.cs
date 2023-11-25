using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class PlayerIndicator : MonoBehaviour
    {
        public GameObject youText;
        public Image indicatorImage;

        public void Set(int playerOrder, bool my)
        {
            Color color;
            switch (playerOrder)
            {
                case 1:
                    color = new Color(0xAB / 255f, 0x60 / 255f, 0x60 / 255f);
                    break;
                case 2:
                    color = new Color(0x30 / 255f, 0x87 / 255f, 0x57 / 255f);
                    break;
                case 4:
                    color = new Color(0x3D / 255f, 0x6F / 255f, 0x90 / 255f);
                    break;
                default:
                    color = Color.white;
                    break;
            }

            indicatorImage.color = color;
            youText.SetActive(my);
        }
    }
}