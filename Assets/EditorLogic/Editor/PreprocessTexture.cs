using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PreprocessTexture : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        if (assetPath.IndexOf("UITexture") == -1)
            return;
        if (assetPath.IndexOf(".png") == -1)
            return;
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
    }
}
