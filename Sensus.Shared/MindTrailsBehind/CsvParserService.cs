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
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;

namespace Sensus.MindTrailsBehind
{
    public class CsvParserService : ICsvParserService
    {
        public List<SessionModel> ReadCsvFileToEmployeeModel(string path)
        {
            try
            {
                using (var reader = new StreamReader(path, Encoding.Default))
                using (var csv = new CsvReader((IParser)reader))
                {
                    csv.Configuration.RegisterClassMap<SessionMap>();
                    var records = csv.GetRecords<SessionModel>().ToList();
                    return records;
                }
            }
            catch (UnauthorizedAccessException e)
            {
                throw new Exception(e.Message);
            }
            catch (FieldValidationException e)
            {
                throw new Exception(e.Message);
            }
            catch (CsvHelperException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<SessionModel> ReadCsvFileToSessionModel(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteNewCsvFile(string path, List<SessionModel> sessionModels)
        {
            throw new NotImplementedException();
        }
    }
}

