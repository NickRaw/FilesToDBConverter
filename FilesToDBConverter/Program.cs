using System;
using FilesToDBConverter.Models;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

namespace FilesToDBConverter
{
    class Program
    {
        public static string folderPathInComputer = "";
        public static List<Category> categories = new List<Category>();
        public static bool[] completedSteps = { false, false, false };
        public static int currentStep = 0;

        static void Main(string[] args)
        {
            bool programIsDone = false;
            while (!programIsDone) // NOTE: Place functions in menu options. ATTENTION: After every succesfull function change completedsteps.
            {
                Console.WriteLine("\n\n");
                Console.WriteLine("######################## Files to DB Converter ########################");
                Console.WriteLine("- Type 'stop' to stop the program");
                Console.WriteLine("- Type one of the optional commands below to redo the action.");
                Console.WriteLine("- When a step is done it will show DONE after the step.\n  The program then will continue to the next step after pressing ENTER.");
                
                Console.WriteLine("\n########### The steps to convert ###########");
                Console.Write("- 1. - COMMAND:'set folderpath' - Set folderpath ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(folderPathInComputer);
                Console.ResetColor();
                if (completedSteps[0]) { Console.ForegroundColor = ConsoleColor.Green;Console.Write(" DONE"); Console.ResetColor(); }
                
                Console.Write("\n- 2. - COMMAND:'prepare files' - Prepare files for upload (Gets filepath, title and match with category per file)");
                if (completedSteps[1]) { Console.ForegroundColor = ConsoleColor.Green; Console.Write(" DONE"); Console.ResetColor(); }
                Console.Write("\n- 3. - COMMAND:'upload to db' - Upload files to database");
                if (completedSteps[2]) { Console.ForegroundColor = ConsoleColor.Green; Console.Write(" DONE"); Console.ResetColor(); }

                if (completedSteps[0]) { Console.WriteLine("\n\n#### Optional options ####"); Console.WriteLine("Type 'show folderfiles' to show all files in folderpath"); }
                if (completedSteps[1]) { Console.WriteLine("Type 'show prepared files' show all prepared files"); }
                if (completedSteps[2]) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("\nProgram completed. Database created and filled with files and folders. Typ 'stop' to stop the program."); Console.ResetColor(); }

                Console.Write("\nMenu option: ");
                string mainMenuInputString = Console.ReadLine();

                if (mainMenuInputString.ToLower() == "set folderpath")
                {
                    // set folderpath
                    Console.Clear();
                    SetFolderPath();
                }
                else if (mainMenuInputString.ToLower() == "stop")
                {
                    programIsDone = true;
                }
                else if (mainMenuInputString.ToLower() == "show folderfiles")
                {
                    if (completedSteps[0])
                    {
                        // Check all files in folderpath
                        ShowFilesInFolderDirectory();
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("##### Set the folderpath first! #####");
                        Console.ResetColor();
                    }
                    
                }
                else if (mainMenuInputString.ToLower() == "prepare files")
                {
                    if (completedSteps[0])
                    {
                        // prepare files
                        PutFilesInClasses();
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("##### Set the folderpath first! #####");
                        Console.ResetColor();
                    }
                }
                else if (mainMenuInputString.ToLower() == "show prepared files")
                {
                    if (completedSteps[1])
                    {
                        // Check all files in classes
                        ShowFilesInClasses();
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("##### Prepare files first! #####");
                        Console.ResetColor();
                    }
                }

                else if (mainMenuInputString.ToLower() == "upload to db")
                {
                    if (completedSteps[1])
                    {
                        // upload to database
                        UploadFilesToDatebase();
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("##### Prepare files first! #####");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("##### Wrong Input #####");
                    Console.ResetColor();
                }

            }
        }

        public static void SetFolderPath()
        {
            bool folderpathConfirmed = false;
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n###### Fill in folderpath of files ######\n");
                Console.ResetColor();

                Console.WriteLine("Fill in the complete path.\nBegin with the name of the disk the files are in and go to the folder that contains the files.\n\nExample:C/Users/Nick/Documents/Images");
                Console.Write("Folderpath: ");
                folderPathInComputer = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nThis is you folderpath:");
                Console.WriteLine(folderPathInComputer);
                Console.ResetColor();

                // Confermatie dat het pad juist is ingevuld
                ConsoleKey response;
                do
                {
                    Console.Write("\nIs this right? [y/n]");
                    response = Console.ReadKey(false).Key; // Vragen om y of n in te vullen
                    if (response != ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                    }
                } while (response != ConsoleKey.Y && response != ConsoleKey.N);
                folderpathConfirmed = response == ConsoleKey.Y; // Code gaat verder als y is ingevuld en begint opnieuw als n is ingevuld.
            } while (!folderpathConfirmed);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Path saved!");

            // Kleine animatie voordat we terug gaan naar het menu
            Console.Write("\nPress a key to continue...");
            Console.ReadLine();
            Console.ResetColor();
            completedSteps[0] = true;
        }

