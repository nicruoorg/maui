﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Platform;
using UIKit;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class EntryTests
	{
		UITextField GetPlatformControl(EntryHandler handler) =>
			(UITextField)handler.PlatformView;

		Task<string> GetPlatformText(EntryHandler handler)
		{
			return InvokeOnMainThreadAsync(() => GetPlatformControl(handler).Text);
		}

		void SetPlatformText(EntryHandler entryHandler, string text) =>
			GetPlatformControl(entryHandler).Text = text;

		int GetPlatformCursorPosition(EntryHandler entryHandler)
		{
			var textField = GetPlatformControl(entryHandler);

			if (textField != null && textField.SelectedTextRange != null)
				return (int)textField.GetOffsetFromPosition(textField.BeginningOfDocument, textField.SelectedTextRange.Start);

			return -1;
		}

		int GetPlatformSelectionLength(EntryHandler entryHandler)
		{
			var textField = GetPlatformControl(entryHandler);

			if (textField != null && textField.SelectedTextRange != null)
				return (int)textField.GetOffsetFromPosition(textField.SelectedTextRange.Start, textField.SelectedTextRange.End);

			return -1;
		}

		void SetupNextBuilder()
		{
			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler<ListView, ListViewRenderer>();
					handlers.AddHandler<TableView, TableViewRenderer>();
					handlers.AddHandler<VerticalStackLayout, LayoutHandler>();
					handlers.AddHandler<Entry, EntryHandler>();
					handlers.AddHandler<EntryCell, EntryCellRenderer>();
				});
			});
		}

		[Fact]
		public async Task NextListView()
		{
			SetupNextBuilder();

			var entry1 = new Entry
			{
				Text = "Entry 1",
				ReturnType = ReturnType.Next
			};

			var listView = new ListView()
			{
				ItemTemplate = new DataTemplate(() =>
				{
					var cell = new EntryCell();
					cell.SetBinding(EntryCell.TextProperty, ".");
					return cell;
				}),
				ItemsSource = Enumerable.Range(0, 10).Select(i => $"EntryCell {i}").ToList()
			};

			var layout = new VerticalStackLayout()
			{
				entry1,
				listView,
			};

			layout.HeightRequest = 1000;
			layout.WidthRequest = 300;

			await InvokeOnMainThreadAsync(async () =>
			{
				var contentViewHandler = CreateHandler<LayoutHandler>(layout);
				await contentViewHandler.PlatformView.AttachAndRun(() =>
				{
					KeyboardAutoManager.GoToNextResponderOrResign(entry1.ToPlatform(), customSuperView: entry1.ToPlatform().Superview);
					var firstResponder = layout.ToPlatform().FindFirstResponder();
					var field = firstResponder as UITextField;
					Assert.NotNull(field);
					Assert.True(field.Text == "EntryCell 0");
				});
			});
		}

		[Fact]
		public async Task NextTableView()
		{
			SetupNextBuilder();

			var entry1 = new Entry
			{
				Text = "Entry 1",
				ReturnType = ReturnType.Next
			};

			var tableView = new TableView()
			{
				Root = new TableRoot("Table Title") {
					new TableSection ("Section 1 Title") {
						new EntryCell {
							Text = "EntryCell1",
						},
						new EntryCell {
							Text = "EntryCell2",
						},
						new EntryCell {
							Text = "EntryCell3",
						}
					},
				}
			};

			var layout = new VerticalStackLayout()
			{
				entry1,
				tableView,
			};

			layout.HeightRequest = 1000;
			layout.WidthRequest = 300;

			await InvokeOnMainThreadAsync(async () =>
			{
				var contentViewHandler = CreateHandler<LayoutHandler>(layout);
				await contentViewHandler.PlatformView.AttachAndRun(() =>
				{
					KeyboardAutoManager.GoToNextResponderOrResign(entry1.ToPlatform(), customSuperView: entry1.ToPlatform().Superview);
					var firstResponder = layout.ToPlatform().FindFirstResponder();
					var field = firstResponder as UITextField;
					Assert.NotNull(field);
					Assert.True(field.Text == "EntryCell1");
				});
			});
		}
	}
}
