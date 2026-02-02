using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // 現在押されているすべてのキーをチェック
        foreach (var key in keyboard.allKeys)
        {
            Debug.Log(key.name);
            if (key.isPressed)
            {
          
                Debug.Log($"現在押されているキー: {key.name}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
