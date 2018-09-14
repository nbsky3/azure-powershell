// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Commands.Network.Models;
using Microsoft.Azure.Management.Network.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.Network
{
    [Cmdlet(VerbsCommon.Set, ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "NetworkProfileContainerNicConfig", SupportsShouldProcess = true), OutputType(typeof(PSNetworkProfile))]
    public partial class SetAzureNetworkProfileContainerNetworkInterfaceConfigCommand : AzureNetworkProfileContainerNetworkInterfaceConfigBase
    {
        [Parameter(
            Mandatory = true,
            HelpMessage = "The name of the Container Nic Ip Configuration")]
            [ValidateNotNullOrEmpty]
        public override string Name { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The reference of the network profile resource.",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public PSNetworkProfile NetworkProfile { get; set; }

        public override void Execute()
        {
            base.Execute();

            var vContainerNetworkInterfaceConfigurationsIndex = this.NetworkProfile.ContainerNetworkInterfaceConfigurations.IndexOf(
                this.NetworkProfile.ContainerNetworkInterfaceConfigurations.SingleOrDefault(
                    resource => string.Equals(resource.Name, this.Name, System.StringComparison.CurrentCultureIgnoreCase)));
            if (vContainerNetworkInterfaceConfigurationsIndex == -1)
            {
                throw new ArgumentException("ContainerNetworkInterfaceConfigurations with the specified name does not exist");
            }
            var vContainerNetworkInterfaceConfigurations = new PSContainerNetworkInterfaceConfiguration();

            vContainerNetworkInterfaceConfigurations.Name = this.Name;
            vContainerNetworkInterfaceConfigurations.IpConfigurations = this.IpConfiguration;
            this.NetworkProfile.ContainerNetworkInterfaceConfigurations[vContainerNetworkInterfaceConfigurationsIndex] = vContainerNetworkInterfaceConfigurations;
            WriteObject(this.NetworkProfile, true);
        }
    }
}
