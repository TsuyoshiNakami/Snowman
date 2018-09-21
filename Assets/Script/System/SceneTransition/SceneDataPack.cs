namespace Tsuyomi.Yukihuru.Scripts.Utilities.SceneDataPacks
{
    public abstract class SceneDataPack
    {
        /// <summary>
        /// 前のシーン
        /// </summary>
        public abstract GameScenes PreviousGameScene { get; }

        /// <summary>
        /// 前シーンで追加ロードしていたシーン一覧
        /// </summary>
        public abstract GameScenes[] PreviousAdditiveScene { get; }
    }

    public class DefaultSceneDataPack : SceneDataPack
    {
        private readonly GameScenes _prevGameScenes;
        private readonly GameScenes[] _additiveScenes;

        public GameScenes[] AdditiveScenes
        {
            get { return _additiveScenes; }
        }

        public override GameScenes PreviousGameScene
        {
            get
            {
                { return  _prevGameScenes; }
            }
        }

        public override GameScenes[] PreviousAdditiveScene
        {
            get
            {
                return null;
            }
        }

        public DefaultSceneDataPack(GameScenes prev, GameScenes[] additive)
        {
            _prevGameScenes = prev;
            _additiveScenes = additive;
        }
    }
}