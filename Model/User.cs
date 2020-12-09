using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DatingApp.API.Enum;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Model
{
    public class User : IdentityUser<int>, IUser
    {
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

        private ICollection<UserLike> likedUsers;

        private ICollection<UserLike> likedByUsers;

        private ICollection<Message> receivedMessages;

        private ICollection<Message> sentMessages;

        private ICollection<UserRole> roles;

        [NotMapped]
        public string PlainPassword { get; set; }

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

        public ICollection<UserLike> LikedUsers
        {
            get => likedUsers;
            set => likedUsers = value;
        }

        public ICollection<UserLike> LikedByUsers
        {
            get => likedByUsers;
            set => likedByUsers = value;
        }

        public ICollection<Message> ReceivedMessages
        {
            get => receivedMessages;
            set => receivedMessages = value;
        }

        public ICollection<Message> SentMessages
        {
            get => sentMessages;
            set => sentMessages = value;
        }

        public ICollection<UserRole> Roles
        {
            get => this.roles;
            set => this.roles = value;
        }
    }
}