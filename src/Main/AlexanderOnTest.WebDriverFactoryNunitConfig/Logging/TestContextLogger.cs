// <copyright>
// Copyright 2018 Alexander Dunn
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
// </copyright>

using System;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace AlexanderOnTest.WebDriverFactoryNunitConfig.Logging
{
    /// <summary>
    /// A simple default ILogger that logs information only to the NUnit TestContext when enabled.
    /// </summary>
    public class TestContextLogger : ILogger
    {
        public TestContextLogger(bool isInformationEnabled)
        {
            IsInformationEnabled = isInformationEnabled;
        }

        private bool IsInformationEnabled { get;}
        
        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsInformationEnabled && logLevel == Microsoft.Extensions.Logging.LogLevel.Information)
            {
                TestContext.WriteLine($"Information: {formatter(state, exception)}");
            }
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) =>
            logLevel == Microsoft.Extensions.Logging.LogLevel.Information && IsInformationEnabled;

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }
        
        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}