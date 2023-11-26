using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public struct AnimationSequence
    {
        private Sprite[] spriteSequence;
    }

    public class SpriteAnimator : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private bool animating = false;
        private float frameTime;
        private Sprite[] spriteSequence;
        private Dictionary<string, AnimationSequence> animationsByName;
        private int currSequenceFrame = 0;
        private bool useGlobalTime;
        private float animationTime;
        public SpriteRenderer SpriteRenderer { get => spriteRenderer; }

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Init(Sprite[] spriteSequence, float frameTime, bool useGlobalTime = true)
        {
            this.spriteSequence = spriteSequence;
            this.frameTime = frameTime;
            this.useGlobalTime = useGlobalTime;
            Setup();
        }

        private void Setup()
        {
            animationTime = useGlobalTime ? Time.time : 0;
            currSequenceFrame = Mathf.FloorToInt(animationTime / frameTime) % spriteSequence.Length;
            spriteRenderer.sprite = spriteSequence[currSequenceFrame];
        }

        public void StartAnimation(bool reset = false)
        {
            if (reset) Setup();
            animating = true;
        }

        public void StopAnimation(bool reset = false)
        {
            if (reset) Setup();
            animating = false;
        }

        public void StartAnimating(bool reset = false)
        {
            if (reset) Setup();
            animating = true;
        }

        public void StopAnimating(bool reset = false)
        {
            if (reset) Setup();
            animating = false;
        }

        void Update()
        {
            if (animating)
            {
                animationTime = useGlobalTime ? Time.time : (animationTime + Time.deltaTime);
                int sequenceFrame = Mathf.FloorToInt(animationTime / frameTime) % spriteSequence.Length;
                if (sequenceFrame != currSequenceFrame)
                {
                    spriteRenderer.sprite = spriteSequence[currSequenceFrame = sequenceFrame];
                }
            }
        }
    }
}