        public static void ShowFilesInFolderDirectory()
        {
            string folderName = ""; // This is the string that is shown at the end with all folders and their total amount of files

            int totalFilesInMainDirectory = 0;
            var directories = Directory.GetDirectories(folderPathInComputer);
            foreach (string directory in directories)
            {
                Console.WriteLine("###### " + directory + " ######\n"); //String formatting to get directoryname and show it as title for list of files in folder
                folderName = folderName + "\n###### " + directory + ": "; // Add foldername to string that is shown in final
                
                int totalFilesInDirecory = 0;
                var files = Directory.GetFiles(directory);
                foreach (string file in files)
                {
                    Console.WriteLine(file);
                    totalFilesInDirecory++;
                    totalFilesInMainDirectory++;
                }
                folderName = folderName + totalFilesInDirecory.ToString();
                Console.WriteLine();
            }
            Console.WriteLine(folderName); // Final string with all folders and total amount of files in folder
            Console.WriteLine("Total files prepared: " + totalFilesInMainDirectory.ToString()); // Total amount of files in all folders
            
            Console.Write("Press enter to go back...");
            Console.ReadLine();
        }

        public static void UploadFilesToDatebase()
        {
            SqlConnectionStringBuilder databaseConnection = new SqlConnectionStringBuilder();
            string datasource = "";
            string userID = "";
            string password = "";
            string database = "";

            bool confirmed = false;
            do
            {
                Console.WriteLine("###### SERVER ######");
                Console.WriteLine("Give the SQL Server you want to connect to");
                Console.Write("Server: ");
                datasource = Console.ReadLine();
                Console.WriteLine("\n###### USERNAME ######");
                Console.WriteLine("Give the username of the user you want to use");
                Console.Write("Username: ");
                userID = Console.ReadLine();
                Console.WriteLine("\n###### PASSWORD ######");
                Console.WriteLine("Give the password of the user");
                Console.Write("Wachtwoord: ");
                password = Console.ReadLine();
                Console.WriteLine("\n###### DATABASE ######");
                Console.WriteLine("How do you want to name the database");
                Console.Write("Database name: ");
                database = Console.ReadLine();

                Console.WriteLine("\nYou filled in the following:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Server");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(datasource);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Username");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(userID);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Password");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(password);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDatabase");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(database);
                Console.ResetColor();

                // Confermatie dat het pad juist is ingevuld
                ConsoleKey response;
                do
                {
                    Console.Write("\nAre you sure to proceed? [y/n]");
                    response = Console.ReadKey(false).Key; // Vragen om y of n in te vullen
                    if (response != ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                    }
                } while (response != ConsoleKey.Y && response != ConsoleKey.N);
                confirmed = response == ConsoleKey.Y; // Code gaat verder als y is ingevuld en begint opnieuw als n is ingevuld.
            } while (!confirmed);
            
