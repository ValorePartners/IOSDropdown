using UIKit;

namespace Valore.IOSDropDown
{
	/// <summary>
	/// Drop down list item.
	/// </summary>
	public class DropDownListItem{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the display text.
		/// </summary>
		/// <value>The display text.</value>
		public string DisplayText {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the image displayed on the left side
		/// </summary>
		/// <value>The image.</value>
		public UIImage Image {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this instance is selected and displays a checkmark
		/// </summary>
		/// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
		public bool IsSelected {
			get;
			set;
		}
	}
	
}
