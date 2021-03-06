﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sensus
{
	public class ProtocolStateDatum : Datum
	{
		/// <summary>
		/// The name of the <see cref="Protocol"/> that changed state and generated this datum.
		/// </summary>
		public string ProtocolName { get; set; }
		/// <summary>
		/// The new <see cref="ProtocolState"/> of the <see cref="Protocol"/> that generated this datum.
		/// </summary>
		public ProtocolState State { get; set; }

		public ProtocolStateDatum(string protocolName, ProtocolState state, DateTimeOffset timestamp) : base(timestamp)
		{
			ProtocolName = protocolName;
			State = state;
		}

		public override string DisplayDetail
		{
			get
			{
				return $"{ProtocolName} state changd to {State}.";
			}
		}

		public override object StringPlaceholderValue
		{
			get
			{
				return $"{ProtocolName}: {State}";
			}
		}
	}
}
