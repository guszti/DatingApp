using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DatingApp.API.Enum;

namespace DatingApp.API.Model
{
    public class User : IUser
    {
        private int id;

        private string username;

        private byte[] password;

        private byte[] salt;

        private DateTime createdAt;

        private DateTime updatedAt;

        private Gender gender;

        private DateTime dateOfBirth;

        private string knownAs;

        private DateTime lastActive;

        private string introduction;

        private string lookingFor;

        private string interests;

        private string city;

        private string country;

        private ICollection<Photo> photos;

        public User()
        {
            this.photos = new Collection<Photo>();
        }
        
        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

        public byte[] Password
        {
            get => password;
            set => password = value;
        }

        public byte[] Salt
        {
            get => salt;
            set => salt = value;
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

        public Gender Gender
        {
            get => gender;
            set => gender = value;
        }

        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set => dateOfBirth = value;
        }

        public string KnownAs
        {
            get => knownAs; 
            set => knownAs = value;
        }
        
        public DateTime LastActive { 
            get => lastActive;
            set => lastActive = value; 
        }

        public string Introduction
        {
            get => introduction;
            set=> introduction = value;
        }

        public string LookingFor
        {
            get => lookingFor;
            set => lookingFor = value;
        }

        public string Interests
        {
            get => interests;
            set => interests = value;
        }


        public string City
        {
            get => city;
            set => city = value;
        }

        public string Country
        {
            get => country;
            set => country = value;
        }

        public ICollection<Photo> Photos
        {
            get => photos;
            set => photos = value;
        }
    }
}