using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 使用するキーデータ
/// </summary>
[CreateAssetMenu(fileName = "UsingKeyData", menuName = "ScriptableObjects/UsingKeyData")]
public class UsingKeyData : ScriptableObject
{
    [SerializeField]
    private Key[] keys;

    public Key[] KeyCodes
    {
        get { return keys; }
    }

    private void Reset()
    {
        // Key.A (1) から Key.Z (26) までの連続した値をキャストで取得
        keys = new Key[26];
        for (int i = 0; i < 26; i++)
        {
            keys[i] = (Key)((int)Key.A + i);
        }
        Debug.Log("InputSystem用のA~Zキーを登録しました！");
    }



}
