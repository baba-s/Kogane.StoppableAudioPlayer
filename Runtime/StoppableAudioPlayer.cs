using System;
using UnityEngine;

namespace Kogane
{
    [DisallowMultipleComponent]
    public sealed class StoppableAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource[] m_audioSources;

        public IDisposable Play( AudioClip audioClip )
        {
            return Play( audioClip, null );
        }

        public IDisposable Play( AudioClip audioClip, float? volume )
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

                return Disposable.Create( () => Stop( audioClip ) );
            }

            return Disposable.Empty;
        }

        public void Stop( AudioClip audioClip )
        {
            foreach ( var audioSource in m_audioSources )
            {
                if ( audioSource.clip != audioClip ) continue;
                audioSource.Stop();
            }
        }
    }
}