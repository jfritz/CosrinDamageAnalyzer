using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace cdagui
{
	
class MainForm : System.Windows.Forms.Form
{
	Label log_filename_label = new Label();
	LogParser parser = new LogParser();
	DataGridView data_grid = new DataGridView();
	StatusBar status_bar = new StatusBar();
	
	public MainForm()
	{
		SetupLayout();
		SetupMenus();
		SetupHandlers();
	}
	
	public void SetupMenus(){
		MainMenu mmAppStart = new MainMenu();
	
	   	MenuItem miFile = new MenuItem("&File");
		miFile.MenuItems.Add("&Open Log...", new EventHandler(Browse_Clicked));
		miFile.MenuItems.Add("&Analyze Log", new EventHandler(Analyze_Clicked));
	  	miFile.MenuItems.Add("E&xit", new EventHandler(Exit_Clicked));
		mmAppStart.MenuItems.Add(miFile);

		MenuItem miEdit = new MenuItem("&Edit");
		miEdit.MenuItems.Add("&Options...", new EventHandler(Options_Clicked));
		mmAppStart.MenuItems.Add(miEdit);

	   	MenuItem miAbout = new MenuItem("&About");
	   	miAbout.MenuItems.Add("&About Cosrin Damage Analyzer...", new EventHandler(About_Clicked));
		mmAppStart.MenuItems.Add(miAbout);
	
		this.Menu = mmAppStart;
	}

	
	private void SetupHandlers()
	{
		
	}
		
	private void SetupLayout() 
	{		
		// Set up data grid	
		SetupDataGridView();

		// Set up status bar
		SetupStatusBar();
		
		// Set up app settings
		this.Text = "Cosrin Damage Analyzer";
		this.StartPosition = FormStartPosition.CenterScreen;
		this.AutoScaleBaseSize = new Size(5,13);
		this.ClientSize = new Size(300,450);
		this.AutoScroll = true;
		this.MaximizeBox = false;
		
		// Add controls
		this.Controls.Add(data_grid);
	    this.Controls.Add(status_bar);
	}

	private void SetupDataGridView()
	{
		data_grid.ColumnCount = 2;

        data_grid.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
        data_grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        data_grid.ColumnHeadersDefaultCellStyle.Font = new Font(data_grid.Font, FontStyle.Bold);

        data_grid.Name = "Damage Analysis";
		data_grid.Location = new Point(10,10);
        data_grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
        data_grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        data_grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        data_grid.GridColor = Color.Black;
        data_grid.RowHeadersVisible = false;

        data_grid.Columns[0].Name = "Stat";
        data_grid.Columns[1].Name = "Value";
        data_grid.Columns[0].DefaultCellStyle.Font = new Font(data_grid.DefaultCellStyle.Font, FontStyle.Bold);

        data_grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        data_grid.MultiSelect = false;
		data_grid.AutoSize = true;
	}

	private void SetupStatusBar()
	{
	    StatusBarPanel panel1 = new StatusBarPanel();
	    StatusBarPanel panel2 = new StatusBarPanel();
	
	    panel1.BorderStyle = StatusBarPanelBorderStyle.Sunken;
	    panel1.AutoSize = StatusBarPanelAutoSize.Spring;
	    
	    panel2.BorderStyle = StatusBarPanelBorderStyle.Raised;
	    panel2.Text = "Ready.";
	    panel2.AutoSize = StatusBarPanelAutoSize.Contents;
	                
	    // Display panels in the StatusBar control.
	    status_bar.ShowPanels = true;
	
	    // Add both panels to the StatusBarPanelCollection of the StatusBar.            
	    status_bar.Panels.Add(panel1);
	    status_bar.Panels.Add(panel2);
	}


	/* --- HANDLERS --- */
	public void Options_Clicked(object ob, EventArgs e)
	{
	}	

	public void Browse_Clicked(object ob, EventArgs e)
	{	
		OpenFileDialog fd = new OpenFileDialog();
			
    	fd.InitialDirectory = Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
    	fd.Filter = "Log Files (*.log)|*.log|Text Files (*.txt)|*.txt|All files|*.*" ;
	    fd.FilterIndex = 2;
	    fd.RestoreDirectory = true;
	
	    if(fd.ShowDialog() == DialogResult.OK)
	    {
			// give the filename to the damage analyzer
			this.parser.log_filename = fd.FileName;

			// give status bar the filename also.
			String[] tmp = fd.FileName.Split('\\');
			String fname = tmp[tmp.Length - 1];
			status_bar.Panels[0].Text = fname;
		}
	}
	
	public void Analyze_Clicked(object ob, EventArgs e)
	{
		// Check if parser is in a good state
		if (String.IsNullOrEmpty(this.parser.log_filename))
		{
			MessageBox.Show(
				"You have to open a log first!",
				"Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error
			);

			return;
		}

		// Clear Datagrid
		data_grid.Rows.Clear();
		
		// Parse log, calculate stats, output them to data grid.
		DamageStatistics statsObject = parser.Parse(); 
		statsObject.CalculateStats();
		ArrayList statistics = statsObject.GetStats();

		foreach (string[] i in statistics)
		{
			data_grid.Rows.Add(i);
		}
	}

	public void About_Clicked(object ob, EventArgs e)
	{
		MessageBox.Show(
			"Cosrin Damage Analyzer. Copyright (c) 2010 Thau.",
			"About Cosrin Damage Analyzer",
			MessageBoxButtons.OK,
			MessageBoxIcon.Information
		);
	}
	
	public void Exit_Clicked(object ob, EventArgs e)
	{
		Application.Exit();
	}

	public void MenuItem_Clicked(object ob, EventArgs e)
	{
		Console.WriteLine("clicked");
	}
	
	[STAThreadAttribute]
	public static void Main()
	{
		Application.EnableVisualStyles();
		Application.Run(new MainForm());
	}
		
}
	
}