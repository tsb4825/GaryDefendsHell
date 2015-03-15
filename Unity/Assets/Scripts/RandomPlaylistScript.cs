using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class RandomPlaylistScript : MonoBehaviour
    {
        public AudioClip[] PlayList;
        public int ListIndex;

        void Start()
        {
            PlayList = RandomizeList(PlayList);
        }

        void Update()
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().clip = PlayList[ListIndex];
                GetComponent<AudioSource>().Play();
                ListIndex++;
                if (ListIndex >= PlayList.Count())
                {
                    ListIndex = 0;
                }
            }
        }

        private AudioClip[] RandomizeList(AudioClip[] playList)
        {
            System.Random rng = new System.Random();
            int n = playList.Count();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                AudioClip value = playList[k];
                playList[k] = playList[n];
                playList[n] = value;
            }
            return playList;
        }
    }
}
