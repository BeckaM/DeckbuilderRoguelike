using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [Serializable]
    public class PlayerClass
    {
        public string ClassName;
        public int SpriteIcon;
        public List<string> Startingdeck;
                
                   
    }

    [Serializable]
    public class PlayerClassWrapper
    {
        public List<PlayerClass> PlayerClasses;
    }
}