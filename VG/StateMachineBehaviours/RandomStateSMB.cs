using UnityEngine;

namespace VG.StateMachineBehaviours
{
    public class RandomStateSMB : StateMachineBehaviour
    {
        public string parameter = "RandomIdle";

        public int numberOfStates = 3;
        public float minNormTime;
        public float maxNormTime = 5f;

        private int _hashParam;
        private float _randomNormTime;
        private int _shuffledIndex;

        private int[] _shuffledNumbers;


        private void Awake()
        {
            _hashParam = Animator.StringToHash(parameter);

            if (numberOfStates > 1)
            {
                _shuffledNumbers = new int[numberOfStates];

                for (var i = 0; i < numberOfStates; i++) _shuffledNumbers[i] = i + 1;

                _shuffledNumbers?.Shuffle();
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Randomly decide a time at which to transition.
            _randomNormTime = Random.Range(minNormTime, maxNormTime);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetInteger(_hashParam, 0);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // If trainsitioning away from this state reset the random idle parameter to 0.
            if (animator.IsInTransition(layerIndex) && animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash ==
                stateInfo.fullPathHash) animator.SetInteger(_hashParam, 0);

            // If the state is beyond the randomly decided normalised time and not yet transitioning then set a random idle.
            if (stateInfo.normalizedTime > _randomNormTime && !animator.IsInTransition(layerIndex))
            {
                if (numberOfStates > 1)
                {
                    if (_shuffledIndex >= numberOfStates)
                    {
                        _shuffledIndex = 0;
                        _shuffledNumbers.Shuffle();
                    }

                    animator.SetInteger(_hashParam, _shuffledNumbers[_shuffledIndex++]);
                }
                else
                {
                    animator.SetInteger(_hashParam, Random.Range(1, numberOfStates + 1));
                }
            }
        }
    }

    public static class ArrayExtensions
    {
        public static T[] Shuffle<T>(this T[] array)
        {
            var n = array.Length;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (array[k], array[n]) = (array[n], array[k]);
            }

            return array;
        }
    }
}