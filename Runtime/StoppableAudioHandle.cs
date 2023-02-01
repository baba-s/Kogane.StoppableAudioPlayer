using System;
using Cysharp.Threading.Tasks;

namespace Kogane
{
    /// <summary>
    /// 再生中の AudioClip を停止できるハンドル
    /// </summary>
    public sealed class StoppableAudioHandle : IDisposable
    {
        //================================================================================
        // 変数(readonly)
        //================================================================================
        private readonly Action m_onStop;

        //================================================================================
        // 変数
        //================================================================================
        private bool m_isDisposed;

        //================================================================================
        // プロパティ
        //================================================================================
        /// <summary>
        /// AudioClip の長さ（秒）を返します
        /// </summary>
        public float Length { get; }

        /// <summary>
        /// AudioClip の再生が完了するまで await する UniTask を返します
        /// </summary>
        public UniTask Task { get; }

        //================================================================================
        // 関数
        //================================================================================
        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal StoppableAudioHandle
        (
            float   length,
            UniTask task,
            Action  onStop
        )
        {
            Length   = length;
            Task     = task;
            m_onStop = onStop;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StoppableAudioHandle() : this
        (
            length: 0,
            task: UniTask.CompletedTask,
            onStop: null
        )
        {
        }

        /// <summary>
        /// 再生中の AudioClip を停止します
        /// </summary>
        public void Dispose()
        {
            if ( m_isDisposed ) return;
            m_isDisposed = true;

            m_onStop?.Invoke();
        }
    }
}