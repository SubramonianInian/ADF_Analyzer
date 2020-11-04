using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Rest;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Text.RegularExpressions;

namespace ADF_Analyzer
{
    public partial class ADFAnlayzer : Form
    {

        public ADFAnlayzer()
        {
            InitializeComponent();
        }
        static DataFactoryManagementClient client;

        private void TriggerPipeline(object sender, EventArgs e)
        {
            string tenantID = tenant.Text;
            string applicationId = appID.Text;
            string authenticationKey = authKey.Text;
            string subscriptionId = subId.Text;
            string resourceGroup = resourceGrp.Text;
            string dataFactoryName = dsName.Text;
            string pipelineName = plName.Text;
            string pipelineParameters = Regex.Replace(plParam.Text, @"\s+", string.Empty);

            // Authorize Azure
            try
            {
                outputDetails.Text = string.Empty;
                outputDetails.Text = "Initiating Azure Authorization...";
                AuthenticationContext context = new AuthenticationContext("https://login.windows.net/" + tenantID);
                ClientCredential cc = new ClientCredential(applicationId, authenticationKey);
                AuthenticationResult result = context.AcquireTokenAsync("https://management.azure.com/", cc).Result;
                ServiceClientCredentials cred = new TokenCredentials(result.AccessToken);

                client = new DataFactoryManagementClient(cred)
                {
                    SubscriptionId = subscriptionId
                };

                outputDetails.Text = outputDetails.Text + "\r\nAzure Authorization Complete...";
            }
            catch (Exception ex)
            {
                outputDetails.Text = outputDetails.Text + "\r\n Azure Authorization failed..." + "\r\n" + ex.InnerException;
            }

            // Set parameters {parameter_name, value}

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string[] items = pipelineParameters.TrimEnd(';').Split(';');
            foreach (string item in items)
            {
                string[] keyValue = item.Split('=');
                parameters.Add(keyValue[0], keyValue[1]);
            }

            // Trigger Azure Datafactory Pipeline
            try
            {
                outputDetails.Text = outputDetails.Text + "\r\nInitiating pipeline run...";
                CreateRunResponse runResponse = client.Pipelines.CreateRunWithHttpMessagesAsync(
                    resourceGroup,
                    dataFactoryName,
                    pipelineName,
                    parameters: parameters
                ).Result.Body;

                outputDetails.Text = outputDetails.Text + "\r\nPipeline run ID: " + runResponse.RunId;

                outputDetails.Text = outputDetails.Text + "\r\nChecking pipeline run status...";
                PipelineRun pipelineRun;
                while (true)
                {
                    pipelineRun = client.PipelineRuns.Get(
                        resourceGroup, dataFactoryName, runResponse.RunId);
                    outputDetails.Text = outputDetails.Text + "\r\nStatus: " + pipelineRun.Status;
                    if (pipelineRun.Status == "InProgress" || pipelineRun.Status == "Queued")
                        System.Threading.Thread.Sleep(15000);
                    else
                        break;
                }
            }
            catch (Exception ex)
            {
                outputDetails.Text = "Pipeline Trigger failed..." + "\r\n" + ex.InnerException; ;
            }

        }

        private void ping(object sender, EventArgs e)
        {
            try
            {
                string tenantID = tenant.Text;
                string applicationId = appID.Text;
                string authenticationKey = authKey.Text;
                string subscriptionId = subId.Text;

                outputDetails.Text = string.Empty;
                outputDetails.Text = "Initiating Azure Authorization...";
                AuthenticationContext context = new AuthenticationContext("https://login.windows.net/" + tenantID);
                ClientCredential cc = new ClientCredential(applicationId, authenticationKey);
                AuthenticationResult result = context.AcquireTokenAsync("https://management.azure.com/", cc).Result;
                ServiceClientCredentials cred = new TokenCredentials(result.AccessToken);

                client = new DataFactoryManagementClient(cred)
                {
                    SubscriptionId = subscriptionId
                };

                outputDetails.Text = outputDetails.Text + "\r\n Azure Authorization Successful...!";
            }
            catch (Exception ex)
            {
                outputDetails.Text = outputDetails.Text + "\r\n Ping Failed...!" + "\r\n" + ex.InnerException;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void subscriptionId_TextChanged(object sender, EventArgs e)
        {

        }

        private void outputDetails_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
