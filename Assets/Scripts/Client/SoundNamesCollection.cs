using System.Collections.Generic;
using Ji2Core.Core.Audio;

namespace Client
{
    public class SoundNamesCollection : ISoundNamesCollection
    {
        public const string Music = "Music";
        public const string ButtonTap = "ButtonTap";
        public const string ChangeDirection = "ChangeDirection";
        public const string EatFood = "EatFood";
        public const string SnakeCollision = "SnakeCollision";
        public const string WinScreenShow = "WinScreenShow";

        public IEnumerable<string> SoundsList => _names;

        private readonly string[] _names =
        {
            Music,
            ButtonTap,
            ChangeDirection,
            EatFood,
            SnakeCollision,
            WinScreenShow
        };
    }
}