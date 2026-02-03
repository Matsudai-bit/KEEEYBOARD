using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーイベントを管理するクラス シングルトン
/// </summary>
public class PlayerEventMessenger
{
    private List<IPlayerEventObserver> m_observers = new List<IPlayerEventObserver>(); // オブザーバーのリスト

    private static PlayerEventMessenger s_instance; // シングルトンインスタンス

    /// <summary>
    /// シングルトンインスタンスを取得
    /// </summary>
    public static PlayerEventMessenger GetInstance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new PlayerEventMessenger();
            }
            return s_instance;
        }
    }

    /// <summary>
    /// オブザーバーを登録するメソッド
    /// </summary>
    /// <param name="observer"></param>
    public void RegisterObserver(IPlayerEventObserver observer)
    {
        // 既に登録されているオブザーバーは追加しない
        if (!m_observers.Contains(observer))
        {
            m_observers.Add(observer);
        }
    }

    /// <summary>
    /// イベントを通知するメソッド
    /// </summary>
    /// <param name="eventMessage">イベントメッセージ</param>
    public void Notify(PlayerEventID eventMessage, GameTile playerTile)
    {
        foreach (var observer in m_observers)
        {
            observer.OnEvent(eventMessage, playerTile);
        }
    }

    /// <summary>
    /// オブザーバーを登録解除するメソッド
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IPlayerEventObserver observer)
    {
        // オブザーバーリストから削除
        if (m_observers.Contains(observer))
        {
            m_observers.Remove(observer);
        }
    }

    /// <summary>
    /// 全てのオブザーバーを登録解除するメソッド
    /// </summary>
    public void RemoveAllObserver()
    {
        m_observers.Clear();
    }
}
