using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Texture_To_Sprite : MonoBehaviour
{
  void Start()
  {

  }
  public static Sprite ConvertTextureToSprite(Texture2D texture)
  {
    // Create a Sprite from Texture2D
    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
  }

  public static Texture2D ConvertSpriteToTexture(Sprite sprite)
  {
    if (sprite == null || sprite.texture == null)
    {
      Debug.LogError("Invalid sprite or texture.");
      return null;
    }

    // Create a new Texture2D with the same dimensions as the sprite's rect
    Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
    texture.filterMode = sprite.texture.filterMode;
    texture.wrapMode = sprite.texture.wrapMode;

    // Copy the pixel data from the sprite's texture
    Color[] pixels = sprite.texture.GetPixels(
        Mathf.FloorToInt(sprite.textureRect.x),
        Mathf.FloorToInt(sprite.textureRect.y),
        Mathf.FloorToInt(sprite.textureRect.width),
        Mathf.FloorToInt(sprite.textureRect.height));
    texture.SetPixels(pixels);
    texture.Apply();

    return texture;
  }


}