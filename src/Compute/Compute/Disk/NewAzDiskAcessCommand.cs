﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.Azure.Commands.Compute.Automation.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Compute.Models;
using Microsoft.WindowsAzure.Commands.Utilities.Common;

namespace Microsoft.Azure.Commands.Compute.Automation
{
    [Cmdlet(VerbsCommon.New, ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "DiskAccess", DefaultParameterSetName = "DefaultParameter", SupportsShouldProcess = true)]
    [OutputType(typeof(PSDiskAccess))]
    public class NewAzureDiskAccess : ComputeAutomationBaseCmdlet
    {

        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        [ResourceGroupCompleter]
        public string ResourceGroupName { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        [Alias("DiskAssessName")]
        public string Name { get; set; }

        [Parameter(
            Position = 2,
            Mandatory = true,
            ValueFromPipeline = true)]
        [LocationCompleter("Microsoft.Compute/diskaccesses")]
        public string Location { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background")]
        public SwitchParameter AsJob { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();
            ExecuteClientAction(() =>
            {
                if (ShouldProcess(this.Name, VerbsCommon.New))
                {
                    string resourceGroupName = this.ResourceGroupName;
                    string diskName = this.Name;
                    DiskAccess disk = new Disk();
                    ComputeAutomationAutoMapperProfile.Mapper.Map<PSDisk, Disk>(this.Disk, disk);

                    var result = DisksClient.CreateOrUpdate(resourceGroupName, diskName, disk);
                    var psObject = new PSDisk();
                    ComputeAutomationAutoMapperProfile.Mapper.Map<DiskAccess, PSDisk>(result, psObject);
                    WriteObject(psObject);
                }
            });
        }
    }
}
