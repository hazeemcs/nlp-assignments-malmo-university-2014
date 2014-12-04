﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment1
{
	class Processor
	{

		internal Dictionary<string, int> CountNGrams(List<string> wordList, NGram inNGram)
		{
			Dictionary<string, int> res = new Dictionary<string, int>();

			switch (inNGram)
			{
				// count unigrams
				case NGram.UNIGRAM:
					for (int i = 0; i < wordList.Count; i++)
					{
						string word = wordList[i];

						if (res.ContainsKey(word))
							res[word] = res[word] + 1;
						else
							res.Add(word, 1);
					}

					return res;

				// count bigrams
				case NGram.BIGRAM:
					for (int i = 0; i < wordList.Count; i++)
					{
						string word1 = wordList[i];

						if (wordList.Count > i + 1)
						{
							string word2 = wordList[i + 1];
							string bigram = word1 + " " + word2;

							if (res.ContainsKey(bigram))
								res[bigram] = res[bigram] + 1;
							else
								res.Add(bigram, 1);
						}
					}

					return res;
			}

			throw new ArgumentException("Illegal NGram enumerator was passed");

		}

		internal Dictionary<string, float> CalcProb	(Dictionary<string, int> wordListBigrams, Dictionary<string, int> wordListUnigrams)
		{
			Dictionary<string, float> res = new Dictionary<string,float>();

			// this part calculates bigram probabilities (the prob of a unigram given a different unigram)
			foreach (KeyValuePair<string, int> entry in wordListBigrams)
			{
				string bigram = entry.Key;
				int bigramcount = entry.Value;

				string[] split = bigram.Split(' ');

				for (int i = 0; i < split.Length; i++)
				{
					string unigram = split[i];
					int unigramcount = wordListUnigrams[unigram];


					float prob = (float)bigramcount / (float)unigramcount;

					res[entry.Key] = prob;

					// FIX: this conditional statement should only be accessible in Mode.TEST
					if (i == 0)
					{
						//Console.WriteLine("\""+bigram +"\""+ " has a probability of " + probability);
						findLowProb(prob, entry.Key);
					}

				}

			}
			//Console.WriteLine("---END---");
			//Console.WriteLine(unigramCount + " -- " + bigramCount);

			return res;
		}

		internal double calcPerplex(Dictionary<string, int> wordListUnigrams, Dictionary<string, float> probListBigrams)
		{
			// --variables for counters and results of processing--
			//      --results--
			int n = 0;
			double sum = 0;

			//      --counters--
			int counterEnd = 0;
			int counter = 0;

			// --calculation of N--
			foreach (KeyValuePair<string, int> entry in wordListUnigrams)
			{
				n += entry.Value;
				counterEnd++;
				// DESC: test code
				//if (counterEnd == 5)
				//    break;
			}
			// EXTRA_INFO: by calculating word tokens divided by word types we know that every word appears 5.8 times on average
			Console.WriteLine("value of n: " + n);

			// --summation of bigram probabilities in log space
			// NOTE: might want to use double datatype everywhere for probabilities (although this will require more memory/processing)
			foreach (KeyValuePair<string, float> entry in probListBigrams)
			{
				counter++;

				sum += Math.Log((double)entry.Value);

				//Console.WriteLine("sum is: " + sum + " after " + counter + " iterations.");
				//Console.WriteLine("d is: " + d + ", dLog is: " + dLog + "\n");
			}

			Console.WriteLine("total sum: " + sum);

			// BOOKMARK: continue with the rest of calc for perplexity!




			// FIX: return res once we have calculated perplexity!
			//return res;
			return sum;

		}


		private void findLowProb(float probability, string bigram)
		{
			float threshholdLowprob = (float)1 / 4000f;
			if (threshholdLowprob > probability)
			{
				//Console.WriteLine(bigram + "has a low prob of: " + probability);
			}
		}
	}
}