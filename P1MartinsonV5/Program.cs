using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
//--------------------------------------------------------------------------
// File name: Program.cs
// Project name: Project1Martinson
// -------------------------------------------------------------------------
// Creator's name and email: Brianna Martinson martinson@etsu.edu
// Course-Section:  001
// Creation Date: 01/27/2020
// -------------------------------------------------------------------------


/// <summary>
/// The namespace that contains Project 1 code
/// </summary>
namespace P1MartinsonV5
{
    /** 
     * Class Name: Algorithms<br>
     * Class Purpose: This class is an application that takes in user input of 
     * coordinates then calculates the shortest distance of the points to take 
     * 
     * <hr>*
     * Date created: 02/01/2020<br>
     * @author Brianna Martinson
     **/
    class Program
    {
        /**
         * Method Name: main <br>
         * Method Purpose: This is an application that takes in user input of 
         * coordinates then calculates the shortest distance of the points to take
         * 
         * <hr>
         * Date created: 02/01/2020<br> 
         * 
         * <hr> 
         * Notes on specifications, special algorithms, and assumptions: The user 
         * only inputs numbers
         * 
         * <hr>
         * @param  args array of Strings (not used in this program)
         * @throws IOException
         */
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine()); //How many coordinates the input contains
            int[] x = new int[n + 1]; //the array that contains the x coordinates 
            int[] y = new int[n + 1]; //the array that contains the y coordinates

            //Set the origin
            x[0] = 0;
            y[0] = 0;

            String instring; //Holds the string from the input
            string[] values; //Holds the words from the string
            int i = 1; //To itterate the while loop below, set to 1 since origin is known
            while (i < n + 1) //Loops through all the points given and puts them in their respected arrays
            {
                instring = Console.ReadLine(); //the next line of input
                values = instring.Split(' '); //spilts up the strings by a space

                Int32.TryParse(values[0], out x[i]); //adding in x coordinates
                Int32.TryParse(values[1], out y[i]); //adding in y coordinates
                i += 1;
            }
            Stopwatch sw = Stopwatch.StartNew(); //creates sw and starts stopwatch

            double[,] distances = DistanceTable(x, y); //the distance table created from the x and y coordinates

            int[] perm = new int[x.Length - 1]; //the current permutation
            i = 1; //Set i for the loop below, 1 because the origin is known & set
            while (i < x.Length) //Makes the first route
            {
                perm[i - 1] = i; //aka in order 
                i += 1;
            }

            double bestDist = distances[0, perm[0]]; //The fastest distance of a route found (the first route is default the fastest)
            bestDist += distances[perm[perm.Length - 1], 0]; //And back to the origin

            //int[] bestRoute = new int[perm.Length]; //Holds the fastest permutation
            //Array.Copy(perm, 0, bestRoute, 0, perm.Length); //The fastest permutation found (the first route is default the fastest)

            i = 0; //Set i for the loop below
            while (i < perm.Length - 1) //Add the distances between the two points (the first route is default the fastest)
            {
                bestDist += distances[perm[i], perm[i + 1]]; //0 to 1 -> 1 to 2 -> 2 to 3 ...
                i += 1;
            }
            bestDist += distances[perm[perm.Length - 1], perm[0]]; //Add the distance of going back to the origin

            bool nextPerm = true; //stops making permutations when all have been checked
            double dist; //the distance traveled in a permutation
            while (perm[0] != n - 1 && nextPerm) //Go through and find the fastest permutation
            {
                if (perm[perm.Length - 1] > perm[0]) //Means this permutation has not been done before
                {
                    //Look up the distance of the newPerm and see if its the new min
                    dist = distances[0, perm[0]]; //0 is always the origin
                    dist += distances[perm[perm.Length - 1], 0]; //Add the last point to the origin
                    i = 0; //Set i for the loop below
                    while (i < perm.Length - 1) //Add the distances between the two points
                    {
                        //Console.WriteLine("Current permutation's distance: " + dist);
                        dist += distances[perm[i], perm[i + 1]]; //0 to 1 + 1 + 2 + ...
                        if (dist > bestDist) //The new permutation is slower so move on
                        {
                            break;
                        }
                        i += 1;
                    }
                    if (dist < bestDist) //One last check to see if the new permutation is slower
                    {
                        bestDist = dist; //new shorter distance
                        //Array.Copy(perm, 0, bestRoute, 0, perm.Length); //new route found
                    }
                    
                }
                nextPerm = NextPermutation(perm); //Generate the next permutation
            }

