using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kogane
{
    /// <summary>
    /// 再生中の AudioClip を停止できるコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class StoppableAudioPlayer : MonoBehaviour
    {
        //================================================================================
        // 変数(SerializeField)
        //================================================================================
        [SerializeField] private AudioSource[] m_audioSources;

        //================================================================================
        // 関数
        //================================================================================
        /// <summary>
        /// 再生します
        /// </summary>
        public StoppableAudioHandle Play( AudioClip audioClip )
        {
            return Play( audioClip, null );
        }

        /// <summary>
        /// 再生します
        /// </summary>
        public StoppableAudioHandle Play( AudioClip audioClip, float? volume )
        {
            foreach ( var audioSource in m_audioSources )
            {
                if ( audioSource.isPlaying ) continue;

                audioSource.clip = audioClip;

                if ( volume != null )
                {
                    audioSource.volume = ( float )volume;
                }

                audioSource.Play();

                return new
                (
                    length: audioClip.length,
                    task: UniTask.Create( () => UniTask.WaitWhile( () => audioSource.isPlaying, cancellationToken: this.GetCancellationTokenOnDestroy() ) ),
                    onStop: () => Stop( audioClip )
                );
            }

            return null;
        }

        /// <summary>
        /// 停止します
        /// </summary>
        public void Stop( AudioClip audioClip )
        {
            foreach ( var audioSource in m_audioSources )
            {
                if ( audioSource == null || audioSource.clip != audioClip ) continue;
                audioSource.Stop();
            }
        }
    }
}