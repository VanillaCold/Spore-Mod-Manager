﻿using SporeMods.Core.Mods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SporeMods.Core
{
	//[DebuggerDisplay("Path = {Path} Query = {Query}")]
	public class ModSearch : INotifyPropertyChanged
	{
		ThreadSafeObservableCollection<ISporeMod> _searchResults = new ThreadSafeObservableCollection<ISporeMod>();
		public ThreadSafeObservableCollection<ISporeMod> SearchResults
		{
			get => _searchResults;
			set
			{
				_searchResults = value;
				NotifyPropertyChanged(nameof(SearchResults));
			}
		}

		
		static ModSearch _instance = null;
		public static ModSearch Instance
		{
			get => _instance;
		}

		public static void Ensure()
		{
			if (_instance == null)
				_instance = new ModSearch();
		}
		private ModSearch()
		{
			ModsManager.InstalledMods.CollectionChanged += (sender, args) =>
			{
				if ((args.OldItems != null) && (args.OldItems.Count > 0))
				{
					foreach (ISporeMod mod in args.OldItems)
					{
						if (SearchResults.Contains(mod))
							SearchResults.Remove(mod);
					}
				}
			};
		}


		public static void CancelSearch()
		{
			if (_searching)
				_cancel = true;
			
			Instance.SearchResults.Clear();
		}

		static bool _searching = false;
		static bool _cancel = false;

		private static void StartSearch(string query, bool searchNames, bool searchDescriptions, bool searchTags)
		{

			while (_searching == true)
            {
				Thread.Sleep(17);
            }

			_searching = true;


			Instance.SearchResults.Clear();
			var mods = new ObservableCollection<ISporeMod>();
			
			ModsManager.RunOnMainSyncContext(state => mods = ModsManager.InstalledMods);
			for (int i = 0; i < mods.Count; i++)
			{
				//MessageDisplay.DebugShowMessageBox($"{mods[i].DisplayName.ToString()}");
				if (_cancel)
				{
					_cancel = false;
					break;
				}
				else
				{


					ISporeMod mod = mods[i];

					bool nameMatches = (searchNames && mod.DisplayName.ToString().Contains(query, StringComparison.OrdinalIgnoreCase));

					bool descMatches = (searchDescriptions && mod.HasInlineDescription && mod.InlineDescription.ToString().Contains(query, StringComparison.OrdinalIgnoreCase));


					Cmd.WriteLine($"Searching in... {searchNames}, {searchDescriptions}...found {nameMatches}, {descMatches}");
					if (nameMatches ||
						descMatches
						/* ||
						(searchTags && false/*temp* /)*/
					)
					{
						if (!Instance.SearchResults.Contains(mod))
							Instance.SearchResults.Add(mod);
					}


				}
			}
			_searching = false;
		}

		public static async void StartSearchAsync(string query, bool searchNames, bool searchDescriptions, bool searchTags)
		{
			var task = new Task(() => StartSearch(query, searchNames, searchDescriptions, searchTags));
			task.Start();
			await task;
		}

		private void NotifyPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
