using System;

namespace DatingApp.API.Model
{
    public class Photo : IPhoto
    {
        private int id;

        private DateTime createdAt;

        private DateTime updatedAt;

        private bool isMain;

        private string url;

        private string description;

        private int userId;

        private User user;

        public int Id
        {
            get => id;
            set => id = value;
        }

        public DateTime CreatedAt
        {
            get => createdAt; 
            set => createdAt = value;
        }

        public DateTime UpdatedAt
        {
            get => updatedAt; 
            set => updatedAt = value;
        }

        public bool IsMain
        {
            get => isMain;
            set => isMain = value;
        }

        public string Url
        {
            get => url;
            set => url = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public int UserId
        {
            get => userId;
            set => userId = value;
        }

        public User User
        {
            get => user;
            set => user = value;
        }
    }
}