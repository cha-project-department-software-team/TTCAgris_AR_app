using UnityEngine;
using UnityEngine.UI;
public class ResizeImageEnable : MonoBehaviour
{
    private void Start()
    {

    }
    private void OnEnable()
    {
        var imageComponent = GetComponent<Image>();
        
        var rawImageComponent = GetComponent<RawImage>();

        if (imageComponent != null && rawImageComponent == null)
        {
            StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(imageComponent));
        }
        else if (rawImageComponent != null && imageComponent == null)
        {
            StartCoroutine(Resize_GameObject_Function.Set_NativeSize_For_GameObject(rawImageComponent));
        }

    }
}