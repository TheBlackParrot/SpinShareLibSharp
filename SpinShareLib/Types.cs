using System;
using System.Text.Json.Serialization;
// ReSharper disable UnassignedField.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable CollectionNeverQueried.Global

namespace SpinShareLib 
{
    namespace Types 
    {
        public abstract class Content
        {
            public int version;
            public int status;
        }

        public abstract class Content<T> : Content
        {
            public T data;
        }

        public abstract class Promo
        {
            public int id;
            public string
                title,
                type,
                textColor,
                color,
                image_path;
            public bool isVisible;
            public Button button;
            public abstract class Button
            {
                public int type;
                public string data;
            }
        }

        public abstract class Song
        {
            public int id;
            public int? 
                easyDifficulty,
                normalDifficulty,
                hardDifficulty,
                expertDifficulty,
                XDDifficulty;
            public bool
                hasEasyDifficulty,
                hasNormalDifficulty,
                hasHardDifficulty,
                hasExpertDifficulty,
                hasXDDifficulty;
            public string 
                title,
                subtitle,
                artist,
                charter,
                updateHash,
                cover,
                zip;
        }
        public abstract class SongDetail : Song
        {
            public int?
                uploader,
                views,
                downloads,
                publicationStatus;
            public string 
                description,
                fileReference;
            [JsonConverter(typeof(StrObjectToArr))]
            public string[] tags;
            public Date uploadDate;
            public Date updateDate;
            public Paths paths;
            public abstract class Paths
            {
                public string
                    ogg,
                    cover,
                    zip;
            }
        }
        public abstract class SongDetailTournament : SongDetail
        {
            public string srtbMD5;
        }
        public abstract class Reviews
        {
            public bool average;
            public Review[] reviews;
            public abstract class Review
            {
                public int id;
                public SongDetail song;
                public User user;
                public bool recommended;
                public string comment;
                public Date reviewDate;
            }
        }
        public abstract class SpinPlays
        {
            public Spinplay[] spinPlays;
            public abstract class Spinplay
            {
                public int id;
                public User user;
                public bool? isActive;
                public Date submitDate;
                public string
                    videoUrl,
                    videoThumbnail;

            }
        }
        public abstract class Playlist
        {
            public int id;
            public string
                title,
                description,
                fileReference,
                cover;
            public User user;
            public SongDetail[] songs;
            public bool? isOfficial;
        }
        
        public class User
        {
            public int id;
            public bool?
                isVerified,
                isPatreon;
            public string
                username,
                avatar;
        }
        public abstract class UserDetail : User
        {
            public string pronouns;
            public int?
                songs,
                playlists,
                reviews,
                spinplay;
            public Card[] cards;
            public abstract class Card
            {
                public int id;
                public string
                    icon,
                    title,
                    description;
                public Date givenDate;
                public int?
                    songs,
                    playlists,
                    reviews,
                    spinplay;
            }
        }

        public abstract class Search
        {
            public Song[] songs;
            public User[] users;
        }

        public abstract class Date
        {
            public DateTime date;
            public string stimezone;
            public int? timezone_type;
        }
    }
}