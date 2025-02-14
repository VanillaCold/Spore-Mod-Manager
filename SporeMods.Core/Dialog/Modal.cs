﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
//using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SporeMods.Core
{
	public static partial class Modal
	{
		static List<ModalShownEventArgs> _modals = new List<ModalShownEventArgs>();
		
		public static async Task<T> Show<T>(IModalViewModel<T> vm)
		{
			Cmd.WriteLine($"Modal \'{vm.ToString()}\' added to queue.");
			var task = vm.CompletionSource.Task;
			AddToQueue(new ModalShownEventArgs(vm, task));
			return await task;
		}




		static void AddToQueue(ModalShownEventArgs args)
		{
			_modals.Add(args);
			StartRollingModals();
		}

		static bool _rolling = false;

		static ModalShownEventArgs _current = null;

		static async void StartRollingModals()
		{
			if (!_rolling)
			{
				_rolling = true;
				while (_modals.Count > 0)
				{
					if (_awaitProceed != null)
					{
						await _awaitProceed.Task;
						_awaitProceed = null;
					}
					
					_current = _modals[0];
					
					
					_modalShown?.Invoke(null, _current);
					//string modalStr = $"\'{_current.ViewModel.ToString()}\'";
					//Cmd.WriteLine($"Showing modal {modalStr}...");
					await _current.Task;
					//Cmd.WriteLine($"...done with {modalStr}.");
					_modals.Remove(_current);
					_current = null;
					_modalShown?.Invoke(null, null);
				}
				_rolling = false;
			}
		}

		public static bool IsCurrent(IModalViewModel vm)
			=> (_current != null) ? (_current.ViewModel == vm) : false;
		
		
		static int _totalHandlers = 0;
		internal static int TotalHandlers
		{
			get => _totalHandlers;
			set => _totalHandlers = Math.Max(0, value);
		}

		static TaskCompletionSource<object> _awaitProceed = null;
		public static void PreventProceed()
		{
			_awaitProceed = new TaskCompletionSource<object>();
		}

		public static void PermitProceed()
		{
			if (_awaitProceed != null)
				_awaitProceed.TrySetResult(null);
			
			_awaitProceed = null;
		}

		
		static event EventHandler<ModalShownEventArgs> _modalShown;
		public static event EventHandler<ModalShownEventArgs> Shown
		{
			add
			{
				_modalShown += value;
				if (_rolling && (_modals.Count > 0))
					_modalShown?.Invoke(null, _current);
				
				TotalHandlers++;
			}
			remove
			{
				_modalShown -= value;
				if (TotalHandlers > 0)
					TotalHandlers--;
			}
		}
	}

	public class ModalShownEventArgs
	{
		Task _task = null;
		public Task Task
			=> _task;

		IModalViewModel _vm = null;
		public IModalViewModel ViewModel
			=> _vm;

		public ModalShownEventArgs(IModalViewModel vm, Task task)
		{
			_vm = vm;
			_task = task;
		}
		
		private ModalShownEventArgs()
		{ }
	}

	
}
