using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using NCMB;
using NCMB.Extensions;
using UniRx;
#if engineer
using Rewired;
#endif

namespace naichilab
{
	public class RankingSceneManager : MonoBehaviour
	{
		[SerializeField] Text scoreLabel;
		[SerializeField] Text highScoreLabel;
		[SerializeField] InputField nameInputField;
		[SerializeField] Button sendScoreButton;
		[SerializeField] Button closeButton;
		[SerializeField] RectTransform scrollViewContent;
		[SerializeField] GameObject rankingNodePrefab;
		[SerializeField] GameObject readingNodePrefab;
		[SerializeField] GameObject notFoundNodePrefab;

		private const string OBJECT_ID = "objectId";
		private string _objectid = null;

		private string ObjectID {
			get {
				return _objectid ?? (_objectid = PlayerPrefs.GetString (OBJECT_ID, null));
			}
			set {
				if (_objectid == value)
					return;
				PlayerPrefs.SetString (OBJECT_ID, _objectid = value);
			}
		}

		private const string RankingDataClassName = "RankingData";
		private NCMBObject highScoreSpreadSheetObject;


        Subject<Unit> closeRankingSubject = new Subject<Unit>();
        public IObservable<Unit> OnCloseRanking
        {
            get { return closeRankingSubject; }
        }

		/// <summary>
		/// 入力した名前
		/// </summary>
		/// <value>The name of the inputted.</value>
		private string InputtedNameForSave {
			get {
				if (string.IsNullOrEmpty (this.nameInputField.text)) {
					return "名無し";
				}
				return this.nameInputField.text;
			}
		}

#if engineer
        Player player;
#endif

		void Start ()
		{
#if engineer
            player = ReInput.players.GetPlayer(0);
#endif
			this.sendScoreButton.interactable = false;

			StartCoroutine (GetHighScoreAndRankingBoard ());
		}

        private void Update()
        {
#if engineer
            if(player.GetButtonDown("Jump") || player.GetButtonDown("Home"))
#else
            if(Input.GetButtonDown(KeyConfig.Jump) || Input.GetButtonDown(KeyConfig.Home))
#endif
            {
                OnCloseButtonClick();
            }
        }

        IEnumerator GetHighScoreAndRankingBoard ()
		{
			this.scoreLabel.text = RankingLoader.Instance.Score.TextForDisplay;
				
			//ハイスコア取得
			{
				this.highScoreLabel.text = "取得中...";

				var hiScoreCheck = new YieldableNcmbQuery<NCMBObject> (RankingDataClassName);
				hiScoreCheck.WhereEqualTo (OBJECT_ID, ObjectID);
				yield return hiScoreCheck.FindAsync ();

				if (hiScoreCheck.Count > 0) {
					//既にハイスコアは登録されている
					highScoreSpreadSheetObject = hiScoreCheck.Result.First ();

					var s = RankingLoader.Instance.BuildScore (highScoreSpreadSheetObject ["hiscore"].ToString ());
					this.highScoreLabel.text = s != null ? s.TextForDisplay : "エラー";

					this.nameInputField.text = highScoreSpreadSheetObject ["name"].ToString ();
				} else { 
					//登録されていない
					this.highScoreLabel.text = "-----";
				}
			}

			//ランキング取得
			yield return StartCoroutine (LoadRankingBoard ());

			//スコア更新している場合、ボタン有効化
			if (this.highScoreSpreadSheetObject == null) {
				this.sendScoreButton.interactable = true;
			} else {
				var highScore = RankingLoader.Instance.BuildScore (this.highScoreSpreadSheetObject ["hiscore"].ToString ());
				var score = RankingLoader.Instance.Score;

				if (RankingLoader.Instance.setting.Order == ScoreOrder.OrderByAscending) {
					//数値が低い方が高スコア
					this.sendScoreButton.interactable = score.Value < highScore.Value;
				} else {
					//数値が高い方が高スコア
					this.sendScoreButton.interactable = highScore.Value < score.Value;
				}
			}
		}


		public void SendScore ()
		{
			StartCoroutine (SendScoreEnumerator ());			
		}

		private IEnumerator SendScoreEnumerator ()
		{
			this.sendScoreButton.interactable = false;
			this.highScoreLabel.text = "送信中...";

			//ハイスコア送信
			if (this.highScoreSpreadSheetObject == null) {
				this.highScoreSpreadSheetObject = new NCMBObject (RankingDataClassName);
				this.highScoreSpreadSheetObject.ObjectId = ObjectID;
			}

			this.highScoreSpreadSheetObject ["name"] = this.InputtedNameForSave;
			this.highScoreSpreadSheetObject ["hiscore"] = RankingLoader.Instance.Score.Value;
			NCMBException errorResult = null;
			
			yield return this.highScoreSpreadSheetObject.YieldableSaveAsync (error => errorResult = error);

			if (errorResult != null) {  //NCMBのコンソールから直接削除した場合に、該当のobjectIdが無いので発生する（らしい）
				highScoreSpreadSheetObject.ObjectId = null;
				yield return this.highScoreSpreadSheetObject.YieldableSaveAsync (error => errorResult = error);	//新規として送信
			}

			//ObjectIDを保存して次に備える
			ObjectID = this.highScoreSpreadSheetObject.ObjectId;
			
			this.highScoreLabel.text = RankingLoader.Instance.Score.TextForDisplay;

			yield return StartCoroutine (LoadRankingBoard ());
		}


		/// <summary>
		/// ランキング取得＆表示
		/// </summary>
		/// <returns>The ranking board.</returns>
		private IEnumerator LoadRankingBoard ()
		{
			int nodeCount = scrollViewContent.childCount;
			Debug.Log (nodeCount);
			for (int i = nodeCount - 1; i >= 0; i--) {
				Destroy (scrollViewContent.GetChild (i).gameObject);
			}

			var msg = Instantiate (readingNodePrefab, scrollViewContent);

			//2017.2.0b3の描画されないバグ暫定対応
			MaskOffOn ();

			var so = new YieldableNcmbQuery<NCMBObject> (RankingDataClassName);
			so.Limit = 30;
			if (RankingLoader.Instance.setting.Order == ScoreOrder.OrderByAscending) {
				so.OrderByAscending ("hiscore");
			} else {
				so.OrderByDescending ("hiscore");
			}
			yield return so.FindAsync ();

			Debug.Log ("count : " + so.Count.ToString ());
			Destroy (msg);

			if (so.Count > 0) {

				int rank = 0;
				foreach (var r in so.Result) {

					var n = Instantiate (this.rankingNodePrefab, scrollViewContent);
					var rankNode = n.GetComponent<RankingNode> ();
					rankNode.NoText.text = (++rank).ToString ();
					rankNode.NameText.text = r ["name"].ToString ();

					var s = RankingLoader.Instance.BuildScore (r ["hiscore"].ToString ());
					rankNode.ScoreText.text = s != null ? s.TextForDisplay : "エラー";

					Debug.Log (r ["hiscore"].ToString ());
				}

			} else {
				Instantiate (this.notFoundNodePrefab, scrollViewContent);
			}
		}

		public void OnCloseButtonClick ()
		{
			this.closeButton.interactable = false;
            closeRankingSubject.OnNext(Unit.Default);
			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync ("Ranking");
		}

		private void MaskOffOn ()
		{
			//2017.2.0b3でなぜかScrollViewContentを追加しても描画されない場合がある。
			//親maskをOFF/ONすると直るので無理やり・・・
			var m = this.scrollViewContent.parent.GetComponent<Mask> ();
			m.enabled = false;
			m.enabled = true;
		}

	}
}
