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
			DXLibmanager.Init((800,600,32),(255,0,0),(1,0));

			while (DX.ProcessMessage() == 0)
			{
			
			}
		}
	}
}
