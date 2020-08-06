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

namespace Sensus.MindTrailsBehind
{
    public class ScenarioViewModel 
    {
        public ScenarioViewModel()
        {
        }
        int block;
        string name;
        string title;
        string word1;
        string word2;
        string statement1;
        string statement2;
        string question;
        string positive; // positive answer (yes or no)
        string negative; // negative answer (yes or no) 
        string answer; // positive or negative 
        string type;

        public int Block
        {
            get { return block; }
            set { block = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Word1
        {
            get { return word1; }
            set { word1 = value; }
        }
        public string Word2
        {
            get { return word2; }
            set { word2 = value; }
        }
        public string Statement1
        {
            get { return statement1; }
            set { statement1 = value; }
        }
        public string Statement2
        {
            get { return statement2; }
            set { statement2 = value; }
        }
        public string Question
        {
            get { return question; }
            set { question = value; }
        }
        public string Positive
        {
            get { return positive; }
            set { positive = value; }
            // if Positive == yes && answer == positive:
                // yes is correct
            // if Positive == no && answer == positive:
                // no is correct
            // if Positive == no && answer == negative:
                // yes is correct
            // if Positive == yes && answer == negative:
                // no is correct
        }
        public string Negative
        {
            get { return negative; }
            set { negative = value; }
        }
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
