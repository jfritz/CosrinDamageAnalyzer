
using System;
using System.Collections;

namespace cdagui
{
	public class LogParser
	{
		private String __log_filename;
		private DamageStatistics __stats;

		public LogParser ()
		{
		}
		
		public DamageStatistics stats
		{
			get { return __stats; }
			set { __stats = value; }
		}
		
		public String log_filename
        {
			get { return __log_filename; }
			set { __log_filename = value; }
		}
		
		// This method just iterates through the log and adds all the hits/absorbs information
		// Into the DamageStatistics object. Then returns that object
		public DamageStatistics Parse()
		{
			String data;
			this.stats = new DamageStatistics();
			int currentDamage = 0;
			int currentAbsorption = 0;
			int currentEff = 0;
			
			try {
			
				System.IO.StreamReader infile = new System.IO.StreamReader(this.log_filename);
				
				while (!infile.EndOfStream)
				{
					data = infile.ReadLine();
					if (data.StartsWith("You hit for"))
					{	
						// --- Find the damage for this hit
						String[] lineChunks = data.Split(' ');
						currentDamage = Int16.Parse(lineChunks[3]);

						// --- Find the absorb for this hit
						currentAbsorption = Int16.Parse(lineChunks[10].TrimEnd('.')); 
		
						// --- Find the effective damage for this hit
						currentEff = currentDamage - currentAbsorption;
						
						// --- Add the data into the stats object.
						this.stats.hits.Add(
							currentDamage.ToString() + ',' + 
							currentAbsorption.ToString() + ',' +
							currentEff.ToString()
						);

						Console.WriteLine("Added hit: " + 
							currentDamage.ToString() + ',' + 
							currentAbsorption.ToString() + ',' +
							currentEff.ToString()
						);
		
					} //endif
				}
				infile.Close();
			} catch (Exception ex) {
				Console.WriteLine("There was an error reading the log file" + ex.ToString());	
			}
			
			return this.stats;
			
		}

	}
}
