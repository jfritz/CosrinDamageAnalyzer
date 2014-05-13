
using System;
using System.Collections;

namespace cdagui
{
	public class DamageStatistics
	{
		private double totalDamage;
		private double totalAbsorbed;
		private double totalEffective;
		private int maxDamage;
		private int minDamage;
		private int maxAbsorb;
		private int minAbsorb;
		private int maxEffective;
		private int minEffective;
		private double averageDamage;
		private double averageAbsorbed;
		private double averageEffectiveDamage;
		
		// An array list of strings "X,Y,Z" where X is the damage for a hit; 
		// Y is the absorb for a hit. Z is the effective damage (X - Y)
		public ArrayList hits; 

		private ArrayList returnStats;

		public DamageStatistics()
		{
			hits = new ArrayList();
			ClearStats();
		}

		// This just clears out this object.
		public void ClearStats()
		{
			hits.Clear();
			totalDamage = 0;
			totalAbsorbed = 0;
			totalEffective = 0;
			maxDamage = 0;
			minDamage = 999;
			maxAbsorb = 0;
			minAbsorb = 999;
			maxEffective = 0;
			minEffective = 999;
			averageDamage = 0;
			averageAbsorbed = 0;
			averageEffectiveDamage = 0;
		}

		// Returns true if this stats object hasn't had meaningful data added to it.
		public bool IsEmpty ()
		{
			return (hits.Count == 0);
		}

		// Add a piece of data (hit) to this stats object.
		public void AddHit (Int16 raw_damage, Int16 absorbed_damage)
		{
			Int16 effective_damage = (Int16)(raw_damage - absorbed_damage);

			// TODO clean the format of the hits ArrayList up for cleaner calculations?
			this.hits.Add(
				raw_damage.ToString() + ',' + 
				absorbed_damage.ToString() + ',' +
				effective_damage.ToString()
			);
		}

		/**
		 * This function calculates and populates statistical variables based on
		 * the hits and absorbs arrayLists.
		*/
		public void CalculateStats()
		{
			Int16 currentDamage = 0;
			Int16 currentAbsorb = 0;
			Int16 currentEffective = 0;

			//RemoveEffectiveHitsBelowThreshold(49);

			foreach (String hit in hits)
			{
				String[] tmpHit = hit.Split(',');
				currentDamage = Int16.Parse(tmpHit[0]);
				currentAbsorb = Int16.Parse(tmpHit[1]);
				currentEffective = Int16.Parse(tmpHit[2]);

				CheckForBoundaryStats(currentDamage,currentAbsorb,currentEffective);
				
				totalDamage += currentDamage;
				totalAbsorbed += currentAbsorb;
				totalEffective += currentEffective;
			}

			averageDamage = totalDamage / hits.Count;
			averageAbsorbed = totalAbsorbed / hits.Count;
			averageEffectiveDamage = totalEffective / hits.Count;
		}

		private void RemoveEffectiveHitsBelowThreshold(Int16 threshold)
		{
			Int16 currentEffective = 0;

			for (Int16 x = 0; x < hits.Count; x++)
			{
				String hit = (String)hits[x];

				String[] tmpHit = hit.Split(',');
				currentEffective = Int16.Parse(tmpHit[2]);				

				if (currentEffective < threshold) 
				{
					hits.Remove(hit);
					Console.WriteLine("Removed below-threshold hit; " + currentEffective);
				}
			}
		}

		// This function checks and updates the "boundary statistics" -- min/max hit, absorb, and effective dmg.
		private void CheckForBoundaryStats(Int16 currentDamage, Int16 currentAbsorb, Int16 currentEffective)
		{	
			this.maxDamage = (currentDamage > this.maxDamage) ? currentDamage : this.maxDamage;
			this.minDamage = (currentDamage < this.minDamage) ? currentDamage : this.minDamage;
			
			this.maxAbsorb = (currentAbsorb > this.maxAbsorb) ? currentAbsorb : this.maxAbsorb;
			this.minAbsorb = (currentAbsorb < this.minAbsorb) ? currentAbsorb : this.minAbsorb;		

			this.maxEffective = (currentEffective > this.maxEffective) ? currentEffective : this.maxEffective;
			this.minEffective = (currentEffective < this.minEffective) ? currentEffective : this.minEffective;
		}
		
		/**
		 * This function formats the statistical data for display in a DataGridView.
		*/
		public ArrayList GetDataGridStats()
		{
			returnStats = new ArrayList();

			returnStats.Add(new String[]{
				"Total Damage",
				totalDamage.ToString()
			});
			returnStats.Add(new String[]{
				"Total Absorbed",
				totalAbsorbed.ToString()
			});
			returnStats.Add(new String[]{
				"Maximum Hit",
				maxDamage.ToString()
			});
			returnStats.Add(new String[]{
				"Minimum Hit",
				minDamage.ToString()
			});
			returnStats.Add(new String[]{
				"Maximum Absorb",
				maxAbsorb.ToString()
			});
			returnStats.Add(new String[]{
				"Minimum Absorb",
				minAbsorb.ToString()
			});
			returnStats.Add(new String[]{
				"Maximum Eff. Damage",
				maxEffective.ToString()
			});
			returnStats.Add(new String[]{
				"Minimum Eff. Damage",
				minEffective.ToString()
			});
			returnStats.Add(new String[]{
				"Average Damage",
				(Math.Round(averageDamage,2)).ToString()
			});
			returnStats.Add(new String[]{
				"Average Absorbed",
				(Math.Round(averageAbsorbed,2)).ToString()
			});
			returnStats.Add(new String[]{
				"Average Eff. Damage",
				(Math.Round(averageEffectiveDamage,2)).ToString()
			});

			return returnStats;
		}
	}
}
