using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Resize_GameObject_Function : MonoBehaviour
{

  //public static bool isResize = false;
  void Start()
  {
  }

  public static void Resize_Parent_GameObject(RectTransform contentTransform, float Multiply = 1.00005f)
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform);
    Canvas.ForceUpdateCanvases();
    float totalHeight = 0f;
    foreach (RectTransform childRect in contentTransform)
    {
      if (childRect.gameObject.activeSelf)
      {
        totalHeight += childRect.rect.height;
      }
    }
    contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, totalHeight * Multiply);
  }

  public static IEnumerator Set_NativeSize_For_GameObject(Image imageComponent)
  {
    Canvas.ForceUpdateCanvases();

    if (imageComponent.sprite == null)
    {
      Debug.LogWarning("Sprite is null on Image component.");
      yield break;
    }


    yield return new WaitForEndOfFrame();
    float originalWidth = imageComponent.sprite.rect.width;
    float originalHeight = imageComponent.sprite.rect.height;

    RectTransform rectTransform = imageComponent.rectTransform;
    float aspectRatio = originalWidth / originalHeight;
    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.x / aspectRatio);
    LayoutRebuilder.ForceRebuildLayoutImmediate(imageComponent.rectTransform);

  }
  public static IEnumerator Set_NativeSize_For_GameObject(RawImage imageComponent)
  {
    if (imageComponent == null)
    {
      Debug.LogWarning("RawImage component is null.");
      yield break;
    }

    Canvas.ForceUpdateCanvases();

    if (imageComponent.texture == null)
    {
      Debug.LogWarning("Texture is null on RawImage component.");
      yield break;
    }

    // imageComponent.SetNativeSize();

    // yield return new WaitForEndOfFrame();

    float originalWidth = imageComponent.texture.width;
    float originalHeight = imageComponent.texture.height;

    Debug.Log($"Texture Size: {originalWidth}x{originalHeight}");
    if (originalHeight <= 0)
    {
      Debug.LogWarning("Texture height is zero or negative, aspect ratio calculation will fail.");
      yield break;
    }

    RectTransform rectTransform = imageComponent.rectTransform;
    if (rectTransform == null)
    {
      Debug.LogWarning("RectTransform is null on RawImage component.");
      yield break;
    }

    float aspectRatio = originalWidth / originalHeight;
    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.x / aspectRatio);
    LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
  }
}