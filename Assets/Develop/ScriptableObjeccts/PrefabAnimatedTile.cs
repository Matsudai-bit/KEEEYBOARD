using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "PrefabAnimatedTile", menuName = "ScriptableObjects/PrefabAnimatedTile")]

public class PrefabAnimatedTile : AnimatedTile
{
    public GameObject prefab;

    // タイルが置かれた時に呼ばれる
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        tileData.gameObject = prefab; // ここでプレハブを指定できる
    }
}
