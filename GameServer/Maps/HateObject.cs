using System;
using System.Collections.Generic;

namespace GameServer.Maps
{
	
	public sealed class HateObject
	{
		
		public HateObject()
		{
			
			
			this.HateDic = new Dictionary<MapObject, HateObject.HateDetail>();
		}

		
		public bool RemoveHateObject(MapObject obj)
		{
			if (this.CurrentTarget == obj)
			{
				this.CurrentTarget = null;
			}
			return this.HateDic.Remove(obj);
		}

		
		public void AddHateObject(MapObject obj, DateTime time, int hateValue)
		{
			if (obj.Died)
			{
				return;
			}
			HateObject.HateDetail hateDetail;
			if (this.HateDic.TryGetValue(obj, out hateDetail))
			{
				hateDetail.HateTime = ((hateDetail.HateTime < time) ? time : hateDetail.HateTime);
				hateDetail.HateValue += hateValue;
				return;
			}
			this.HateDic[obj] = new HateObject.HateDetail(time, hateValue);
		}

		
		public bool 切换仇恨(MapObject host)
		{
			int num = int.MinValue;
			List<MapObject> list = new List<MapObject>();
			foreach (KeyValuePair<MapObject, HateObject.HateDetail> keyValuePair in this.HateDic)
			{
				if (keyValuePair.Value.HateValue > num)
				{
					num = keyValuePair.Value.HateValue;
					list = new List<MapObject>
					{
						keyValuePair.Key
					};
				}
				else if (keyValuePair.Value.HateValue == num)
				{
					list.Add(keyValuePair.Key);
				}
			}
			if (num == 0 && this.CurrentTarget != null)
			{
				return true;
			}
			int num2 = int.MaxValue;
			MapObject MapObject = null;
			foreach (MapObject MapObject2 in list)
			{
				int num3 = host.GetDistance(MapObject2);
				if (num3 < num2)
				{
					num2 = num3;
					MapObject = MapObject2;
				}
			}
			PlayerObject PlayerObject = MapObject as PlayerObject;
			if (PlayerObject != null)
			{
				PlayerObject.玩家获得仇恨(host);
			}
			return (this.CurrentTarget = MapObject) != null;
		}

		
		public bool 最近仇恨(MapObject 主人)
		{
			int num = int.MaxValue;
			List<KeyValuePair<MapObject, HateObject.HateDetail>> list = new List<KeyValuePair<MapObject, HateObject.HateDetail>>();
			foreach (KeyValuePair<MapObject, HateObject.HateDetail> item in this.HateDic)
			{
				int num2 = 主人.GetDistance(item.Key);
				if (num2 < num)
				{
					num = num2;
					list = new List<KeyValuePair<MapObject, HateObject.HateDetail>>
					{
						item
					};
				}
				else if (num2 == num)
				{
					list.Add(item);
				}
			}
			int num3 = int.MinValue;
			MapObject MapObject = null;
			foreach (KeyValuePair<MapObject, HateObject.HateDetail> keyValuePair in list)
			{
				if (keyValuePair.Value.HateValue > num3)
				{
					num3 = keyValuePair.Value.HateValue;
					MapObject = keyValuePair.Key;
				}
			}
			PlayerObject PlayerObject = MapObject as PlayerObject;
			if (PlayerObject != null)
			{
				PlayerObject.玩家获得仇恨(主人);
			}
			return (this.CurrentTarget = MapObject) != null;
		}

		
		public MapObject CurrentTarget;//当前目标


        public DateTime SwitchTime;//切换时间


        public readonly Dictionary<MapObject, HateObject.HateDetail> HateDic;

		
		public sealed class HateDetail //仇恨详情
        {
			
			public HateDetail(DateTime hateTime, int hateValue)
			{
				this.HateTime = hateTime;
				this.HateValue = hateValue;
			}

		
			public int HateValue;

			public DateTime HateTime;
		}
	}
}
