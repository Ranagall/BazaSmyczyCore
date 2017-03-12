namespace BazaSmyczy.Core.Consts
{
    public class EventsIds
    {
        public class Account
        {
            public const int AdminLogged = 50;
            public const int Created = 60;
            public const int ChangedPassword = 65;
            public const int SetPassword = 66;
            public const int Deleted = 70;
            public const int LockedOut = 71;
        }

        public class Leash
        {
            public const int Created = 300;
            public const int Edited = 305;
            public const int Deleted = 310;
        }

        public class File
        {
            public const int Saved = 400;
            public const int SaveFailed = 401;
            public const int Deleted = 410;
            public const int InvalidExt = 415;
            public const int InvalidContentType = 416;
            public const int InvalidSignature = 417;
        }
    }
}
