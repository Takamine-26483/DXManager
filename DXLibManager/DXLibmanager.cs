using System;
using DxLibDLL;

namespace TakamineProduction
{
	/// <summary>
	/// DXライブラリの起動・終了の処理を簡略化するクラス。
	/// </summary>
	public class DXLibManager
	{
		/// <summary>初期化済みかを表す</summary>
		public bool IsInitialized { get { return DX.DxLib_IsInit() != 0; } }

		/// <summary>デストラクタ（ここでDXLIBの終了）</summary>
		~DXLibManager() => End();

		/// <summary>DXLIBを直ちに終了する（通常はデストラクタで自動的に終了する）</summary>
		public int End() => DX.DxLib_End();


		/// <summary>マニュアル設定でDXLIBを初期化する。名前付き引数("引数名":"実引数")の使用を推奨</summary>
		/// <param name="scr">ウィンドウのサイズ(x,y)とそのビットレート(bit)</param>
		/// <param name="back">画面背景色RGB</param>
		/// <param name="sample">マルチサンプルの設定。マルチサンプルに使う画面の倍率(level)と綺麗さ具合(quality:0~3を指定)</param>
		/// <param name="title">タイトルバーのテキスト</param>
		/// <param name="win_mode">ウィンドウモードで起動する</param>
		/// <param name="extend_rate">画面の拡大倍率</param>
		/// <param name="wait_vsync">ＣＲＴの垂直同期信号を待つ</param>
		/// <param name="always_run">アクティブでなくても処理を続行する</param>
		/// <param name="draw_mode">描画方式。DX_DRAWMODE_~で指定。（NEAREST:くっきりだが拡大縮小が粗い(デフォルト)　BILINEAR:拡大縮小が滑らかだが、ぼけやすい）</param>
		/// <param name="double_startable">多重起動を許可する</param>
		/// <param name="out_log">DXLIBのログを出力する</param>
		/// <param name="graphics_reset_when_screen_change">フルスクリーン切り替え時、グラフィックハンドルをリセットする</param>
		/// <param name="font_cache_premul">作成するフォントデータを「乗算済みα」用にする</param>
		/// <param name="nodll_err_str">DLLが見つからなかった時のエラーテキスト</param>
		/// <param name="dxlib_err_str">DXLIBの初期化に失敗した時のエラーテキスト</param>
		public void Init(
			(int x, int y, int bit) scr,
			(byte r, byte g, byte b) back,
			(int level, int quality) sample,
			string title = "BRAND-NEW SOFTWARE",
			bool win_mode = true,
			float extend_rate = 1.0f,
			bool wait_vsync = true,
			bool always_run = true,
			int draw_mode = DX.DX_DRAWMODE_NEAREST,
			bool double_startable = false,
			bool out_log = true,
			bool graphics_reset_when_screen_change = false,
			bool font_cache_premul = true)
		{
			DX.SetMainWindowText(title);
			DX.SetGraphMode(scr.x, scr.y, scr.bit);
			DX.SetBackgroundColor(back.r, back.g, back.b);
			DX.SetOutApplicationLogValidFlag(out_log ? 1 : 0);
			DX.SetDoubleStartValidFlag(double_startable ? 1 : 0);
			DX.SetWaitVSyncFlag(wait_vsync ? 1 : 0);
			DX.ChangeWindowMode(win_mode ? 1 : 0);

			if (DX.DxLib_Init() == -1)
				throw new Exception("DxLib_Init() == -1");

			DX.SetAlwaysRunFlag(always_run ? 1 : 0);
			DX.SetWindowSizeExtendRate(extend_rate);
			DX.SetCreateDrawValidGraphMultiSample(sample.level, sample.quality);
			DX.SetChangeScreenModeGraphicsSystemResetFlag(graphics_reset_when_screen_change ? 1 : 0);
			DX.SetFontCacheUsePremulAlphaFlag(font_cache_premul ? 1 : 0);
			DX.SetDrawMode(draw_mode);
		}

		/// <summary>マネージャーの初期設定でDXLIBを初期化する。</summary>
		public void Init() => Init((640, 480, 32), (0, 0, 0), (1, 0));
	}
}