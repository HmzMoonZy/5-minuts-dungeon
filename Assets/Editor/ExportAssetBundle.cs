using System.IO;
using UnityEditor;
using UnityEngine;

public class ExportAssetBundle
{
    //private static string assetbundle_path = Application.streamingAssetsPath;
    private static string assetbundle_path = Application.dataPath + "/AssetBundle";

    ///LZMA算法, 高压缩比;
    [@MenuItem("AssetBundle/ExportAssetBundle_LZMA")]
    private static void ExportAssetBundle_LZ4D()
    {
        if (!Directory.Exists(assetbundle_path))
        {
            Directory.CreateDirectory(assetbundle_path);
        }
        BuildPipeline.BuildAssetBundles(assetbundle_path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

    ///LZ4算法, 数据块压缩;
    [@MenuItem("AssetBundle/ExportAssetBundle_LZ4")]
    private static void ExportAssetBundle_LZ4()
    {
        if (!Directory.Exists(assetbundle_path))
        {
            Directory.CreateDirectory(assetbundle_path);
        }
        BuildPipeline.BuildAssetBundles(assetbundle_path, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
    }

    ///不压缩
    [@MenuItem("AssetBundle/ExportAssetBundle_Uncompresse")]
    private static void ExportAssetBundle_Uncompresse()
    {
        if (!Directory.Exists(assetbundle_path))
        {
            Directory.CreateDirectory(assetbundle_path);
        }
        BuildPipeline.BuildAssetBundles(assetbundle_path, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);
    }
}