﻿using UnityEngine;
using UnityEngine.UI;
namespace PingAk9
{
    /// <summary>
    /// Small script that makes it easy to create looping 2D sprite animations.
    /// </summary>
    public class UIImageAnimation : MonoBehaviour
    {
        /// <summary>
        /// Index of the current frame in the sprite animation.
        /// </summary>

        public int frameIndex = 0;

        /// <summary>
        /// How many frames there are in the animation per second.
        /// </summary>

        [SerializeField] protected int framerate = 20;

        /// <summary>
        /// Should this animation be affected by time scale?
        /// </summary>

        public bool ignoreTimeScale = true;

        /// <summary>
        /// Should this animation be looped?
        /// </summary>

        public bool loop = true;

        /// <summary>
        /// Actual sprites used for the animation.
        /// </summary>

        public Sprite[] frames;

        SpriteRenderer mUnitySprite;
        Image mImage;
        float mUpdate = 0f;

        /// <summary>
        /// Returns is the animation is still playing or not
        /// </summary>

        public bool isPlaying { get { return enabled; } }

        /// <summary>
        /// Animation framerate.
        /// </summary>

        public int framesPerSecond { get { return framerate; } set { framerate = value; } }

        /// <summary>
        /// Continue playing the animation. If the animation has reached the end, it will restart from beginning
        /// </summary>

        public void Play()
        {
            if (frames != null && frames.Length > 0)
            {
                if (!enabled && !loop)
                {
                    int newIndex = framerate > 0 ? frameIndex + 1 : frameIndex - 1;
                    if (newIndex < 0 || newIndex >= frames.Length)
                        frameIndex = framerate < 0 ? frames.Length - 1 : 0;
                }

                enabled = true;
                UpdateSprite();
            }
        }

        /// <summary>
        /// Pause the animation.
        /// </summary>

        public void Pause() { enabled = false; }

        /// <summary>
        /// Reset the animation to the beginning.
        /// </summary>

        public void ResetToBeginning()
        {
            frameIndex = framerate < 0 ? frames.Length - 1 : 0;
            UpdateSprite();
        }

        /// <summary>
        /// Start playing the animation right away.
        /// </summary>

        void Start() { Play(); }

        /// <summary>
        /// Advance the animation as necessary.
        /// </summary>

        void Update()
        {
            if (frames == null || frames.Length == 0)
            {
                enabled = false;
            }
            else if (framerate != 0)
            {
                float time = ignoreTimeScale ? Time.fixedTime : Time.time;

                if (mUpdate < time)
                {
                    mUpdate = time;
                    int newIndex = framerate > 0 ? frameIndex + 1 : frameIndex - 1;

                    if (!loop && (newIndex < 0 || newIndex >= frames.Length))
                    {
                        enabled = false;
                        return;
                    }

                    frameIndex = RepeatIndex(newIndex, frames.Length);
                    UpdateSprite();
                }
            }
        }
        int RepeatIndex(int val, int max)
        {
            if (max < 1) return 0;
            while (val < 0) val += max;
            while (val >= max) val -= max;
            return val;
        }
        /// <summary>
        /// Immediately update the visible sprite.
        /// </summary>

        void UpdateSprite()
        {
            if (mUnitySprite == null && mImage == null)
            {
                mUnitySprite = GetComponent<UnityEngine.SpriteRenderer>();
                mImage = GetComponent<Image>();

                if (mUnitySprite == null && mImage == null)
                {
                    enabled = false;
                    return;
                }
            }

            float time = ignoreTimeScale ? Time.fixedTime : Time.time;
            if (framerate != 0) mUpdate = time + Mathf.Abs(1f / framerate);

            if (mUnitySprite != null)
            {
                mUnitySprite.sprite = frames[frameIndex];
            }
            else if (mImage != null)
            {
                mImage.sprite = frames[frameIndex];
            }
        }
    }
}