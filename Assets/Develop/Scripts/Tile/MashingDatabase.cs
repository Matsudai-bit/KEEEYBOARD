using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MashingDatabase : MonoBehaviour
{
    [Serializable]
    struct MashingData
    {
        public Vector3Int pos;
        public Key keyCode;
        public int mashingCount;

        public MashingData(Vector3Int pos, Key keyCode, int count)
        {
            this.pos = new(pos.x, pos.y, pos.z);
            this.keyCode = keyCode;
            this.mashingCount = count;
        }
    }

    // シリアライズ用（保存される）
    [SerializeField]
    List<MashingData> data = new();

    // 実行時高速アクセス用
    Dictionary<Vector3Int, MashingData> m_mashingDatabase = new();

    private void Awake()
    {
        // List → Dictionary 復元
        m_mashingDatabase.Clear();
        foreach (var d in data)
        {
            m_mashingDatabase[d.pos] = d;
        }
    }

    public void AddData(Vector3Int key, Key keyCode, int mashingCount)
    {
        var newData = new MashingData(key, keyCode, mashingCount);

        // Dictionary は上書き対応
        m_mashingDatabase[key] = newData;

        // List 側も同期
        int index = data.FindIndex(d => d.pos == key);
        if (index >= 0)
        {
            data[index] = newData;
        }
        else
        {
            data.Add(newData);
        }
    }

    public void GetData(Vector3Int key, out Key keyCode, out int mashingCount)
    {
        if (m_mashingDatabase.TryGetValue(key, out var d))
        {
            keyCode = d.keyCode;
            mashingCount = d.mashingCount;
            return;
        }

        // 見つからなかった場合のデフォルト
        keyCode = Key.None;
        mashingCount = 9999;
    }
}
