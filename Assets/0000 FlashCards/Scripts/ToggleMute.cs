﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Flashcards
{
	/// <summary>
	/// Toggles a sound source when clicked on. It also records the sound state (on/off) in a PlayerPrefs. 
	/// In order to detect clicks you need to attach this script to a UI Button and set the proper OnClick() event.
	/// </summary>
	public class ToggleMute : MonoBehaviour
	{
        //The tag of the sound object
        public string soundObjectTag = "Sound";

        // The source of the sound
        public Transform soundObject;

        // The PlayerPrefs name of the sound
        public string playerPref = "SoundVolume";

        // The index of the current value of the sound
        internal float currentState = 1;

        [Tooltip("The volume when this sound button is toggled on")]
        public float volumeOn = 1;

        [Tooltip("The volume when this sound button is toggled off")]
        public float volumeOff = 0;

        public Sprite onSprite, offSprite;

        Image image;


        private void Awake() => image = GetComponent<Image>();

        void OnEnable()
        {
            //if (!soundObject && soundObjectTag != string.Empty) { soundObject = GameObject.FindGameObjectWithTag(soundObjectTag).transform; }
            if (!soundObject && soundObjectTag != string.Empty) { soundObject = GameObject.FindGameObjectWithTag(soundObjectTag).transform; }
            else { Debug.Log("No music object found, ignore if there isn't any music in the scene"); }
            // Get the current state of the sound from PlayerPrefs
            if (soundObject)
                currentState = PlayerPrefs.GetFloat(playerPref, soundObject.GetComponent<AudioSource>().volume);
            else
                currentState = PlayerPrefs.GetFloat(playerPref, currentState);

            // add the audio player so this button plays a toggle sound effect everytime you touch it
            gameObject.AddComponent<AudioSource>();

            // Set the sound in the sound source
            SetSound();
        }

        /// <summary>
        /// Sets the sound volume
        /// </summary>
        public void SetSound()
        {
            if (!soundObject && soundObjectTag != string.Empty) soundObject = GameObject.FindGameObjectWithTag(soundObjectTag).transform;

            // Set the sound in the PlayerPrefs
            PlayerPrefs.SetFloat(playerPref, currentState);

            Color newColor = image.material.color;

            // Update the graphics of the button image to fit the sound state
            if (currentState == volumeOn)
            {
                newColor.a = 1;
                image.sprite = onSprite;
            }
            else
            {
                newColor.a = 0.5f;
                image.sprite = offSprite;
            }

            image.color = newColor;

            // Set the value of the sound state to the source object
            if (soundObject)
                soundObject.GetComponent<AudioSource>().volume = currentState;
        }

        /// <summary>
        /// Toggle the sound. Cycle through all sound modes and set the volume and icon accordingly
        /// </summary>
        public void ToggleSound()
        {
            // turn the volume on or off
            if (currentState == volumeOn)
            {
                currentState = volumeOff;
                this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("turnoff"));
            }
            else
            {
                currentState = volumeOn;
                this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("turnon"));
            }

            SetSound();
        }

        /// <summary>
        /// Starts the sound source.
        /// </summary>
        public void StartSound()
        {
            if (soundObject)
                soundObject.GetComponent<AudioSource>().Play();
        }
    }
}