            sw.Stop(); //stops stopwatch

            //Output results

            Console.WriteLine("Time used: {0} secs", sw.Elapsed.TotalMilliseconds / 1000); //Display run time
            Console.WriteLine("Best distance found: " + bestDist); //Display the best distance found
            /*
            Console.Write("Best route found: ");
            for (int j = 0; j < bestRoute.Length; j++) //Display the best route found
            {
                Console.Write(bestRoute[j] + " ");
            }
            */
        } //end of main

        /// <summary>
        /// Calculates all the distances between all of the given points and puts them into a table
        /// </summary>
        /// <param name="xCords">The x coordinates</param>
        /// <param name="yCords">The y coordinates</param>
        /// <returns>A double 2D array that contains all of the calculated distances</returns>
        public static double[,] DistanceTable(int[] xCords, int[] yCords)
        {
            double[,] table = new double[xCords.Length, yCords.Length]; //the distance table
            table[0, 0] = 0; //the origin is always 0
            int x = -1; //To iterate the loop below, start at -1 to not get array out of bounds 
            int y; //To iterate the inner loop below
            while (x < xCords.Length - 1) //go row by row
            {
                y = -1; //start at -1 to not get array out of bounds 
                while (y < yCords.Length - 1) //iterate by column number
                {
                    if (table[y + 1, x + 1] != 0) //See if it is a previously calculated distance
                    {
                        table[x + 1, y + 1] = table[y + 1, x + 1]; //Use the previously made distance
                    }
                    else
                    {
                        table[x + 1, y + 1] = Math.Sqrt((Math.Pow(xCords[y + 1] - xCords[x + 1], 2.0)) + (Math.Pow(yCords[y + 1] - yCords[x + 1], 2.0)) * 1.0);
                    }
                    y += 1;
                }
                x += 1;
            }
            return table;
        }//end of DistanceTable

        /// <summary>
        /// Changes the given permutation to the next permutation
        /// </summary>
        /// <param name="perm">The current permutation</param>
        /// <returns>A boolean that says if another permutation was made or not</returns>
        public static bool NextPermutation(int[] perm)
        {
            //Find a decrease
            int i = perm.Length - 1; //The index of the current value
            while (i > 0 && perm[i - 1] >= perm[i]) //Find a decrease from R->L
            { 
                i -= 1;
            }
            if (i <= 0) //No decrease so no new permutation
            {
                return false; //A new permutation cannot be made
            }

            //Find the next closest number
            int j = perm.Length - 1; //The index of the next closest value
            while (perm[j] < perm[i - 1]) //Loop through to see where the next closest value is
            {
                j -= 1;
            }

            //Swap the values
            int temp = perm[j]; //Hold the value 
            perm[j] = perm[i - 1]; //Set index j to its new value
            perm[i - 1] = temp;

            //Reverse the array
            j = perm.Length - 1; //Set j for the loop below
            while (i < j) //Reverse all to the left of i
            {
                //Reversing from the index to the end of the array (outside in)
                temp = perm[i]; //Hold the value
                perm[i] = perm[j]; //swap
                perm[j] = temp; //swap
                i += 1; //R->
                j -= 1; //L<-
            }
            return true; //A new permutation has been made
        } //end of NextPermutation
    } //end of class
} //end of namespace
