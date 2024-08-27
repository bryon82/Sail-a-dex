using System.Collections.Generic;
using UnityEngine;

namespace sailadex
{
    public class NotificationUiQueue : MonoBehaviour
    {
        public static NotificationUiQueue instance;
        private float timer;
        private Queue<string> queue;
        private AudioSource audioSource;

        public void Start()
        {
            instance = this;
            queue = new Queue<string>();
            timer = 0f;

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = Plugin.notificationSoundVolume.Value;
            audioSource.spatialBlend = 1.0f;
            audioSource.minDistance = 10f;
            audioSource.maxDistance = 20f;
        }

        private void Update()
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 0f;
                if (queue.Count > 0) 
                {
                    NotificationUi.instance.ShowNotification(queue.Dequeue());
                    if (Plugin.notificationSoundVolume.Value > 0f)
                        audioSource.PlayOneShot(AssetsLoader.notificationSound);
                    timer = 3f;
                }
            }
        }

        public void QueueNotification(string message) 
        {
            queue.Enqueue(message);
        }
    }
}
