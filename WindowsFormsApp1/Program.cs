using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using TakamineProduction;
using DxLibDLL;



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
			var dxm = new DXLibManager();
   			dxm.Init(draw_mode:DX.DX_DRAWMODE_BILINEAR);

			while (DX.ProcessMessage() == 0)
			{
				if (dxm.IsInitialized)
					DX.DrawString(10, 10, "しょきか", DX.GetColor(255, 255, 255));
				if (DX.CheckHitKeyAll() != 0)
					dxm.End();
			}
		}
	}
}
