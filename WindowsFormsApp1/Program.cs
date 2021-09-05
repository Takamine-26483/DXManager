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
			var dxm = new DXLibmanager();
			dxm.Init();

			while (DX.ProcessMessage() == 0)
			{
				if (dxm.IsInitialized)
					DX.DrawString(10, 10, "しょきか", DX.GetColor(255, 255, 255));
			}
		}
	}
}
