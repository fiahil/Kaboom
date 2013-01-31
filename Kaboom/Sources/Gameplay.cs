namespace Kaboom.Sources
{
    /// <summary>
    /// Manage game interaction and logic
    /// </summary>
    class Gameplay
    {
        /// <summary>
        /// Called on entity creation
        /// </summary>
        /// <param name="entity">Entity created</param>
        /// <returns>Success or failure</returns>
        public bool OnNewEntity(IEntity entity)
        {
            return true;
        }

        /// <summary>
        /// Called on entity destruction
        /// </summary>
        /// <param name="entity">Entity destroyed</param>
        /// <returns>Success or failure</returns>
        public bool OnDestroyEntity(IEntity entity)
        {
            return true;
        }

        /// <summary>
        /// Called on bomb pickup
        /// </summary>
        /// <param name="entity">Entity picked up</param>
        /// <param name="player">Player who picked up the entity</param>
        /// <returns></returns>
        public bool OnPickUp(IEntity entity, Player player)
        {
            //TODO: Replace IEntity with IBomb
            return true;
        }

        /// <summary>
        /// Called on bomb explosion. Propagation is launched once within this function
        /// </summary>
        /// <param name="bomb">Exploded bomb</param>
        /// <returns>Success or failure</returns>
        public bool OnExplosion(IEntity bomb)
        {
            //TODO: Replace IEntity with IBomb
            return true;
        }

        /// <summary>
        /// Called on checkpoint activation
        /// </summary>
        /// <param name="checkpoint">Activated checkpoint</param>
        /// <returns>Success or failure</returns>
        public bool OnActivation(IEntity checkpoint)
        {
            //TODO: Replace IEntity with IActivable/ICheckpoint
            return true;
        }

        /// <summary>
        /// Called on item collect
        /// </summary>
        /// <param name="item">Picked-up bonus/malus</param>
        /// <returns>Success or failure</returns>
        public bool OnCollect(IEntity item)
        {
            //TODO: Replace IEntity with ICollectable
            return true;
        }
    }
}