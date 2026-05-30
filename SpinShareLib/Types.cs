using System;
using System.Text.Json.Serialization;
// ReSharper disable UnassignedField.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable CollectionNeverQueried.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace SpinShareLib 
{
    namespace Types 
    {
        public class Content
        {
            public int version;
            public int status;
        }
        
        public class Content<T> : Content
        {
            public T data;
        }

        public class Promo
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
            public class Button
            {
                public int type;
                public string data;
            }
        }

        public class Song
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
        public class SongDetail : Song
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
            public class Paths
            {
                public string
                    ogg,
                    cover,
                    zip;
            }
        }
        public class SongDetailTournament : SongDetail
        {
            public string srtbMD5;
        }
        public class Reviews
        {
            public bool average;
            public Review[] reviews;
            public class Review
            {
                public int id;
                public SongDetail song;
                public User user;
                public bool recommended;
                public string comment;
                public Date reviewDate;
            }
        }
        public class SpinPlays
        {
            public Spinplay[] spinPlays;
            public class Spinplay
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
        public class Playlist
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
        public class UserDetail : User
        {
            public string pronouns;
            public int?
                songs,
                playlists,
                reviews,
                spinplay;
            public Card[] cards;
            public class Card
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

        public class Search
        {
            public Song[] songs;
            public User[] users;
        }

        public class Date
        {
            public DateTime date;
            public string stimezone;
            public int? timezone_type;
        }
    }
}