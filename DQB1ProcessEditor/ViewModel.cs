using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DQB1ProcessEditor
{
	internal class ViewModel
	{
		public Info Info { get; init; } = Info.Instance;
		public uint Count { get; set; } = 500;
		public uint ImportTemplateItemIndex { get; set; } = 0;
		public ObservableCollection<ItemInfo> Items { get; init; } = new();

		public ICommand ImportTemplateItemCommand { get; init; }
		public ICommand WriteItemCountCommand { get; init; }
		public ICommand ClearItemCommand { get; init; }
		public String Filter
		{
			get => mFilter;
			set
			{
				mFilter = value;
				CreateItemList();
			}
		}
		private String mFilter = "";

		public ViewModel()
		{
			ImportTemplateItemCommand = new CommandAction(ImportTemplateItem);
			WriteItemCountCommand = new CommandAction(WriteItemCount);
			ClearItemCommand = new CommandAction(ClearItem);

			CreateItemList();
		}

		public void AppendItem(ItemInfo info)
		{
			var item = new Item();
			item.ID = info.ID;
			item.Count = Count;
			var items = new List<Item>();
			items.Add(item);

			var pm = new ProcessMemory();
			pm.AppendItem(items);
		}

		public void KeyboardAction(int keyCode)
		{
			switch (keyCode)
			{
				// F1
				case 112:
					WriteItemCount(null);
					break;

				// F3
				case 114:
					ClearItem(null);
					break;

				// F10
				case 121:
					if (Info.Instance.ItemTemplates.Count > 0)
					{
						ImportTemplateItem(null);
					}
					break;
			}
		}

		private void CreateItemList()
		{
			Items.Clear();
			foreach (var item in Info.Items)
			{
				if(string.IsNullOrEmpty(mFilter) || item.Name.IndexOf(mFilter) >= 0)
				{
					Items.Add(item);
				}
			}
		}

		private void WriteItemCount(object? parameter)
		{
			var pm = new ProcessMemory();
			pm.WriteItemCount(Count);
		}

		private void ImportTemplateItem(object? parameter)
		{
			if (ImportTemplateItemIndex >= Info.ItemTemplates.Count) return;

			var pm = new ProcessMemory();
			pm.AppendItem(Info.ItemTemplates[(int)ImportTemplateItemIndex].Items);
		}

		private void ClearItem(object? parameter)
		{
			var pm = new ProcessMemory();
			pm.ClearItem();
		}
	}
}
