namespace Kaboom.Sources
{
    class ScoreManager
    {
        #region Singleton
        private static ScoreManager instance_;

        public static ScoreManager Instance
        {
            get { return instance_ ?? (instance_ = new ScoreManager()); }
        }
        #endregion

        private int score_;

        public int Score
        {
            get { return score_; }
            set
            {
                if (inGame_)
                    score_ = value;
            }
        }

        private int explodedBlocks_;
        private int explodedBombs_;
        private bool inGame_;

        public void Restart(int availableTunrs)
        {
            inGame_ = true;
            Score = 0;
            explodedBombs_ = 0;
            explodedBlocks_ = 0;
        }

        public void EntityDestructed(Entity entity)
        {
            if (entity is Block)
            {
                ++explodedBlocks_;
                Score += 50;
            }
            else if (entity is Bomb)
            {
                ++explodedBombs_;
            }
        }

        public void EndOfTurn(int bombsAvailable)
        {
            Score += (explodedBlocks_ * explodedBlocks_) * 5;
            if (explodedBombs_ != 0)
                Score += (20 * bombsAvailable / explodedBombs_);
            explodedBlocks_ = 0;
            explodedBombs_ = 0;
        }

        public void CheckPointDiscovered()
        {
            Score += 200;
        }

        public void EndReached(int turnsRemaining)
        {
            Score += (turnsRemaining * turnsRemaining * 100);
            inGame_ = false;
        }
    }
}
