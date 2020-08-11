// Copyright 2014 The Rector & Visitors of the University of Virginia
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.IO;

namespace Sensus.UI
{
    class test
    {
        public static void Main()
        {
            try
            {
                // Get the current directory.
                string path = Directory.GetCurrentDirectory();
                string target = @"c:\temp";
                Console.WriteLine("The current directory is {0}", path);
                if (!Directory.Exists(target))
                {
                    Directory.CreateDirectory(target);
                }

                // Change the current directory.
                Environment.CurrentDirectory = (target);
                if (path.Equals(Directory.GetCurrentDirectory()))
                {
                    Console.WriteLine("You are in the temp directory.");
                }
                else
                {
                    Console.WriteLine("You are not in the temp directory.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}
