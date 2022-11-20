# Kogane Stoppable Audio Player

再生中の AudioClip を停止できるコンポーネント

## 使用例

```cs
using Kogane;
using UnityEngine;

public sealed class Example : MonoBehaviour
{
    [SerializeField] private StoppableAudioPlayer m_player;
    [SerializeField] private AudioClip            m_audioClip;

    private void Start()
    {
        // 再生します
        var disposable = m_player.Play
        (
            audioClip: m_audioClip,
            volume: 1.0f
        );

        // 停止します
        disposable.Dispose();
    }
}
```

![2022-11-20_212317](https://user-images.githubusercontent.com/6134875/202901676-406251de-6f7d-4315-bfc4-0cf4ad41916e.png)