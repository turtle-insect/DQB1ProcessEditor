using System;
using System.Collections.Generic;

namespace DQB1ProcessEditor
{
	internal class Info
	{
		public List<ItemInfo> Items { get; private set; } = new List<ItemInfo>();
		public List<ItemTemplate> ItemTemplates { get; private set; } = new List<ItemTemplate>();

		public static Info Instance
		{
			get => sInstance;
		}
		private static Info sInstance = new Info();

		private Info()
		{
			LoadItem();
			LoadItemTemplate();
		}

		private void LoadItem()
		{
			String filename = @"info\item.txt";
			if (!System.IO.File.Exists(filename)) return;

			Items.Clear();
			foreach (var line in System.IO.File.ReadAllLines(filename))
			{
				var elements = SplitLine(line);
				if (elements.Length < 2) continue;

				var item = new ItemInfo();
				item.ID = uint.Parse(elements[0]);
				item.Name = elements[1];
				if (item.ID == 0) continue;

				Items.Add(item);
			}
		}

		public void LoadItemTemplate()
		{
			String path = @"info\template";
			if (!System.IO.Directory.Exists(path)) return;

			ItemTemplates.Clear();
			foreach (var filename in System.IO.Directory.GetFiles(path))
			{
				var template = new ItemTemplate();
				template.Name = System.IO.Path.GetFileName(filename);

				foreach (var line in System.IO.File.ReadAllLines(filename))
				{
					var items = SplitLine(line);
					if (items.Length < 2) continue;

					var item = new Item();
					item.ID = Convert.ToUInt16(items[0]);
					item.Count = Convert.ToUInt16(items[1]);
					if (item.Count > 9999) item.Count = 9999;
					template.Items.Add(item);
				}

				if (template.Items.Count > 0)
				{
					ItemTemplates.Add(template);
				}
			}
		}

		private String[] SplitLine(String line)
		{
			line = line.Replace("\n", "");
			line = line.Replace("\r", "");
			if (line.Length < 3) return new String[0];
			if (line[0] == '#') return new String[0];

			return line.Split('\t');
		}
	}
}
