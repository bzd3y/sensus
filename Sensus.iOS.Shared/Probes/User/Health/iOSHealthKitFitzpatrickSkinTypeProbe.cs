﻿// Copyright 2014 The Rector & Visitors of the University of Virginia
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

using HealthKit;
using Foundation;
using System;
using System.Threading;
using System.Collections.Generic;
using Sensus;
using Sensus.Probes.User.Health;
using Syncfusion.SfChart.XForms;
using System.Threading.Tasks;

namespace Sensus.iOS.Probes.User.Health
{
    public class iOSHealthKitFitzpatrickSkinTypeProbe : iOSHealthKitProbe
    {
        public sealed override string DisplayName
        {
            get
            {
                return "Fitzpatrick Skin Type";
            }
        }

        public override Type DatumType
        {
            get
            {
                return typeof(FitzpatrickSkinTypeDatum);
            }
        }

        public override int DefaultPollingSleepDurationMS
        {
            get
            {
                return int.MaxValue;
            }
        }

        public iOSHealthKitFitzpatrickSkinTypeProbe()
            : base(HKCharacteristicType.Create(HKCharacteristicTypeIdentifier.FitzpatrickSkinType))
        {
        }

        protected override Task<List<Datum>> PollAsync(CancellationToken cancellationToken)
        {
            List<Datum> data = new List<Datum>();

            NSError error;
            HKFitzpatrickSkinTypeObject skinType = HealthStore.GetFitzpatrickSkinType(out error);

            if (error == null)
            {
                if (skinType.SkinType == HKFitzpatrickSkinType.I)
                {
                    data.Add(new FitzpatrickSkinTypeDatum(DateTimeOffset.Now, FitzpatrickSkinType.TypeI));
                }
                else if (skinType.SkinType == HKFitzpatrickSkinType.II)
                {
                    data.Add(new FitzpatrickSkinTypeDatum(DateTimeOffset.Now, FitzpatrickSkinType.TypeII));
                }
                else if (skinType.SkinType == HKFitzpatrickSkinType.III)
                {
                    data.Add(new FitzpatrickSkinTypeDatum(DateTimeOffset.Now, FitzpatrickSkinType.TypeIII));
                }
                else if (skinType.SkinType == HKFitzpatrickSkinType.IV)
                {
                    data.Add(new FitzpatrickSkinTypeDatum(DateTimeOffset.Now, FitzpatrickSkinType.TypeIV));
                }
                else if (skinType.SkinType == HKFitzpatrickSkinType.V)
                {
                    data.Add(new FitzpatrickSkinTypeDatum(DateTimeOffset.Now, FitzpatrickSkinType.TypeV));
                }
                else if (skinType.SkinType == HKFitzpatrickSkinType.VI)
                {
                    data.Add(new FitzpatrickSkinTypeDatum(DateTimeOffset.Now, FitzpatrickSkinType.TypeVI));
                }
                else if (skinType.SkinType == HKFitzpatrickSkinType.NotSet)
                {
                    data.Add(new FitzpatrickSkinTypeDatum(DateTimeOffset.Now, FitzpatrickSkinType.NotSet));
                }
                else
                {
                    throw new Exception("User has not provided -- or has not allowed access to -- their fitzpatrick skin type.");
                }
            }
            else
            {
                throw new Exception("Error reading Fitzpatrick skin type:  " + error.Description);
            }

            return Task.FromResult(data);
        }

        protected override ChartSeries GetChartSeries()
        {
            throw new NotImplementedException();
        }

        protected override ChartDataPoint GetChartDataPointFromDatum(Datum datum)
        {
            throw new NotImplementedException();
        }

        protected override ChartAxis GetChartPrimaryAxis()
        {
            throw new NotImplementedException();
        }

        protected override RangeAxisBase GetChartSecondaryAxis()
        {
            throw new NotImplementedException();
        }
    }
}