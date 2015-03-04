using UIKit;
using CoreGraphics;
using System;
using System.Threading.Tasks;

namespace Valore.IOSDropDown
{

	/// <summary>
	/// Drop down list changed handler.
	/// </summary>
	public delegate void DropDownListChangedHandler (int itemIndex, DropDownListItem listItem);
	/// <summary>
	/// Drop down list.
	/// </summary>
	public class DropDownList: UIView
	{
		/// <summary>
		/// Occurs when drop down list changed.
		/// </summary>
		public event DropDownListChangedHandler DropDownListChanged;
		private UIView container;
		private UITableView table;
		private DropDownListItem[] m_ListItems;
		private DropDownListSource m_Source;
		private bool isOpen;
		private UIColor m_TextColor;
		private UIColor m_BackgroundColor;
		private UIColor m_TintColor;
		private float m_ImageSize;
		private UIColor m_ImageColor;

		/// <summary>
		/// Gets or sets the opacity.
		/// </summary>
		/// <value>The opacity.</value>
		public float Opacity {
			get{ return this.Layer.Opacity; }
			set{ this.Layer.Opacity = value; }
		}

		/// <summary>
		/// Gets or sets the width of the border.
		/// </summary>
		/// <value>The width of the border.</value>
		public nfloat BorderWidth {
			get{ return this.Layer.BorderWidth; }
			set{ this.Layer.BorderWidth = value; }
		}

		/// <summary>
		/// Gets or sets the color of the border.
		/// </summary>
		/// <value>The color of the border.</value>
		public CGColor BorderColor {
			get{ return this.Layer.BorderColor; }
			set{ this.Layer.BorderColor = value; }
		}

