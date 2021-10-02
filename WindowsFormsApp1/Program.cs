using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using TakamineProduction;
using DxLibDLL;
using System.Drawing;

namespace WindowsFormsApp1
{
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main()
		{
			var dxManager = new DXManager
			{
				WindowMode = true,
				BackgroundColor = Color.Gray,
				AlwaysRun = true,
				MainWindowIcon = Properties.Resources.zz.Handle,
				MainWindowText = "hoge!?",
				GraphMode = (600, 600, 32),
				ChangeScreenModeGraphicsSystemReset = false,
				UsePremulAlphaConvertLoad = true,
				TransColor = Color.FromArgb(255, 0, 255),
				WindowSizeExtendRate = 1f
			};
			
			dxManager.Init(DXManager.DXDrawMode.Bilinear);
			while (dxManager.ProcMSG())
			{
				DX.ClearDrawScreen();



				DX.ScreenFlip();
			}
		}
	}
}
