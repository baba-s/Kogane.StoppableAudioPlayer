﻿using System;
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
            return Play( audioClip, null, null );
        }

        /// <summary>
        /// 再生します
        /// </summary>
        public StoppableAudioHandle Play( AudioClip audioClip, float? volume, float? pitch )
        {
            if ( audioClip == null ) return new();

            foreach ( var audioSource in m_audioSources )
            {
                if ( audioSource == null ) continue;
                if ( audioSource.isPlaying ) continue;

                audioSource.clip = audioClip;

                if ( volume != null )
                {
                    audioSource.volume = ( float )volume;
                }

                if ( pitch != null )
                {
                    audioSource.pitch = ( float )pitch;
                }

                audioSource.Play();

                return new
                (
                    length: audioClip.length,
                    task: UniTask.Create
                    (
                        () =>
                        {
                            if ( this == null ) throw new OperationCanceledException();
                            if ( audioSource == null ) throw new OperationCanceledException();

                            return UniTask.WaitWhile
                            (
                                () => audioSource.isPlaying,
                                cancellationToken: this.GetCancellationTokenOnDestroy()
                            );
                        }
                    ),
                    onStop: () => Stop( audioClip )
                );
            }

            return new();
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