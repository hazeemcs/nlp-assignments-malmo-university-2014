﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLP_Assignment1
{
    class Processor
    {

        public void CountNGramsAndCalcProb( List<string> wordList, Dictionary<string, int> wordListUnigrams,
                                            Dictionary<string, int> wordListBigrams, Dictionary<string, float> probListBigrams)
        {


            // this part counts unigrams & bigrams
            for (int i = 0; i < wordList.Count; i++)
            {
                string word = wordList[i];

                // count unigrams
                if (wordListUnigrams.ContainsKey(word))
                {
                    wordListUnigrams[word] = wordListUnigrams[word] + 1;
                }
                else
                {
                    wordListUnigrams.Add(word, 1);
                }

                // count bigrams
                if (wordList.Count > i + 1)
                {
                    string word2 = wordList[i + 1];

                    string bigram = word + " " + word2;

                    if (wordListBigrams.ContainsKey(bigram))
                    {
                        wordListBigrams[bigram] = wordListBigrams[bigram] + 1;
                    }
                    else
                    {
                        wordListBigrams.Add(bigram, 1);
                    }
                }
            }


            // this part calculates bigram probabilities (the prob of a unigram given a different unigram)
            foreach (KeyValuePair<string, int> entry in wordListBigrams)
            {
                string bigram = entry.Key;
                int bigramcount = entry.Value; // hur många gånger finns "kvinnors jämlikhet"

                string[] split = bigram.Split(' '); // Kvinnörs jämlikhet

                for (int i = 0; i < split.Length; i++)
                {
                    string unigram = split[i];
                    int unigramcount = wordListUnigrams[unigram]; // hur många gånger finns "kvinnors"


                    float probability = (float)bigramcount / (float)unigramcount;

                    probListBigrams[entry.Key] = probability;

                    if (i == 0)
                    {
                        //Console.WriteLine("\""+bigram +"\""+ " has a probability of " + probability);
                        // TODO: move this up to LanguageModel.cs
                        findLowProb(probability, entry.Key);
                    }

                }

            }
            //Console.WriteLine("---END---");
            //Console.WriteLine(unigramCount + " -- " + bigramCount);
        }

        public void calcPerplex(Dictionary<string, int> wordListUnigrams, Dictionary<string, float> probListBigrams)
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
                // NOTE: test code
                //if (counterEnd == 5)
                //    break;
            }
            // NOTE: by calculating word tokens divided by word types we know that every word appears 5.8 times on average
            Console.WriteLine("value of n: " + n);

            // --summation of bigram probabilities in log space
            // TODO: might want to use double datatype everywhere for probabilities (although this will require more memory/processing)
            foreach (KeyValuePair<string, float> entry in probListBigrams)
            {
                counter++;

                sum += Math.Log((double)entry.Value);

                //Console.WriteLine("sum is: " + sum + " after " + counter + " iterations.");
                //Console.WriteLine("d is: " + d + ", dLog is: " + dLog + "\n");
            }

            Console.WriteLine("total sum: " + sum);

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
