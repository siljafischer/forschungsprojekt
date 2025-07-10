// classes

using System;
using UnityEngine;

namespace Assets.Models
{
    // user class
    [Serializable]
    public class DiaryDiaryentry
    {
        // attributes
        [SerializeField] public string id_diary { get; set; }
        [SerializeField] public string id_diaryentry { get; set; }
        [SerializeField] public string date { get; set; }
        [SerializeField] public string taken_picture { get; set; }

        // events for databinding
        public event Action<string> OnIdDiaryChanged;
        public event Action<string> OnIdDiaryentryChanged;
        public event Action<string> OnDateChanged;
        public event Action<string> OnTakenPictureChanged;

        // getter, setter
        public string IdDiary
        {
            get => id_diary;
            set
            {
                if (id_diary != value)
                {
                    id_diary = value;
                    OnIdDiaryChanged?.Invoke(id_diary);
                }
            }
        }
        public string IdDiaryentry
        {
            get => id_diaryentry;
            set
            {
                if (id_diaryentry != value)
                {
                    id_diaryentry = value;
                    OnIdDiaryChanged?.Invoke(id_diaryentry);
                }
            }
        }
        public string Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;
                    OnDateChanged?.Invoke(date);
                }
            }
        }
        public string TakenPicture
        {
            get => taken_picture;
            set
            {
                if (taken_picture != value)
                {
                    taken_picture = value;
                    OnTakenPictureChanged?.Invoke(taken_picture);
                }
            }
        }
    }
}
