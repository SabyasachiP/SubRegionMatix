using System;
using System.Collections.Generic;
using System.Linq;

namespace SubRegionMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            int rowSize, colSize, threshold;
            Console.Write("Please provide the row size of 2D array:");
            int.TryParse(Console.ReadLine(), out rowSize);
            Console.Write("Please provide the column size of 2D array:");
            int.TryParse(Console.ReadLine(), out colSize);

            List<Cell> filteredCells = new List<Cell>();
            int totalRegion = default;

            Console.Write("Please provide the threshold value:");
            int.TryParse(Console.ReadLine(), out threshold);

            if (rowSize == default || colSize == default)
            {
                Console.WriteLine("Invalid input");
                Console.ReadLine();
            }
            else
            {
                int[,] arr1 = new int[rowSize, colSize];
                for (int a = 0; a < rowSize; a++)
                {
                    for (int b = 0; b < colSize; b++)
                    {
                        Console.Write("element - [{0},{1}] : ", a, b);
                        arr1[a, b] = Convert.ToInt32(Console.ReadLine());
                    }
                }

                Console.Write("\nThe matrix is : \n");
                for (var a = 0; a < rowSize; a++)
                {
                    Console.Write("\n");
                    for (var b = 0; b < colSize; b++)
                        Console.Write("{0}\t", arr1[a, b]);
                }
                Console.Write("\n\n");
    
                Console.WriteLine("Threshold value : {0}", threshold);

                for (var b = 0; b < colSize; b++)
                {
                    for (var a = rowSize - 1; a >= 0; a--)
                    {
                        if (arr1[a, b] > threshold)
                        {
                            Cell c = new Cell();
                            c.x = b;
                            c.y = a;
                            c.region = 0;
                            c.value = arr1[a, b];
                            filteredCells.Add(c);
                        }
                    }
                }
                if (filteredCells != null && filteredCells.Count > 0)
                {
                    markRegion(filteredCells, rowSize, colSize, totalRegion);

                    var items = filteredCells.GroupBy(a => a.region);
                    Console.WriteLine("Filtered Positions are:");
                    foreach (var group in items)
                    {
                        Console.WriteLine("Sub Region: {0} ", group.Key);
                        foreach (var cell in group)
                        {
                            Console.Write("[{0},{1}],", cell.x, cell.y);
                        }
                        Console.Write("\n\n");
                    }
                    totalRegion = items.OrderByDescending(a => a.Key).First().Key;
                    CalculateCenterOfMass(totalRegion, items);
                }
            }
            Console.ReadLine();
        }

        private static void markRegion(List<Cell> filteredCells, int rowSize, int colSize, int totalRegion)
        {
            for (int regNo = 1; regNo < (rowSize * colSize) + 1; regNo++)
            {
                markRegionToCell(regNo, filteredCells.Where(a => a.check == false).ToList(), rowSize, colSize);
            }
        }
        private static void findCellToLeft(int x, int y, int regNoo, List<Cell> filteredList, int colSize)
        {
            x = x - 1;
            if (x > colSize || x < 0)
            {
                return;
            }
            else
            {
                if (search(x, y, regNoo, filteredList) == true)
                {
                    findCellToLeft(x, y, regNoo, filteredList, colSize);
                }
            }
        }
        private static void findCellToRight(int x, int y, int regNo, List<Cell> filteredList, int colSize)
        {
            x = x + 1;
            if (x > colSize || x < 0)
            {
                return;
            }
            else
            {
                if (search(x, y, regNo, filteredList) == true)
                {
                    findCellToRight(x, y, regNo, filteredList, colSize);
                }
            }
        }
        private static void findCellToDown(int x, int y, int regNo, List<Cell> filteredList, int rowSize)
        {
            y = y - 1;
            if (y > rowSize || y < 0)
            {
                return;
            }
            else
            {
                if (search(x, y, regNo, filteredList) == true)
                {
                    findCellToDown(x, y, regNo, filteredList, rowSize);
                }
            }
        }
        private static void findCellToUp(int x, int y, int regNo, List<Cell> filteredList, int rowSize)
        {
            y = y + 1;
            if (y > rowSize || y < 0)
            {
                return;
            }
            else
            {
                if (search(x, y, regNo, filteredList) == true)
                {
                    findCellToUp(x, y, regNo, filteredList, rowSize);
                }
            }
        }
        private static void markRegionToCell(int regNo, List<Cell> filteredList, int rowSize, int colSize)
        {
            if (filteredList.Any())
            {
                filteredList.First().region = regNo;
                filteredList.First().check = true;
                foreach (var element in filteredList)
                {
                    if (element.check == true)
                    {
                        search(element.x, element.y, regNo, filteredList);
                        findCellToRight(element.x, element.y, regNo, filteredList, colSize);
                        findCellToLeft(element.x, element.y, regNo, filteredList, colSize);
                        findCellToUp(element.x, element.y, regNo, filteredList, rowSize);
                        findCellToDown(element.x, element.y, regNo, filteredList, rowSize);
                    }
                }
            }
        }

        public static bool search(int x, int y, int regNo, List<Cell> filteredList)
        {
            bool b = false;
            for (var element = 0; element < filteredList.Count; element++)
            {
                if ((filteredList[element].x == x) && (filteredList[element].y == y))
                {
                    filteredList[element].region = regNo;
                    filteredList[element].check = true;
                    b = true;
                }
            }

            return b;
        }

        public static void CalculateCenterOfMass(int totalRegion, IEnumerable<IGrouping<int, Cell>> groupedList)
        {
            int sum;
            int avg;
            int i;
            int regcnt = 0;
            if (totalRegion != 0)
            {
                foreach (var elm in groupedList)
                {
                    sum = 0;
                    i = 0;
                    foreach (var cell in elm)
                    {
                        i++;
                        sum += cell.value;
                    }
                    if (i == 0 && sum != 0)
                    {
                    }
                    else
                    {
                        avg = sum / i;
                        Console.Write("center of mass for region  {0} average: ", regcnt+1);
                        Console.Write(avg);
                        Console.Write("\n");
                    }
                    regcnt++;
                    if (regcnt == totalRegion)
                    {
                        break;
                    }
                }
            }
            else
            {
                Console.Write("regions: 0");
                Console.Write("\n");
                Console.Write("cener of mass average: None");
                Console.Write("\n");
            }
        }
    }
}
