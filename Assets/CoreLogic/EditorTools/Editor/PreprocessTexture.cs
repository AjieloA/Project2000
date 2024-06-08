//==========================
// - FileName: PreprocessTexture.cs
// - Created: AjieloA
// - CreateTime: 2024-06-08 18:40:52
// - Email: 1758580256@qq.com
// - Description:图片资源导入模板
//==========================
using UnityEditor;

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
