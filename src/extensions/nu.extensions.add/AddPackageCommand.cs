// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace nu.extensions.add
{
    using System;
    using core.Commands;
    using core.Configuration;
    using core.FileSystem;
    using core.Nugs;
    using Magnum.Logging;

    public class AddPackageCommand :
        Command
    {
        readonly ILogger _logger = Logger.GetLogger<AddPackageCommand>();
        readonly string _name;
        readonly NugsDirectory _nugsDirectory;
        readonly ProjectConfiguration _projectConfiguration;
        readonly FileSystem _fileSystem;

        public AddPackageCommand(string name, NugsDirectory nugsDirectory, ProjectConfiguration projectConfiguration, FileSystem fileSystem)
        {
            _name = name;
            _nugsDirectory = nugsDirectory;
            _projectConfiguration = projectConfiguration;
            _fileSystem = fileSystem;
        }

        public void Execute()
        {
            if (_projectConfiguration == null)
                throw new Exception("there is no project");

            //is the nug already installed?

            //if it is, what version?

            //install
            var package = _nugsDirectory.GetNug(_name);
            
                //TODO: should this be hidden behind another 'directory'?
                var libName = _projectConfiguration["project.librarydirectoryname"];
                var libDir = _projectConfiguration.ProjectRoot.GetChildDirectory(libName);
                _logger.Debug(x => x.Write("'lib' dir is located at '{0}'", libDir.Name));
                _fileSystem.CreateDirectory(libDir);


                var targetPackageDir = libDir.GetChildDirectory(package.NugName);
                _fileSystem.CreateDirectory(targetPackageDir);


                foreach (var file in package.GetFiles())
                {
                    var fileWriteTo = targetPackageDir.GetChildFile(file.Name.GetName());
                    _fileSystem.Write(fileWriteTo.Name.GetPath(), file.Name.GetPath());
                }

//                _projectConfiguration.InstalledNugs.Add(new InstalledNugInformation()
//                    {
//                        Name = package.NugName,
//                        Version = package.Version
//                    });
        }
    }
}