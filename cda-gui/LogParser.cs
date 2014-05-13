
using System;
using System.Collections;

namespace cdagui
{
	public class LogParser
	{
		private String __log_filename;
		private DamageStatistics __stats;
		
		// Melee specific detection data
		private const uint DAMAGE_POSITION = 3;
		private const uint ABSORB_POSITION = 9;

		// Spell specific detection data
		private ArrayList __spell_strings;

		public LogParser ()
		{
			// TODO initialize spell-strings
		}
		
		private ArrayList spell_strings {
			get{ return __spell_strings; }
			set{ __spell_strings = value; }
		}

		public DamageStatistics stats {
			get { return __stats; }
			set { __stats = value; }
		}
		
		public String log_filename {
			get { return __log_filename; }
			set { __log_filename = value; }
		}
		
		// This method just iterates through the log and adds all the hits/absorbs information
		// Into the DamageStatistics object. Then returns that object
		public DamageStatistics Parse()
		{
			String logfileLine;
			this.stats = new DamageStatistics();

			try {
			
				System.IO.StreamReader infile = new System.IO.StreamReader(this.log_filename);
				
				while (!infile.EndOfStream)
				{
					logfileLine = infile.ReadLine();
					DetectMeleeHit(logfileLine); 
					// DetectSpellHit(data);
				}
				infile.Close();
			} catch (Exception ex) {
				Console.WriteLine("There was an error reading the log file: " + ex.ToString());	
			}
			
			return this.stats;
			
		}

		private void DetectMeleeHit (string logfileLine)
		{
			Int16 raw_damage = 0;
			Int16 absorbed_damage = 0;
			
			if (logfileLine.StartsWith ("You hit for")) {

				// --- Find the damage for this hit
				String[] lineChunks = logfileLine.Split(
					new String[] {" "},
					StringSplitOptions.RemoveEmptyEntries
				);
				
				raw_damage = Int16.Parse (lineChunks[DAMAGE_POSITION]);

				// --- Find the absorb for this hit
				absorbed_damage = Int16.Parse (lineChunks[ABSORB_POSITION].TrimEnd ('.'));

				// --- Add the data into the stats object.
				this.stats.AddHit (raw_damage, absorbed_damage);

				Console.WriteLine ("Added hit: " + raw_damage.ToString () + ',' + absorbed_damage.ToString ());
			}
		}

		// TODO. Need character name to solve the problem of finding hits specific to this character's damage.
		private void DetectSpellHit (string logfileLine)
		{
			// TODO
		}
	}
}
