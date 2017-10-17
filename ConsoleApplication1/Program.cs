using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static uint[] Options(ref Dictionary<uint, char> graph, ref uint start, ref uint height, ref uint length)
        {
            uint[] options = new uint[8];
            //kx = 24, ky = 16, px = 8
            //move north
            if ((start >> 16) == (start - 1) % 65536)
            {
                if (graph.ContainsKey(((start - (1 << 16)) >> 16)))
                {
                    if (graph[((start - (1 << 16)) >> 16)] == '.')
                        options[0] = start - 1 - (1 << 16);
                    else
                        options[0] = 0;
                }
                else
                    options[0] = 0;
            }
            else if (graph.ContainsKey((start - 1) % 65536))
            {
                if (graph[(start - 1) % 65536] == '.')
                    options[1] = start - 1;
                else
                    options[1] = 0;
            }
            else
                options[1] = 0;

            //move east
            if ((start >> 16) == ((start + (1 << 8)) % 65536))
            {
                //Console.WriteLine("hi!");
                if (graph.ContainsKey(((start + (1 << 24)) >> 16)))
                {
                    if (graph[((start + (1 << 24)) >> 16)] == '.')
                        options[2] = start + (1 << 8) + (1 << 24);
                    else
                        options[2] = 0;
                }
                else
                    options[2] = 0;
            }
            else if (graph.ContainsKey((start + (1 << 8)) % 65536))
            {
                //Console.WriteLine("{0} {1}", (start >> 24), (((start >> 8) % 256) + 1));
                if (graph[(start + (1 << 8)) % 65536] == '.')
                    options[3] = start + (1 << 8);
                else
                    options[3] = 0;
            }
            else
                options[3] = 0;

            //move west
            if ((start >> 16) == ((start - (1 << 8)) % 65536))
            {
                if (graph.ContainsKey(((start - (1 << 24)) >> 16)))
                {
                    if (graph[((start - (1 << 24)) >> 16)] == '.')
                        options[4] = start - (1 << 8) - (1 << 24);
                    else
                        options[4] = 0;
                }
                else
                    options[4] = 0;
            }
            else if (graph.ContainsKey((start - (1 << 8)) % 65536))
            {
                if (graph[(start - (1 << 8)) % 65536] == '.')
                    options[5] = start - (1 << 8);
                else
                    options[5] = 0;
            }
            else
                options[5] = 0;

            //move south
            if ((start >> 16) == (start + 1) % 65536)
            {
                if (graph.ContainsKey(((start + (1 << 16)) >> 16)))
                {
                    if (graph[((start + (1 << 16)) >> 16)] == '.')
                        options[6] = start + 1 + (1 << 16);
                    else
                        options[6] = 0;
                }
                else
                    options[6] = 0;
            }
            else if (graph.ContainsKey((start + 1) % 65536))
            {
                if (graph[(start + 1) % 65536] == '.')
                    options[7] = start + 1;
                else
                    options[7] = 0;
            }
            else
                options[7] = 0;


            return options;
        }


        static void Main(string[] args)
        {
            string[] firstline = Console.ReadLine().Split(' ');
            uint length = uint.Parse(firstline[0]);
            uint height = uint.Parse(firstline[1]);
            char mode = firstline[2][0];
            uint start = 0;
            uint finish = 0;
            uint end = 0;
            bool done = false;
            Dictionary<uint, char> graph = new Dictionary<uint, char>();
            Dictionary<uint, char> visited = new Dictionary<uint, char>();
            Queue<uint> Q = new Queue<uint>();

            for (uint y = 0; y < height; y++)
            {
                string row = Console.ReadLine();
                for (uint x = 0; x < length; x++)
                {
                    uint position = (x << 8) + y;
                    if (row[(int)x] == '!')
                    {
                        start += (x << 24) + (y << 16);
                        graph.Add(position, '.');
                    }
                    else if (row[(int)x] == '+')
                    {
                        start += (x << 8) + y;
                        graph.Add(position, '.');
                    }
                    else if (row[(int)x] == '?')
                    {
                        finish += (x << 8) + y;
                        graph.Add(position, '.');
                    }
                    else
                    {
                        graph.Add(position, row[(int)x]);
                    }
                }
            }

            Q.Enqueue(start);
            visited.Add(start, '0');
            while (Q.Count > 0 && !done)
            {
                //Console.WriteLine("I'm in!");
                uint position = Q.Dequeue();
                uint[] o = Options(ref graph, ref position, ref height, ref length);
                for (uint i = 0; i < 8; i++)
                {

                    if ((!visited.ContainsKey(o[i])) && (o[i] != 0))
                    {
                        if (o[i] >> 16 == finish)
                        {
                            //Console.WriteLine("I'm done!");
                            end = o[i];
                            done = true;
                        }
                        //Console.WriteLine("{0} {1} {2} {3}",o[i] % 256, (o[i] >> 8) % 256, (o[i] >> 16) % 256, (o[i] >> 24) % 256);
                        Q.Enqueue(o[i]);
                        char news;
                        switch (i)
                        {
                            case 0:
                                news = 'N';
                                break;
                            case 1:
                                news = 'n';
                                break;
                            case 2:
                                news = 'E';
                                break;
                            case 3:
                                news = 'e';
                                break;
                            case 4:
                                news = 'W';
                                break;
                            case 5:
                                news = 'w';
                                break;
                            case 6:
                                news = 'S';
                                break;
                            case 7:
                                news = 's';
                                break;
                            default:
                                news = 'X';
                                Console.WriteLine("oops");
                                break;
                        }
                        visited.Add(o[i], news);
                    }
                }

            }

            if (Q.Count == 0)
            {
                Console.WriteLine("No solution");
                //Console.WriteLine("{0} {1}", finish % 256, (finish >> 8) % 256);
            }
            else
            {
                int count = 0;
                string path = "";
                bool finished = false;
                while (!finished)
                {
                    //Console.WriteLine("{0} {1} {2} {3} {4}", (end >> 16) % 256, end >> 24, (end >> 8) % 256, end % 256, visited[end]);
                    path += char.ToUpper(visited[end]);
                    count++;
                    switch (visited[end])
                    {
                        case 'N':
                            end += 1 + (1 << 16);
                            break;
                        case 'n':
                            end += 1;
                            break;
                        case 'E':
                            //Console.WriteLine("{0} {1} {2} {3}", (end >> 16) % 256, end >> 24, (end >> 8) % 256, end % 256);
                            end -= (1 << 8) + (1 << 24);
                            break;
                        case 'e':
                            end -= (1 << 8);
                            break;
                        case 'W':
                            end += (1 << 8) + (1 << 24);
                            break;
                        case 'w':
                            end += (1 << 8);
                            break;
                        case 'S':
                            end -= 1 + (1 << 16);
                            break;
                        case 's':
                            end -= 1;
                            break;
                        case '0':
                            finished = true;
                            break;
                    }
                }
                count--;
                Console.WriteLine(count);
                if (mode == 'P')
                {
                    path = path.Remove(count);
                    char[] charpath = path.ToCharArray();
                    Array.Reverse(charpath);
                    Console.WriteLine(charpath);//
                }
            }


        }
    }
}
