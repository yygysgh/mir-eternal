﻿using System;

namespace GameServer.Networking
{
	
	[PacketInfoAttribute(来源 = PacketSource.Client, 编号 = 528, 长度 = 6, 注释 = "删除仇人")]
	public sealed class 玩家删除仇人 : GamePacket
	{
		
		public 玩家删除仇人()
		{
			
			
		}

		
		[WrappingFieldAttribute(SubScript = 2, Length = 4)]
		public int 对象编号;
	}
}