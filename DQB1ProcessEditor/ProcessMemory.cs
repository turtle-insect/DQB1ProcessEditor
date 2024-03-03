using System;
using System.Collections.Generic;

namespace DQB1ProcessEditor
{
	internal class ProcessMemory
	{
		private readonly Memory.Mem mMemory = new Memory.Mem();
		private UInt64 mBaseAddress;

		public void AppendItem(List<Item> items)
		{
			if (!Initialization()) return;

			int counter = 0;
			for (uint index = 0; index < 24 && counter < items.Count; index++)
			{
				var address = GetItemAddress(index);
				var current = ReadItem(address);
				if (current.ID != 0) continue;

				WriteItem(address, items[counter]);
				counter++;
			}
		}

		public void WriteItemCount(uint count)
		{
			if (!Initialization()) return;

			for (uint index = 0; index < 24; index++)
			{
				var address = GetItemAddress(index);
				var item = ReadItem(address);
				if (item.ID == 0) continue;

				item.Count = count;
				WriteItem(address, item);
			}
		}

		public void ClearItem()
		{
			if (!Initialization()) return;

			for (uint index = 0; index < 24; index++)
			{
				var address = GetItemAddress(index);
				WriteItem(address, new Item());
			}
		}

		private bool Initialization()
		{
			var pid = mMemory.GetProcIdFromName("DQB.exe");
			if (pid == 0) return false;
			if (!mMemory.OpenProcess(pid)) return false;

			Byte[] buffer = mMemory.ReadBytes("GameAssembly.dll+2B0AF30,0xB8,0,0x48,0x18,0x10,0x20", 8);
			mBaseAddress = BitConverter.ToUInt64(buffer);
			if (mBaseAddress == 0) return false;

			return true;
		}

		private UInt64 GetItemAddress(uint index)
		{
			var address = mBaseAddress + index * 0x08 + 0x20;
			var buffer = mMemory.ReadBytes(address.ToString("x"), 8);
			return BitConverter.ToUInt64(buffer) + 0x10;
		}

		private Item ReadItem(UInt64 address)
		{
			var buffer = mMemory.ReadBytes(address.ToString("x"), 4);
			var item = new Item();

			item.ID = BitConverter.ToUInt16(buffer);
			item.Count = BitConverter.ToUInt16(buffer, 2);
			return item;
		}

		private void WriteItem(UInt64 address, Item item)
		{
			var buffer = new Byte[4];
			Array.Copy(BitConverter.GetBytes(item.ID), 0, buffer, 0, 2);
			Array.Copy(BitConverter.GetBytes(item.Count), 0, buffer, 2, 2);
			mMemory.WriteBytes(address.ToString("x"), buffer);
		}
	}
}
