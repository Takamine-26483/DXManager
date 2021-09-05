using System;
using DxLibDLL;

namespace TakamineProduction
{
	/// <summary>
	/// DXライブラリの起動・終了の処理を簡略化するクラス。シングルトンでインスタンスを生成しているのでInitするだけでOK
	/// </summary>
	public sealed class DXLibmanager
	{
		/// <summary>Staticで生成したインスタンス</summary>
		private static DXLibmanager instance = new DXLibmanager();


		private DXLibmanager() { }

		/// <summary>デストラクタ（ここでDXLIBの終了）</summary>
		~DXLibmanager() { DX.DxLib_End(); }

		/// <summary>☆DXLIBを初期化します。名前付き引数("引数名":"実引数")の使用を推奨</summary>
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
		public static void Init(
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
			bool font_cache_premul = true,
			string nodll_err_str = "DLLが見つかりませんでした。", string dxlib_err_str = "DXLibの初期化に失敗しました。"
			)
		{
			void MesAndExit(string mes, string cap)
			{
				//MessageBox.Show(mes, cap, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(-1);
			}

			try
			{
				DX.SetMainWindowText(title);
				DX.SetGraphMode(scr.x, scr.y, scr.bit);
				DX.SetBackgroundColor(back.r, back.g, back.b);
				DX.SetOutApplicationLogValidFlag(out_log ? 1 : 0);
				DX.SetDoubleStartValidFlag(double_startable ? 1 : 0);
				DX.SetWaitVSyncFlag(wait_vsync ? 1 : 0);
				DX.ChangeWindowMode(win_mode ? 1 : 0);

				if (DX.DxLib_Init() == -1)
					MesAndExit(dxlib_err_str, "DxLib初期化エラー");

				DX.SetAlwaysRunFlag(always_run ? 1 : 0);
				DX.SetWindowSizeExtendRate(extend_rate);
				DX.SetCreateDrawValidGraphMultiSample(sample.level, sample.quality);
				DX.SetChangeScreenModeGraphicsSystemResetFlag(graphics_reset_when_screen_change ? 1 : 0);
				DX.SetFontCacheUsePremulAlphaFlag(font_cache_premul ? 1 : 0);
				DX.SetDrawMode(draw_mode);
			}
			catch (DllNotFoundException)
			{
				MesAndExit(nodll_err_str, "DLL不見当エラー");
			}
		}
	}
}