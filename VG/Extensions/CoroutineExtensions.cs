using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace VG.Extensions
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public static class CoroutineExtensions
    {
        public static Coroutine WaitForSeconds<TMono>(this TMono owner, float seconds, Action onEnd)
            where TMono : MonoBehaviour
        {
            if (owner && owner.isActiveAndEnabled)
                return owner.StartCoroutine(IE_WaitForSeconds(seconds, onEnd));

            onEnd?.Invoke();
            return null;
        }

        public static Coroutine WaitForSecondsRealtime<TMono>(this TMono owner, float seconds, Action onEnd)
            where TMono : MonoBehaviour
        {
            if (owner && owner.isActiveAndEnabled)
                return owner.StartCoroutine(IE_WaitForSecondsRealtime(seconds, onEnd));

            onEnd?.Invoke();
            return null;
        }

        public static Coroutine WaitForFrames<TMono>(this TMono owner, int frames, Action onEnd)
            where TMono : MonoBehaviour
        {
            if (owner && owner.isActiveAndEnabled)
                return owner.StartCoroutine(IE_WaitForFrames(frames, onEnd));

            onEnd?.Invoke();
            return null;
        }

        public static Coroutine WaitForFixedFrames<TMono>(this TMono owner, int frames, Action onEnd)
            where TMono : MonoBehaviour
        {
            if (owner && owner.isActiveAndEnabled)
                return owner.StartCoroutine(IE_WaitForFixedFrames(frames, onEnd));

            onEnd?.Invoke();
            return null;
        }

        public static Coroutine WaitWhile<TMono>(this TMono owner, Func<bool> predicate, Action onEnd)
            where TMono : MonoBehaviour
        {
            if (owner && owner.isActiveAndEnabled)
                return owner.StartCoroutine(IE_WaitWhile(predicate, onEnd));

            onEnd?.Invoke();
            return null;
        }

        public static Coroutine WaitUntil<TMono>(this TMono owner, Func<bool> predicate, Action onEnd)
            where TMono : MonoBehaviour
        {
            if (owner && owner.isActiveAndEnabled)
                return owner.StartCoroutine(IE_WaitUntil(predicate, onEnd));

            onEnd?.Invoke();
            return null;
        }

        public static void StopWaiting<TMono>(this TMono owner, ref Coroutine ref_coroutine) where TMono : MonoBehaviour
        {
            if (owner &&
                ref_coroutine != null)
            {
                owner.StopCoroutine(ref_coroutine);
                ref_coroutine = null;
            }
        }

        private static IEnumerator IE_WaitForSeconds(float seconds, Action onEnd)
        {
            if (seconds > 0.0f)
                yield return new WaitForSeconds(seconds);

            onEnd?.Invoke();
        }

        private static IEnumerator IE_WaitForSecondsRealtime(float seconds, Action onEnd)
        {
            if (seconds > 0.0f)
                yield return new WaitForSecondsRealtime(seconds);

            onEnd?.Invoke();
        }

        private static IEnumerator IE_WaitForFrames(int frames, Action onEnd)
        {
            if (frames > 0)
            {
                var waitForEndOfFrame = new WaitForEndOfFrame();

                while (frames > 0)
                {
                    frames--;
                    yield return waitForEndOfFrame;
                }
            }

            onEnd?.Invoke();
        }

        private static IEnumerator IE_WaitForFixedFrames(int frames, Action onEnd)
        {
            if (frames > 0)
            {
                var waitForFixedUpdate = new WaitForFixedUpdate();

                while (frames > 0)
                {
                    frames--;
                    yield return waitForFixedUpdate;
                }
            }

            onEnd?.Invoke();
        }

        private static IEnumerator IE_WaitWhile(Func<bool> predicate, Action onEnd)
        {
            if (predicate?.Invoke() ?? false)
                yield return new WaitWhile(predicate);

            onEnd?.Invoke();
        }

        private static IEnumerator IE_WaitUntil(Func<bool> predicate, Action onEnd)
        {
            if ((predicate?.Invoke() ?? true) == false)
                yield return new WaitUntil(predicate);

            onEnd?.Invoke();
        }
    }
}