using System;
using System.Drawing;
using System.Windows.Forms;

namespace cdagui
{
	
	// See http://www.c-sharpcorner.com/UploadFile/DipalChoksi/PassingDataBetweenaWindowsFormandDialogBoxes11242005035025AM/PassingDataBetweenaWindowsFormandDialogBoxes.aspx
	public class OptionsForm : System.Windows.Forms.Form
	{
		System.Windows.Forms.Form MyParentForm;
		
		public OptionsForm(System.Windows.Forms.Form ParentForm)
		{
			MyParentForm = ParentForm;
		 	SetupLayout();
		}
		

		private void SetupLayout() 
		{		
			// Set up app settings
			this.Text = "Options";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.AutoScaleBaseSize = new Size(5,13);
			this.ClientSize = new Size(250,100);
			this.AutoScroll = true;
			this.MaximizeBox = false;
			this.ShowInTaskbar = false;
			
			// Add control
			//this.Controls.Add(status_bar);
		}

	}
}
