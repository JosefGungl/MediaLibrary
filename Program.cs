using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace MediaLibrary
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
            logger.Info(scrubbedFile);
            MovieFile movieFile = new MovieFile(scrubbedFile);
            string choice = "";
            do
            {
                // display choices to user
                Console.WriteLine("1) Add Movie");
                Console.WriteLine("2) Display All Movies");
                Console.WriteLine("3) Find Movie");
                Console.WriteLine("Enter to quit");
                // input selection
                choice = Console.ReadLine();

                if (choice == "1")
                {
                    // Add movie
                    Movie movie = new Movie();
                    // ask user to input movie title
                    Console.WriteLine("Enter movie title");
                    // input title
                    movie.title = Console.ReadLine();
                    // verify title is unique
                    if (movieFile.isUniqueTitle(movie.title)){
                        // input genres
                        string input;
                        do
                        {
                            // ask user to enter genre
                            Console.WriteLine("Enter genre (or done to quit)");
                            // input genre
                            input = Console.ReadLine();
                            // if user enters "done"
                            // or does not enter a genre do not add it to list
                            if (input != "done" && input.Length > 0)
                            {
                                movie.genres.Add(input);
                            }
                            
                        } while (input != "done");
                        // specify if no genres are entered
                        if (movie.genres.Count == 0)
                        {
                            movie.genres.Add("(no genres listed)");
                        }
                        Console.WriteLine("Enter movie director");
                        movie.director = Console.ReadLine();
                        Console.WriteLine("Enter running time (h:m:s)");
                        movie.runningTime = TimeSpan.Parse(Console.ReadLine());
                        // add movie
                        movieFile.AddMovie(movie);
                    }
                } else if (choice == "2")
                {
                    // Display All Movies
                    foreach(Movie m in movieFile.Movies)
                    {
                        Console.WriteLine(m.Display());
                    }
                } else if (choice == "3")
                {
                    //Search for a movie
                    Console.Write("Enter movie title: ");
                    string find = Console.ReadLine();
                    Console.WriteLine("");
                    //find movie with matching name
                    var movieFind = movieFile.Movies.Where(m => m.title.Contains(find)).Select(m => m.title);
                    Console.WriteLine($"There are {movieFind.Count()} movies with {find} in the title");
                    foreach(string t in movieFind){
                        Console.WriteLine($" {t}");
                    }
                }
            } while (choice == "1" || choice == "2" || choice == "3");

            logger.Info("Program ended");
        }
    }
}
