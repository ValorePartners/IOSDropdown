iOS Dropdown Control
===================

###Overview

I was on a project and needed a dropdown list similar to what I could create in the Actionbar in Android.  iOS doesn't have such a control defined. I also didn't want to grab an existing project in Objective-C and then create a wrapper project because many of the solutions didn't have exactly what I wanted.  I needed a control that could resize the images and change the color of all the fields displayed.

###Code Example

Method to define data source. DropDownListItem is the object the defines what will be displayed in the dropdown:

        private List<DropDownListItem> GetList1()
        {
            var list = new List<DropDownListItem>();
            list.Add(new DropDownListItem()
                {
                    Id = "1",
                    DisplayText = "View Animal Selections",
                    Image = UIImage.FromBundle("footprint.png")
                });
            list.Add(new DropDownListItem()
                {
                    Id = "2",
                    DisplayText = "Bugs Are The Bomb",
                    Image = UIImage.FromBundle("bug.png"),
                    IsSelected = true
                });
            list.Add(new DropDownListItem()
                {
                    Id = "3",
                    DisplayText = "Connect With Friends",
                    Image = UIImage.FromBundle("facebook.png")
                });
            list.Add(new DropDownListItem()
                {
                    Id = "4",
                    DisplayText = "What Can Hurt You",
                    Image = UIImage.FromBundle("toxic.png")
                });
            list.Add(new DropDownListItem()
                {
                    Id = "5",
                    DisplayText = "Your Connections",
                    Image = UIImage.FromBundle("arrow.png")
                });
            list.Add(new DropDownListItem()
                {
                    Id = "6",
                    DisplayText = "Danger Will Robinson",
                    Image = UIImage.FromBundle("fire.png")
                });
            return list;
        }


Method to create and define the properties of the drop down control:

        private DropDownList GetDDL1()
        {
            var ddl = new DropDownList(GetList1().ToArray())
            {
                BackgroundColor = UIColor.FromRGB(220, 220, 220),
                TextColor = UIColor.Blue,
                Opacity = 0.85f,
                TintColor = UIColor.Blue,
                ImageColor = UIColor.Blue
            };
            return ddl;
        }

In the ViewDidLoad method call the custom methods and add the control to the view controller.  The control can be animated from any other event you desire.

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
		
            var ddl = GetDDL1();

            ddl.DropDownListChanged += (e, a) =>
            {
                var index = e; // e is the index selected
                var strValue = a.DisplayText; //a is the dropdown list item object
                var id = a.Id;
                lblMessage.Text = string.Format("Id: {0} => Text: {1}", id, strValue);
                ddl.Toggle();
            };

            this.btnOptions.Clicked += (e, a) =>
            {
                ddl.Toggle();
            };

            btnChange.TouchUpInside += (e, a) =>
            {
                ddl.UpdatedDropDownList(GetList2().ToArray());//Change to a different list
            };

            this.View.Add(ddl);

        }
