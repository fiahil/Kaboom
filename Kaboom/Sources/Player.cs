namespace Kaboom.Sources
{
    /// <summary>
    /// Manage player state
    /// </summary>
    class Player
    {
        private Map map_;

        /// <summary>
        /// Initialize new player with default value
        /// </summary>
        /// <param name="map">Map reference</param>
        /// <param name="name">Player's name</param>
        /// <param name="play">Define if the player will play his first turn</param>
        public Player(Map map, string name, bool play = false)
        {
            this.map_ = map;
            this.Name = name;
            this.BombSelected = null;
            this.TurnToPlay = play;
        }

        /// <summary>
        /// Player's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Return true if this player can play
        /// </summary>
        public bool TurnToPlay { get; set; }

        /// <summary>
        /// Return the actual picked up bomb or null if none selected
        /// </summary>
        public Entity BombSelected { get; set; }

        #region Unitest
        /// <summary>
        /// Player Unitest
        /// </summary>
        public static void Unitest()
        {
            var p1 = new Player(null, "Boris", true);
            var p2 = new Player(null, "Georges");

            if (p1.TurnToPlay)
            {
                p1.BombSelected = new Entity(0, KaboomResources.Sprites["Ground"].Clone() as SpriteSheet);
                p1.TurnToPlay = false;
            }
            else
            {
                p1.TurnToPlay = true;
            }

            if (p2.TurnToPlay)
            {
                p2.BombSelected = new Entity(0, KaboomResources.Sprites["DestructibleBlock"].Clone() as SpriteSheet);
                p2.TurnToPlay = false;
            }
            else
            {
                p2.TurnToPlay = true;
            }

            //Put your breakpoint here
        }
        #endregion
    }
}
