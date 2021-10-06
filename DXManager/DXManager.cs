using System;
using System.Drawing;
using DxLibDLL;

namespace TakamineProduction
{
	/// <summary>DXライブラリの必須処理やよく使いそうな設定を簡略化するクラス。</summary>
	public class DXManager
	{
		/// <summary>コンストラクタ（SetOutApplicationLogValidFlagを実行する）</summary>
		/// /// <param name="outLog">ログ出力を行うかのフラグ</param>
		public DXManager(bool outLog)
		=> DX.SetOutApplicationLogValidFlag(outLog ? 1 : 0);

		/// <summary>デストラクタ（DxLib_Endを実行する）</summary>
		~DXManager() => End();

		/// <summary>DXライブラリの初期化処理を行う（初期化前にいくつかの関数を実行する）</summary>
		/// <param name="drawMode">描画モード</param>
		/// <param name="sysCommandOff">システムのキーフックを無効にするかのフラグ（AltキーとF10キーで処理が止まるのを防げる）</param>
		/// <param name="waitVSync">ScreenFlip実行時、ＣＲＴの垂直同期信号待ちをするかのフラグ</param>
		/// <param name="allowDoubleStart">多重起動を許可するかのフラグ</param>
		/// <returns>DxLib_Initの戻り値</returns>
		public int Init(DXDrawMode drawMode = DXDrawMode.Nearest, bool sysCommandOff = true, bool waitVSync = true, bool allowDoubleStart = false)
		{
			DX.SetDoubleStartValidFlag(allowDoubleStart ? 1 : 0);
			DX.SetWaitVSyncFlag(waitVSync ? 1 : 0);
			DX.SetSysCommandOffFlag(sysCommandOff ? 1 : 0);

			var resp = DX.DxLib_Init();
			if (resp != -1)
			{
				DX.SetDrawMode((int)drawMode);
				DX.SetSysCommandOffFlag(sysCommandOff ? 1 : 0);
			}
			return resp;
		}
		/// <summary>DxLib_Endを実行する。通常は使わなくてよい。（デストラクタで自動的に終了する）</summary>
		/// <returns>DxLib_Endの戻り値</returns>
		public int End() => DX.DxLib_End();
		/// <summary>ProcessMessageを実行する。</summary>
		/// <returns>ProcessMessageの戻り値が0であるかが返る</returns>
		public bool ProcMSG() => DX.ProcessMessage() == 0;
		/// <summary>初期化処理済みフラグ</summary>
		public bool IsInit => DX.DxLib_IsInit() != 0;
		/// <summary>最後にsetしたプロパティで使われた関数の戻り値</summary>
		public int LastReturn { get; private set; }
		/// <summary>メインウィンドウのタイトル</summary>
		public string MainWindowText { set => LastReturn = DX.SetMainWindowText(value); }
		/// <summary>メインウィンドウのアイコン</summary>
		public IntPtr MainWindowIcon { set => LastReturn = DX.SetWindowIconHandle(value); }
		/// <summary>画面モード</summary>
		public (int width, int height, int bit) GraphMode { set => LastReturn = DX.SetGraphMode(value.width, value.height, value.bit); }
		/// <summary>ウィンドウモード時の画面・描画領域の大きさの倍率</summary>
		public double WindowSizeExtendRate { set => LastReturn = DX.SetWindowSizeExtendRate(value); }
		/// <summary>画面モードやウインドウモード変更時、グラフィック設定やグラフィックハンドルをリセットするフラグ</summary>
		public bool ChangeScreenModeGraphicsSystemReset { set => LastReturn = DX.SetChangeScreenModeGraphicsSystemResetFlag(value ? 1 : 0); }
		/// <summary>読み込む画像を乗算済みα画像に変換するフラグ</summary>
		public bool UsePremulAlphaConvertLoad { set => LastReturn = DX.SetUsePremulAlphaConvertLoad(value ? 1 : 0); }
		/// <summary>ウィンドウの背景色や描画可能グラフィックのクリアカラー</summary>
		public Color BackgroundColor
		{
			get
			{
				DX.GetBackgroundColor(out int r, out int g, out int b, out int a);
				return Color.FromArgb(a, r, g, b);
			}
			set => LastReturn = DX.SetBackgroundColor(value.R, value.G, value.B, value.A);
		}
		/// <summary>透過色（Aは無視される）</summary>
		public Color TransColor
		{
			get
			{
				DX.GetTransColor(out int r, out int g, out int b);
				return Color.FromArgb(255, r, g, b);
			}
			set => LastReturn = DX.SetTransColor(value.R, value.G, value.B);
		}
		/// <summary>ウィンドウモードで表示するフラグ</summary>
		public bool WindowMode
		{
			get => DX.GetWindowModeFlag() == 1;
			set => LastReturn = DX.ChangeWindowMode(value ? 1 : 0);
		}
		/// <summary>アクティブでなくても処理を続行するフラグ</summary>
		public bool AlwaysRun
		{
			get => DX.GetAlwaysRunFlag() == 1;
			set => LastReturn = DX.SetAlwaysRunFlag(value ? 1 : 0);
		}
		/// <summary>Startで実行される関数(引数のインスタンスはこのオブジェクト自身が渡される。)</summary>
		public Action<DXManager> EntryPoint { protected get; set; }
		/// <summary>EntryPointを実行する(引数のインスタンスはこのオブジェクト自身が渡される。)</summary>
		public void Start() => EntryPoint?.Invoke(this);
		
		/// <summary>描画モード</summary>
		public enum DXDrawMode
		{
			/// <summary>ニアレストネイバー法</summary>
			Nearest = DX.DX_DRAWMODE_NEAREST,
			/// <summary>バイリニア法</summary>
			Bilinear = DX.DX_DRAWMODE_BILINEAR
		}
	}
}