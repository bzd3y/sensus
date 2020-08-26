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
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace Sensus.UI.MindTrailsBehind
{
    public class SessionModel
    {
        public int block { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public int numberMissing { get; set; }
        public string word1 { get; set; }
        public string word2 { get; set; }
        public string statement1 { get; set; }
        public string statement2 { get; set; }
        public string question { get; set; }
        public string positive { get; set; } // positive answer (yes or no)
        public string negative { get; set; } // negative answer (yes or no) 
        public string answer { get; set; } // is the correct answer positive or negative?
        public string type { get; set; }
        public string format { get; set; }
        public string immersion { get; set; }
        public string ifthen { get; set; }
    }

    public class Root
    {
        public List<SessionModel> firstSession { get; set; }

    }
}