		/// <summary>
		/// Gets or sets the dropdown offset based on the height of the navigation bar.
		/// </summary>
		/// <value>The height of the navigation bar assumed.</value>
		public float NavigationBarAssumedHeight {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the color of the image.
		/// </summary>
		/// <value>The color of the image.</value>
		public UIColor ImageColor {
			get {
				return m_ImageColor;
			}
			set {
				if (value != null) {
					m_ImageColor = value;
					if (m_Source == null) {
						InitTableAndSource ();
					}
					this.m_Source.ImageColor = m_ImageColor;
				}
			}
		}

		/// <summary>
		/// Gets or sets the size of the image.
		/// </summary>
		/// <value>The size of the image.</value>
		public float ImageSize {
			get {
				return m_ImageSize;
			}
			set {
				if (value != null) {
					m_ImageSize = value;
					if (m_Source == null) {
						InitTableAndSource ();
					}
					this.m_Source.ImageSize = m_ImageSize;
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the tint.
		/// </summary>
		/// <value>The color of the tint.</value>
		public UIColor TintColor {
			get{ return m_TintColor != null ? m_TintColor : UIColor.Gray; }
			set { 
				if (value != null) {
					m_TintColor = value;
					if (m_Source == null) {
						InitTableAndSource ();
					}
					this.table.TintColor = m_TintColor;
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the background.
		/// </summary>
		/// <value>The color of the background.</value>
		public UIColor BackgroundColor {
			get{ return m_BackgroundColor != null ? m_BackgroundColor : UIColor.White; }
			set { 
				if (value != null) {
					m_BackgroundColor = value;
					if (m_Source == null) {
						InitTableAndSource ();
					}
					m_Source.BackgroundColor = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the text.
		/// </summary>
		/// <value>The color of the text.</value>
		public UIColor TextColor {
			get{ return m_TextColor != null ? m_TextColor : UIColor.Black; }
			set { 
				if (value != null) {
					m_TextColor = value;
					if (m_Source == null) {
						InitTableAndSource ();
					}
					m_Source.TextColor = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the drop down top inset.  Default is 60
		/// </summary>
		/// <value>The drop down top inset.</value>
		public nfloat DropDownTopInset{ get; set; }

		/// <summary>
		/// Reloads the table view.
		/// </summary>
		public void ReloadTableView ()
		{
			this.table.ReloadData ();
		}

		/// <summary>
		/// Updates the drop down list.
		/// </summary>
		/// <param name="listItems">List items.</param>
		public async void UpdatedDropDownList (DropDownListItem[] listItems)
		{
			if (isOpen) {
				ScrollTheView (false);
				isOpen = false;
				await Task.Delay (300);
				ResetTableView (listItems);
			} else {
				ResetTableView (listItems);
			}

		}

		/// <summary>
		/// Toggles the dropdown open or closed.
		/// </summary>
		public void Toggle ()
		{
			if (UIScreen.MainScreen.Bounds.Width != this.Layer.Frame.Width) {
				InitUXSettings ();
				table.Frame = new CGRect (0, 0, this.Frame.Width, this.Frame.Height);
			}
			InitTableAndSource ();
			if (isOpen) {
				ScrollTheView (false);
				isOpen = false;
			} else {
				this.container.Add (this);
				ScrollTheView (true);
				isOpen = true;
			}
		}

		public void ToggleClosed(){
			if(isOpen){
				Toggle();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DropDownList"/> class.
		/// </summary>
		/// <param name="listItems">List items.</param>
		public DropDownList (UIView view, DropDownListItem[] listItems)
		{
			this.container = view;
			this.m_ListItems = listItems;
			InitUXSettings ();
		}


		private void InitUXSettings(){
			this.Layer.Frame = GetFrameDimensions ();
			this.Layer.BorderWidth = 1;
			this.Layer.BorderColor = UIColor.LightGray.CGColor;
			this.Layer.Opacity = 0.95f;
		}

		private void ResetTableView (DropDownListItem[] listItems)
		{
			m_Source.DropDownListChanged -= DropDownChangedMethod;
			table.RemoveFromSuperview ();
			m_Source = null;
			table = null;
			this.m_ListItems = listItems;
			this.Layer.Frame = GetFrameDimensions ();
			InitTableAndSource ();

			if (m_BackgroundColor != null)
				m_Source.BackgroundColor = m_BackgroundColor;
			if (m_TextColor != null)
				m_Source.TextColor = m_TextColor;
			if (m_TintColor != null)
				table.TintColor = m_TintColor;
			if (m_ImageSize != null)
				m_Source.ImageSize = m_ImageSize;
			if (m_ImageColor != null)
				m_Source.ImageColor = m_ImageColor;
		}

		private void InitTableAndSource ()
		{
			if (m_Source == null) {
				m_Source = new DropDownListSource (m_ListItems, this.m_TextColor);
				m_Source.DropDownListChanged += DropDownChangedMethod;
				table = new UITableView (new CGRect (0, 0, this.Frame.Width, this.Frame.Height));
				if (DropDownTopInset == 0)
					DropDownTopInset = 60;
				table.ContentInset = new UIEdgeInsets (0, 0, 0, 0);
				table.Source = m_Source;
				this.Add (table);
			}
		}

		private void DropDownChangedMethod (int index, DropDownListItem item)
		{
			if (DropDownListChanged != null) {
				DropDownListChanged (index, item);
			}
		}


		private CGRect GetFrameDimensions ()
		{
			var height = m_ListItems.Length * 44f;
			return new CGRect (0, (height * -2), UIScreen.MainScreen.Bounds.Width, height);
		}

		private async void ScrollTheView (bool up)
		{
			this.Hidden = false;
			await UIView.AnimateAsync (0.3, () => {
				var frame = this.Layer.Frame;
				if (up) {
					frame.Y = NavigationBarAssumedHeight == 0f ? 60 : NavigationBarAssumedHeight;
				} else {
					frame.Y = (this.Layer.Frame.Height * -2);
				}
				this.Layer.Frame = frame;
			});
			if (!up) {
				await UIView.AnimateAsync (0.3, () => {
					this.Hidden = true;
					this.RemoveFromSuperview ();
				});
			}
		}
	}

}