            databaseConnection.DataSource = datasource;
            databaseConnection.UserID = userID;
            databaseConnection.Password = password;
            try
            {
                using (SqlConnection con = new SqlConnection(databaseConnection.ConnectionString))
                {
                    con.Open();
                    string createDatabaseSQL = "CREATE DATABASE " + database + " ;";
                    using (SqlCommand cmd = new SqlCommand(createDatabaseSQL, con))
                    {
                        cmd.ExecuteNonQuery();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Database " + database + " made");
                    }

                    string createTableCategorySQL = "CREATE TABLE " + database + ".dbo.TBL_Category (categoryID int NOT NULL IDENTITY, categoryName varchar(255), PRIMARY KEY (categoryID));";
                    string createTableImageSQL = "CREATE TABLE " + database + ".dbo.TBL_File (fileID int NOT NULL IDENTITY, fileTitle varchar(255),filePath varchar(255), fileExtention varchar(255), categoryID int, PRIMARY KEY (fileID), FOREIGN KEY (categoryID) REFERENCES TBL_Category(categoryID));";

                    using (SqlCommand cmd = new SqlCommand(createTableCategorySQL, con))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Table TBL_Category made");
                    }

                    using (SqlCommand cmd = new SqlCommand(createTableImageSQL, con))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Table TBL_File made");
                    }
                } // Database and tables are succesfully created

                Console.ResetColor();
                Console.WriteLine("\nPress enter to proceed and fill the database...");
                Console.ReadLine();

                databaseConnection.InitialCatalog = database;

                using (SqlConnection con = new SqlConnection(databaseConnection.ConnectionString))
                {
                    con.Open();
                    //######################### SQL FOR INSERT IN DATABASE #########################
                    foreach (Category category in categories)
                    {
                        string sql = "INSERT INTO TBL_Category (categoryName) VALUES (@categoryName)";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@categoryName", category.CategoryName);
                            cmd.ExecuteNonQuery();
                        }

                        foreach (FilesToDBConverter.Models.File file in category.Files)
                        {
                            string sql_2 = "INSERT INTO TBL_File (fileTitle, filePath, fileExtention, categoryID) VALUES (@fileTitle, @filePath, @fileExtention, @categoryID)";
                            using (SqlCommand cmd = new SqlCommand(sql_2, con))
                            {
                                cmd.Parameters.AddWithValue("@fileTitle", file.FileTitle);
                                cmd.Parameters.AddWithValue("@filePath", file.FilePath);
                                cmd.Parameters.AddWithValue("@fileExtention", file.FileExtention);
                                cmd.Parameters.AddWithValue("@categoryID", category.CategoryID);
                                cmd.ExecuteNonQuery();
                            }
                        }

                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Database filled");
                    Console.ResetColor();
                    Console.WriteLine("\nPress enter to continue...");
                    Console.ReadLine();
                    //######################### EINDE SQL VOOR INSERT IN DATABASE #########################
                    completedSteps[2] = true;
                }


            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                
                Console.ResetColor();
                Console.WriteLine("\nPress enter to continue...");
                Console.ReadLine();
            }

        }

        public static void PutFilesInClasses()
        {
            var directories = Directory.GetDirectories(folderPathInComputer);
            int directoryID = 1;
            int fileID = 1;
            foreach (string directory in directories)
            {
                Category category = new Category(directoryID, new DirectoryInfo(directory).Name);
                
                Console.WriteLine(new DirectoryInfo(directory).Name + " class made");

                var files = Directory.GetFiles(directory);
                foreach (string file in files)
                {
                    string filename = new DirectoryInfo(file).Name.Replace("-", " ");
                    string fileExtention = Path.GetExtension(file);
                    filename = filename.Replace("_", " ");
                    filename = filename.Remove(filename.Length - 4);
                    new FilesToDBConverter.Models.File(fileID, filename, file, fileExtention, category);
                    Console.WriteLine(new DirectoryInfo(file).Name);
                    fileID++;
                }
                categories.Add(category);
                directoryID++;
            }
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
            completedSteps[1] = true;
        }

        public static void ShowFilesInClasses()
        {
            foreach (Category category in categories)
            {
                Console.WriteLine("###### " + category.CategoryName + " ######");
                foreach (FilesToDBConverter.Models.File meme in category.Files)
                {
                    Console.WriteLine(meme.FileTitle);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }


    }
}
