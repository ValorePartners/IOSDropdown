using UIKit;
using CoreGraphics;
using System.Linq;
using Foundation;
using System;

namespace Valore.IOSDropDown
{

	internal class DropDownListSource: UITableViewSource
	{
		private DropDownListItem[] m_ListItems;
		private const string m_CellIdentifier = "DropDownListCell";

		public event DropDownListChangedHandler DropDownListChanged;

		private UIColor m_TextColor;
		private UIColor m_CellColor;
		nfloat m_ImageSize;

		private UIColor m_ImageColor;

		public UIColor ImageColor {
			get {
				return m_ImageColor;
			}
			set {
				m_ImageColor = value;
			}
		}

		public nfloat ImageSize {
			get {
				return m_ImageSize;
			}
			set {
				m_ImageSize = value;
			}
		}

		public UIColor CellColor {
			get {
				return m_CellColor;
			}
			set {
				m_CellColor = value;
			}
		}

		public UIColor TextColor {
			get{ return m_TextColor; }
			set{ m_TextColor = value; }
		}

		private UIColor m_BackgroundColor;

		public UIColor BackgroundColor {
			get{ return m_BackgroundColor; }
			set{ m_BackgroundColor = value; }
		}

		public DropDownListSource (DropDownListItem[] listItems, UIColor textColor)
		{
			this.m_TextColor = m_TextColor;
			this.m_ListItems = listItems;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var item = m_ListItems [indexPath.Row];
			var cell = tableView.DequeueReusableCell (m_CellIdentifier) as UITableViewCell;
			if (cell == null) {
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, m_CellIdentifier);
			}
			if (m_BackgroundColor != null)
				cell.BackgroundColor = m_BackgroundColor;
			if (this.m_TextColor != null)
				cell.TextLabel.TextColor = m_TextColor;
			cell.TextLabel.Text = item.DisplayText;
			if (item.Image != null) {
				if (m_ImageColor != null) {
					var img = ChangeImageColor (item.Image, m_ImageColor);
					cell.ImageView.Image = img;
				} else {
					cell.ImageView.Image = item.Image;
				}
				var newSize = new CGSize (25f, 25f);
				if (m_ImageSize > 0.0f)
					newSize = new CGSize (m_ImageSize, m_ImageSize);
				UIGraphics.BeginImageContextWithOptions (newSize, false, UIScreen.MainScreen.Scale);
				cell.ImageView.Image.Draw (new CGRect (0, 0, newSize.Width, newSize.Height));
				cell.ImageView.Image = UIGraphics.GetImageFromCurrentImageContext ();
				cell.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			}

			if (this.m_CellColor != null)
				cell.TintColor = CellColor;
			if (item.IsSelected)
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			else
				cell.Accessory = UITableViewCellAccessory.None;

			return cell;
		}

		public override System.nint RowsInSection (UITableView tableview, System.nint section)
		{
			return this.m_ListItems.Length;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var oldItem = m_ListItems.FirstOrDefault (x => x.IsSelected);
			if (oldItem != null)
				oldItem.IsSelected = false;
			var oldCell = tableView.VisibleCells.FirstOrDefault (c => ((UITableViewCell)c).Accessory == UITableViewCellAccessory.Checkmark) as UITableViewCell;
			if (oldCell != null)
				oldCell.Accessory = UITableViewCellAccessory.None;

			m_ListItems [indexPath.Row].IsSelected = true;
			var cell = tableView.CellAt (indexPath) as UITableViewCell;
			cell.Accessory = UITableViewCellAccessory.Checkmark;
			if (DropDownListChanged != null) {
				DropDownListChanged (indexPath.Row, m_ListItems [indexPath.Row]);
			}
		}

		private UIImage ChangeImageColor (UIImage image, UIColor color)
		{
			var rect = new  CGRect (0, 0, image.Size.Width, image.Size.Height);
			UIGraphics.BeginImageContext (rect.Size);
			var ctx = UIGraphics.GetCurrentContext ();
			ctx.ClipToMask (rect, image.CGImage);
			ctx.SetFillColor (color.CGColor);
			ctx.FillRect (rect);
			var img = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return UIImage.FromImage (img.CGImage, 1.0f, UIImageOrientation.DownMirrored);
		}

	}
}
