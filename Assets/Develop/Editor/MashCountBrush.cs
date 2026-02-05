using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;


// [CreateAssetMenu] を使うことで、Projectウィンドウからブラシを作成できます
[CreateAssetMenu(menuName = "2D/Custom/MashGridBrush")]
[CustomGridBrush(true, false, false, "Mash Grid Brush")]
public class MashCountBrush : GridBrush // GameObjectBrush ではなく GridBrush を継承
{
    public int mashCountPerTile = 5;

    public override void Paint(GridLayout grid, GameObject target, Vector3Int pos)
    {
        // 新しい Brush 初回対策
        if (grid == null || target == null)
            return;

        if (!target.TryGetComponent<Tilemap>(out var tilemap))
            return;

        // ActiveTilemap が未確定な瞬間を回避
        if (GridPaintingState.scenePaintTarget != target)
            return;

        base.Paint(grid, target, pos);

        var tile = tilemap.GetTile<MashingTile>(pos);
        if (tile == null) return;

        //var instance = Object.Instantiate(tile);
        //instance.MashingCount = mashCountPerTile;
        //tilemap.SetTile(pos, instance);

        // 2. データ管理コンポーネントを取得（なければ追加）
        var dataManager = target.GetComponent<MashingDatabase>();
        if (dataManager == null)
        {
            dataManager = target.AddComponent<MashingDatabase>();
        }

        // 3. 座標に対してカウントを記録（タイル自体は複製しない！）
        dataManager.AddData(pos, tile.KeyCode, mashCountPerTile);

        EditorUtility.SetDirty(target);
        PrefabUtility.RecordPrefabInstancePropertyModifications(target);
        Debug.Log(target.name +" :正常に配置");
    }
}