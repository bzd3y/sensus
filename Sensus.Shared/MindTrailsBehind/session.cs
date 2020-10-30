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
    public class session
    {
        public int Block { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Word1 { get; set; }
        public string Word2 { get; set; }
        public string Statement1 { get; set; }
        public string Statement2 { get; set; }
        public string Question { get; set; }
        public string Positive { get; set; } // positive answer (yes or no)
        public string Negative { get; set; } // negative answer (yes or no) 
        public string Answer { get; set; } // positive or negative 
        public string Type { get; set; }
        // public session()
        // {
        // }
    }
}
