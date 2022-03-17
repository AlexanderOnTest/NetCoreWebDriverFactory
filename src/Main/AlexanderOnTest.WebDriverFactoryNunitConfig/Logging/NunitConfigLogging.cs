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

using Microsoft.Extensions.Logging;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AlexanderOnTest.WebDriverFactoryNunitConfig.Logging
{
    public static class NunitConfigLogging
    {
        private static ILoggerFactory _factory = null;

        public static void ConfigureLogger(ILoggerFactory factory)
        {
            factory.AddProvider(new TestContextLoggerProvider(true));
        }

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = new LoggerFactory();
                    ConfigureLogger(_factory);
                }
                return _factory;
            }
            set { _factory = value; }
        }
        
        public static ILogger CreateLogger() => LoggerFactory.CreateLogger("Information");
    }
}