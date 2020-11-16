using System;
using System.Collections.Generic;
using System.Text;

namespace SubRegionMatrix
{
	public class Cell
	{
		public Cell()
		{
			this.x = 0;
			this.y = 0;
			this.value = 0;
			this.region = 0;
			this.check = false;
		}
		public int x;
		public int y;
		public int value;
		public int region;
		public bool check;
	}
}
