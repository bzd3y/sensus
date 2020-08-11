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
using CsvHelper.Configuration;
using CsvHelper;

namespace Sensus.MindTrailsBehind
{
    public sealed class SessionMap : ClassMap<SessionModel>
    {
        public SessionMap()
        {
            Map(m => m.block);
            Map(m => m.name);
            Map(m => m.title);
            Map(m => m.word1);
            Map(m => m.word2);
            Map(m => m.statement1);
            Map(m => m.statement2);
            Map(m => m.question);
            Map(m => m.positive);
            Map(m => m.negative);
            Map(m => m.answer);
            Map(m => m.type);
        }
    }
}
