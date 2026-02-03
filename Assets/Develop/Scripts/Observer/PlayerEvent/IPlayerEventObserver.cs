using UnityEngine;

/// <summary>
/// プレイヤイベントのインターフェース
/// </summary>
public interface IPlayerEventObserver
{
    /// <summary>
    /// プレイヤラクションイベントを受信するメソッド
    /// </summary>
    /// <param name="eventID">イベントメッセージ</param>
    public void OnEvent(PlayerEventID eventID, GameTile playerTile);
}